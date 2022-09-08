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

        [HideInInspector] public ErrorEvent OnError;
        [HideInInspector] public WarningEvent OnWarning;
        [HideInInspector] public UnityEvent OnStartLoadQuestions;
        [HideInInspector] public UnityEvent OnQuestionsLoaded;
        [HideInInspector] public UnityEvent OnStartSubmission;
        [HideInInspector] public SubmissionEvent OnSubmissionFinished;

        public int ResponseID
        {
            get => responseID;
            set => responseID = value;
        }

        private void Awake()
        {
            LimeSurveyManager.Instance.OnLoggedIn.AddListener((sessionKey) => { StartCoroutine(Initialize()); });
        }

        private IEnumerator Initialize()
        {
            Debug.Log("Init start");
            OnStartLoadQuestions.Invoke();
            
            var cd = new CoroutineWithData(this, LimeSurveyManager.Instance.GetQuestionGroups());
            yield return cd.Coroutine;

            foreach (var questionGroup in (List<QuestionGroup>)cd.Result)
            {
                _questionGroups[questionGroup.GID] = questionGroup;
                _questions.AddRange(questionGroup.Questions);
            }
            SetupButtons();
            ShowQuestion(0);
            
            OnQuestionsLoaded.Invoke();
            Debug.Log("Init end");
        }


        private void ShowQuestion(int questionIndex)
        {
            if (questionIndex >= _questions.Count)
                return;

            ClearQuestionContent();
            
            Debug.Log($"new Question: {CurrentQuestion.Title}: mandatory: {CurrentQuestion.Mandatory}, type: {CurrentQuestion.QuestionType}");

            questionText.text = $"{CurrentGroup.GroupName}\n";
            questionText.text += CurrentQuestion.Mandatory ? $"* {CurrentQuestion.QuestionText}" : CurrentQuestion.QuestionText;
            
            switch (CurrentQuestion.QuestionType)
            {
                case QuestionType.Text:
                case QuestionType.ShortText:
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
                case QuestionType.IntNumber:
                    CreateIntNumberText();
                    break;
                default:
                    var question = _questions[questionIndex];
                    var warningStr = $"Unknown Question Type '{question.QuestionType}'";
                    var detailStr = $"Question '{question.Title}' uses unsupported type '{question.GetTypeString()}'.";
                    Debug.Log($"[LimeSurvey] WARNING: {warningStr}: {detailStr} --- '{question.QuestionText}'");
                    OnWarning.Invoke(warningStr, detailStr);
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
                AdaptNextButton();
            } ));
        }
        
        private void CreateIntNumberText()
        {
            var inputField = ((GameObject)Instantiate(UIContent.Input, questionContent.transform)).GetComponent<InputField>();
            inputField.GetComponent<LayoutElement>().minHeight = 300;
            inputField.lineType = TMP_InputField.LineType.SingleLine;
            inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            inputField.characterLimit = 10;

            var placeholder = inputField.placeholder?.GetComponent<TMP_Text>();
            if (placeholder)
            {
                placeholder.text = "Enter number...";
            }
            
            if (CurrentQuestion.Answer != null)
                inputField.text = CurrentQuestion.Answer;

            inputField.onValueChanged.AddListener((answer =>
            {
                CurrentQuestion.Answer = answer;
                AdaptNextButton();
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
                        AdaptNextButton();
                    });
                    toggle.onValueChanged.AddListener(value =>
                    {
                        if (value && string.IsNullOrWhiteSpace(inputField.text))
                            toggle.isOn = false;
                        AdaptNextButton();
                    });
                }
                else
                {
                    if (subQuestion.Answer != null)
                        toggle.isOn = subQuestion.Answer == "Y";

                    toggle.onValueChanged.AddListener(answer =>
                    {
                        subQuestion.Answer = answer ? "Y" : string.Empty;
                        AdaptNextButton();
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
                    
                    AdaptNextButton();
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
                if (!MandatoryOK()) return;
                
                ShowQuestion(++_questionIndex);
            });
            
            // Submit Button
            submitButton.onClick.AddListener(() =>
            {
                if (!MandatoryOK()) return;

                ClearQuestionContent();
                
                // Disable Buttons
                prevButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);
                submitButton.interactable = false;

                questionText.text = "Submitting Responses ...";

                StartCoroutine(SubmitResponses());
            });
        }

        private IEnumerator SubmitResponses()
        {
            OnStartSubmission.Invoke();
            
            var cd = new CoroutineWithData(this, LimeSurveyManager.Instance.UploadQuestionResponses(_questions, responseID));
            yield return cd.Coroutine;

            responseID = (int)cd.Result;
            if (responseID != -1)
                OnSubmissionFinished?.Invoke(responseID);
            else
            {
                var errorStr = "Unable to submit responses.";
                Debug.Log("[LimeSurvey] ERROR OnSubmission: " + errorStr);
                OnError.Invoke(errorStr, LimeSurveyManager.Instance.GetLastError());
            }
        }

        private void EnableButtons()
        {
            prevButton.gameObject.SetActive(_questionIndex != 0);
            nextButton.gameObject.SetActive(_questionIndex != _questions.Count - 1);
            submitButton.gameObject.SetActive(_questionIndex == _questions.Count - 1);

            prevButton.interactable = _questionIndex != 0;
            nextButton.interactable = !CurrentQuestion.Mandatory || CurrentQuestion.HasAnswer();
            submitButton.interactable = !CurrentQuestion.Mandatory || CurrentQuestion.HasAnswer();
        }

        private void AdaptNextButton()
        {
            var nextIsInteractable = !CurrentQuestion.Mandatory || CurrentQuestion.HasAnswer();
            if(nextButton.isActiveAndEnabled)
                nextButton.interactable = nextIsInteractable;
            if (submitButton.isActiveAndEnabled)
                submitButton.interactable = nextIsInteractable;
        }

        private void ClearQuestionContent()
        {
            for (var childIndex = 0; childIndex < questionContent.transform.childCount; childIndex++)
            {
                var content = questionContent.transform.GetChild(childIndex);
                Destroy(content.gameObject);
            }
        }

        private bool MandatoryOK()
        {
            if (CurrentQuestion.Mandatory && CurrentQuestion.SubQuestions.Count == 0 &&
                CurrentQuestion.Answer == null) return false;

            if (CurrentQuestion.Mandatory && CurrentQuestion.QuestionType == QuestionType.MultipleChoice &&
                CurrentQuestion.SubQuestions.TrueForAll(subQuestion => subQuestion.Answer == null)) return false;

            if (CurrentQuestion.Mandatory && CurrentQuestion.QuestionType != QuestionType.MultipleChoice &&
                !CurrentQuestion.SubQuestions.TrueForAll(subQuestion => subQuestion.Answer != null)) return false;

            return true;
        }
    }
}