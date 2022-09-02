using System;
using System.Collections;
using System.Collections.Generic;
using GameLabGraz.LimeSurvey.Data;
using GameLabGraz.UI;
using GEAR.Gadgets.Coroutine;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;
using InputField = GameLabGraz.UI.InputField;

namespace GameLabGraz.LimeSurvey
{
    [Serializable]
    public class SubmissionEvent : UnityEvent<int>{}

    public class LimeSurveyView : MonoBehaviour
    {
        [SerializeField] private int responseID = -1;

        [SerializeField] private TMP_Text questionText;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private GameObject questionContent;
        [SerializeField] private Button prevButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button submitButton;

        private readonly Dictionary<int, QuestionGroup> _questionGroups = new Dictionary<int, QuestionGroup>();
        private readonly List<Question> _questions = new List<Question>();

        private int _questionIndex;

        public QuestionGroup CurrentGroup => _questionGroups[CurrentQuestion.GID];
        public Question CurrentQuestion => _questions[_questionIndex];

        public SubmissionEvent OnSubmission;

        public int ResponseID
        {
            get => responseID;
            set => responseID = value;
        }

        private void Start()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            var cd = new CoroutineWithData(this, LimeSurveyManager.Instance.GetQuestionGroups());
            yield return cd.Coroutine;

            foreach (var questionGroup in (List<QuestionGroup>)cd.Result)
            {
                _questionGroups[questionGroup.GID] = questionGroup;
                _questions.AddRange(questionGroup.Questions);
            }
            SetupButtons();
            ShowQuestion(0);
        }


        private void ShowQuestion(int questionIndex)
        {
            if (questionIndex >= _questions.Count)
                return;

            ClearQuestionContent();

            questionText.text = $"{CurrentGroup.GroupName}\n";
            questionText.text += CurrentQuestion.Mandatory ? $"* {CurrentQuestion.QuestionText}" : CurrentQuestion.QuestionText;

            switch (CurrentQuestion.QuestionType)
            {
                case QuestionType.Text:
                    CreateFreeText();
                    break;
                case QuestionType.ListRadio:
                    CreateListRadio();
                    break;
                case QuestionType.MultipleChoice:
                    CreateMultipleChoice();
                    break;
                case QuestionType.FivePointChoice:
                    CreatePointOptions(5);
                    break;
                case QuestionType.FivePointMatrix:
                case QuestionType.TenPointMatrix:
                case QuestionType.Matrix:
                    CreateMatrix();
                    break;
                default:
                    Debug.LogWarning($"Unknown Question Type: {_questions[questionIndex].QuestionType}");
                    break;
            }

            Invoke("ScrollToTop", 0.1f);

            EnableButtons();
        }
    
        private void ScrollToTop()
        {
            scrollRect.verticalNormalizedPosition = 1;
        }

        private void CreateFreeText()
        {
            var inputField = ((GameObject)Instantiate(UIContent.Input, questionContent.transform)).GetComponent<InputField>();
            inputField.GetComponent<LayoutElement>().minHeight = 300;
            inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
            if (CurrentQuestion.Answer != null)
                inputField.text = CurrentQuestion.Answer;

            inputField.onValueChanged.AddListener((answer =>
            {
                CurrentQuestion.Answer = answer;
            } ));
        }

        private void CreateListRadio()
        {
            var toggles = new List<Toggle>();
            foreach (var answerOption in CurrentQuestion.AnswerOptions)
            {
                var toggle = ((GameObject)Instantiate(UIContent.RadioButtonGroup, questionContent.transform)).GetComponent<Toggle>();
                toggle.GetComponentInChildren<TMP_Text>().text = answerOption.AnswerText;

                if (answerOption.AnswerCode == "NA" && string.IsNullOrEmpty(CurrentQuestion.Answer))
                    toggle.isOn = true;

                
                toggles.Add(toggle);
            }

            SetupRadioButtonOnValueChange(toggles, CurrentQuestion);
        }

        private void CreateMultipleChoice()
        {
            foreach (var subQuestion in CurrentQuestion.SubQuestions)
            {
                var hGroup = Instantiate(UIContent.HorizontalLayoutGroup, questionContent.transform) as GameObject;

                var toggle = ((GameObject)Instantiate(UIContent.ToggleGroup, hGroup.transform)).GetComponent<Toggle>();
                toggle.GetComponentInChildren<TMP_Text>().text = subQuestion.QuestionText;

                if (subQuestion.Title == "other")
                {
                    var inputField = ((GameObject)Instantiate(UIContent.Input, hGroup.transform)).GetComponent<InputField>();
                    if (subQuestion.Answer != null && !string.IsNullOrWhiteSpace(subQuestion.Answer))
                    {
                        toggle.isOn = true;
                        inputField.text = subQuestion.Answer;
                    }

                    inputField.onValueChanged.AddListener(input =>
                    {
                        subQuestion.Answer = string.IsNullOrWhiteSpace(input) ? null : input;
                        toggle.isOn = !string.IsNullOrWhiteSpace(subQuestion.Answer);
                    });
                    toggle.onValueChanged.AddListener(value =>
                    {
                        if (value && string.IsNullOrWhiteSpace(inputField.text))
                            toggle.isOn = false;
                    });
                }
                else
                {
                    if (subQuestion.Answer != null)
                        toggle.isOn = subQuestion.Answer == "Y";

                    toggle.onValueChanged.AddListener(answer =>
                    {
                        subQuestion.Answer = answer ? "Y" : string.Empty;
                    });
                }

            }
        }

        private void CreateMatrix()
        {
            var rowGroup = ((GameObject)Instantiate(UIContent.VerticalLayoutGroup, questionContent.transform)).GetComponent<VerticalLayoutGroup>();
            rowGroup.spacing = 30;

            // Header
            var headerGroup = ((GameObject)Instantiate(UIContent.HorizontalLayoutGroup, rowGroup.transform)).GetComponent<HorizontalLayoutGroup>();
            var placeholder = Instantiate(new GameObject(), headerGroup.transform).AddComponent<LayoutElement>();
            placeholder.minWidth = 300;
            placeholder.preferredWidth = 300;
            placeholder.flexibleWidth = 0;

            foreach(var answerOption in CurrentQuestion.AnswerOptions)
            {
                var optionObj = Instantiate(UIContent.Text, headerGroup.transform) as GameObject;
                optionObj.GetComponent<TMP_Text>().text = answerOption.AnswerText;
                optionObj.GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Center;
                optionObj.GetComponent<LayoutElement>().preferredWidth = 40;
                optionObj.GetComponent<LayoutElement>().flexibleWidth = 0;
            }

            // SubQuestions
            foreach (var subQuestion in CurrentQuestion.SubQuestions)
            {
                var columnGroup = ((GameObject)Instantiate(UIContent.HorizontalLayoutGroup, rowGroup.transform)).GetComponent<HorizontalLayoutGroup>();
                var subQuestionObj = Instantiate(UIContent.Text, columnGroup.transform) as GameObject;
                subQuestionObj.GetComponent<TMP_Text>().text = subQuestion.QuestionText;
                subQuestionObj.GetComponent<LayoutElement>().minWidth = 300;
                subQuestionObj.GetComponent<LayoutElement>().preferredWidth = 300;
                subQuestionObj.GetComponent<LayoutElement>().flexibleWidth = 0;

                var toggles = new List<Toggle>();
                foreach (var answerOption in CurrentQuestion.AnswerOptions)
                {
                    var toggle = ((GameObject)Instantiate(UIContent.RadioButton, columnGroup.transform)).GetComponent<Toggle>();
                    toggles.Add(toggle);
                    if (answerOption.AnswerCode == "NA" && string.IsNullOrEmpty(subQuestion.Answer))
                        toggle.isOn = true;
                }

                SetupRadioButtonOnValueChange(toggles, subQuestion);
            }
        }

        private void CreatePointOptions(int optionSize)
        {
            var optionGroup = Instantiate(UIContent.HorizontalLayoutGroup, questionContent.transform) as GameObject;
            if (optionGroup == null) return;

            var toggles = new List<Toggle>();
            foreach(var answerOption in CurrentQuestion.AnswerOptions)
            {
                var toggle = ((GameObject)Instantiate(UIContent.RadioButtonGroup, optionGroup.transform)).GetComponent<Toggle>();
                toggle.GetComponentInChildren<TMP_Text>().text = answerOption.AnswerText;

                if (answerOption.AnswerCode == "NA" && string.IsNullOrEmpty(CurrentQuestion.Answer))
                    toggle.isOn = true;
                
                toggles.Add(toggle);
            }
            SetupRadioButtonOnValueChange(toggles, CurrentQuestion);
        }

        private void SetupRadioButtonOnValueChange(List<Toggle> toggles, BaseQuestion question)
        {
            Assert.AreEqual(toggles.Count, CurrentQuestion.AnswerOptions.Count);

            for (var optionIndex = 0; optionIndex < toggles.Count; optionIndex++)
            {
                var toggle = toggles[optionIndex];
                var answer = CurrentQuestion.AnswerOptions[optionIndex];

                if (question.Answer != null && question.Answer == answer.AnswerCode)
                    toggle.isOn = true;

                toggle.onValueChanged.AddListener(value =>
                {
                    if (value)
                    {
                        if (!CurrentQuestion.Mandatory && answer.AnswerCode == "NA")
                        {
                            question.Answer = string.Empty;
                        }
                        else
                        {
                            question.Answer = answer.AnswerCode;
                        }
                    }

                    var oldToggle = toggles.Find(t => t != toggle && t.isOn);
                    if (oldToggle != null)
                        oldToggle.isOn = false;
                    else if (value == false)
                        toggle.isOn = true;
                });
            }
        }

        private void SetupButtons()
        {
            // Prev Button
            prevButton.onClick.AddListener(() =>
            {
                ShowQuestion(--_questionIndex);
            });

            // Next Button
            nextButton.onClick.AddListener(() =>
            {
                if (CurrentQuestion.Mandatory && CurrentQuestion.SubQuestions.Count == 0 &&
                    CurrentQuestion.Answer == null) return;

                if (CurrentQuestion.Mandatory && CurrentQuestion.QuestionType == QuestionType.MultipleChoice &&
                    CurrentQuestion.SubQuestions.TrueForAll(subQuestion => subQuestion.Answer == null)) return;

                if (CurrentQuestion.Mandatory && CurrentQuestion.QuestionType != QuestionType.MultipleChoice &&
                    !CurrentQuestion.SubQuestions.TrueForAll(subQuestion => subQuestion.Answer != null)) return;


                ShowQuestion(++_questionIndex);
            });
            
            // Submit Button
            submitButton.onClick.AddListener(() =>
            {
                ClearQuestionContent();
                
                // Disable Buttons
                prevButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
                submitButton.gameObject.SetActive(false);

                questionText.text = "Submitting Responses ...";

                StartCoroutine(SubmitResponses());
            });
        }

        private IEnumerator SubmitResponses()
        {
            var cd = new CoroutineWithData(this, LimeSurveyManager.Instance.UploadQuestionResponses(_questions, responseID));
            yield return cd.Coroutine;

            responseID = (int)cd.Result;
            if (responseID != -1)
                OnSubmission?.Invoke(responseID);
            else
                Debug.LogError("LimeSurveyView::OnSubmission: Unable to submit responses.");
        }

        private void EnableButtons()
        {
            prevButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(false);

            prevButton.interactable = true;
            nextButton.interactable = true;


            if (_questionIndex == 0)
                prevButton.interactable = false;
            if (_questionIndex == _questions.Count - 1)
            {
                nextButton.interactable = false;
                submitButton.gameObject.SetActive(true);
            }
        }

        private void ClearQuestionContent()
        {
            for (var childIndex = 0; childIndex < questionContent.transform.childCount; childIndex++)
            {
                var content = questionContent.transform.GetChild(childIndex);
                Destroy(content.gameObject);
            }
        }
    }
}