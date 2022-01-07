using System;
using System.Collections.Generic;
using GameLabGraz.LimeSurvey.Data;
using GameLabGraz.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;
using UnityEngine.UI;
using InputField = GameLabGraz.UI.InputField;

namespace GameLabGraz.LimeSurvey
{
    public class LimeSurveyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text questionText;
        [SerializeField] private GameObject questionContent;

        private readonly Dictionary<int, QuestionGroup> _questionGroups = new Dictionary<int, QuestionGroup>();
        private readonly List<Question> _questions = new List<Question>();

        private int _questionIndex;

        private QuestionGroup CurrentGroup => _questionGroups[CurrentQuestion.GID];
        private Question CurrentQuestion => _questions[_questionIndex];

        private void Start()
        {
            foreach(var questionGroup in LimeSurveyManager.Instance.GetQuestionGroups())
            {
                _questionGroups[questionGroup.GID] = questionGroup;
                _questions.AddRange(questionGroup.Questions);
            }

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
                    CreatePointMatrix(5);
                    break;
                case QuestionType.TenPointMatrix:
                    CreatePointMatrix(10);
                    break;
                case QuestionType.Matrix:
                    CreateMatrix();
                    break;
                default:
                    Debug.LogWarning($"Unknown Question Type: {_questions[questionIndex].QuestionType}");
                    break;
            }

            CreateButtons();
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
            var options = new List<Toggle>();
            foreach (var answerOption in CurrentQuestion.AnswerOptions)
            {
                var option = ((GameObject)Instantiate(UIContent.RadioButtonGroup, questionContent.transform)).GetComponent<Toggle>();
                option.GetComponentInChildren<TMP_Text>().text = answerOption.AnswerText;
                options.Add(option);
            }
            if (!CurrentQuestion.Mandatory)
            {
                var option = ((GameObject)Instantiate(UIContent.RadioButtonGroup, questionContent.transform)).GetComponent<Toggle>();
                option.GetComponentInChildren<TMP_Text>().text = "NA";

                if (string.IsNullOrEmpty(CurrentQuestion.Answer))
                    option.isOn = true;

                options.Add(option);
            }

            foreach (var option in options)
            {
                var answer = option.GetComponentInChildren<TMP_Text>().text;

                if(CurrentQuestion.Answer != null && (string)CurrentQuestion.Answer == answer)
                    option.isOn = true;

                option.onValueChanged.AddListener(value =>
                {
                    if(value)
                    {
                        if (!CurrentQuestion.Mandatory && answer == "NA")
                        {
                            CurrentQuestion.Answer = string.Empty;
                        }
                        else
                        {
                            CurrentQuestion.Answer = answer;
                        }
                    }

                    var toggle = options.Find(op => op != option && op.isOn);
                    if (toggle != null)
                        toggle.isOn = false;
                    else if (value == false)
                        option.isOn = true;
                });
            }
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
                    if (subQuestion.Answer != null && !string.IsNullOrWhiteSpace((string)subQuestion.Answer))
                    {
                        toggle.isOn = true;
                        inputField.text = (string)subQuestion.Answer;
                    }

                    inputField.onValueChanged.AddListener(input =>
                    {
                        subQuestion.Answer = string.IsNullOrWhiteSpace(input) ? null : input;
                        toggle.isOn = !string.IsNullOrWhiteSpace((string)subQuestion.Answer);
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
                        toggle.isOn = (string)subQuestion.Answer == "Y";

                    toggle.onValueChanged.AddListener(answer =>
                    {
                        subQuestion.Answer = answer ? "Y" : string.Empty;
                    });
                }

            }
        }

        private void CreatePointMatrix(int optionSize)
        {
            var rowGroup = ((GameObject)Instantiate(UIContent.VerticalLayoutGroup, questionContent.transform)).GetComponent<VerticalLayoutGroup>();
            rowGroup.spacing = 30;

            // First Row
            var firstColumnGroup = ((GameObject)Instantiate(UIContent.HorizontalLayoutGroup, rowGroup.transform)).GetComponent<HorizontalLayoutGroup>();
            var placeholder = Instantiate(new GameObject(), firstColumnGroup.transform).AddComponent<LayoutElement>();
            placeholder.minWidth = 300;
            placeholder.preferredWidth = 300;
            placeholder.flexibleWidth = 0;

            var answerOptions = new List<AnswerOption>();
            for (var point = 1; point <= optionSize; point++)
            {
                var pointLabelObj = Instantiate(UIContent.Text, firstColumnGroup.transform) as GameObject;
                pointLabelObj.GetComponent<TMP_Text>().text = $"{point}";
                pointLabelObj.GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Center;
                pointLabelObj.GetComponent<LayoutElement>().preferredWidth = 40;
                pointLabelObj.GetComponent<LayoutElement>().flexibleWidth = 0;

                answerOptions.Add(new AnswerOption($"{point}", $"{point}", point - 1));
            }
            if (!CurrentQuestion.Mandatory)
            {
                var naLabelObj = Instantiate(UIContent.Text, firstColumnGroup.transform) as GameObject;
                naLabelObj.GetComponent<TMP_Text>().text = "No answer";
                naLabelObj.GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Center;
                naLabelObj.GetComponent<LayoutElement>().preferredWidth = 40;
                naLabelObj.GetComponent<LayoutElement>().flexibleWidth = 0;

                answerOptions.Add(new AnswerOption("NA", "NA", answerOptions.Count));
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
                for (var point = 1; point <= optionSize; point++)
                {
                    var toggle = ((GameObject)Instantiate(UIContent.RadioButton, columnGroup.transform)).GetComponent<Toggle>();
                    toggles.Add(toggle);
                }

                if (!CurrentQuestion.Mandatory)
                {
                    var toggle = ((GameObject)Instantiate(UIContent.RadioButton, columnGroup.transform)).GetComponent<Toggle>();
                    if (string.IsNullOrEmpty(subQuestion.Answer))
                        toggle.isOn = true;
                    toggles.Add(toggle);
                }

                CurrentQuestion.AnswerOptions = answerOptions;
                SetupRadioButtonOnValueChange(toggles, subQuestion);
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


        private void CreatePointOptions(int optionSize, Transform optionContent = null)
        {
            if (optionContent == null)
                optionContent = questionContent.transform;

            var optionGroup = Instantiate(UIContent.HorizontalLayoutGroup, optionContent) as GameObject;
            if (optionGroup == null) return;

            var toggles = new List<Toggle>();

            for (var value = 1; value <= optionSize; value++)
            {
                var optionObj = Instantiate(UIContent.RadioButtonGroup, optionGroup.transform) as GameObject;
                if (optionObj == null) continue;

                var toggle = optionObj.GetComponent<Toggle>();
                toggle.GetComponentInChildren<TMP_Text>().text = $"{value}";

                toggles.Add(toggle);
                CurrentQuestion.AnswerOptions.Add(new AnswerOption($"{value}", $"{value}", value - 1));
            }

            if (!CurrentQuestion.Mandatory)
            {
                var optionObj = Instantiate(UIContent.RadioButtonGroup, optionGroup.transform) as GameObject;
                if (optionObj == null) return;

                var toggle = optionObj.GetComponent<Toggle>();
                toggle.GetComponentInChildren<TMP_Text>().text = "NA";

                if (string.IsNullOrEmpty(CurrentQuestion.Answer))
                    toggle.isOn = true;

                toggles.Add(toggle);
                CurrentQuestion.AnswerOptions.Add(new AnswerOption("NA", "NA", CurrentQuestion.AnswerOptions.Count));
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

        private void CreateButtons()
        {
            var buttonGroup = Instantiate(UIContent.HorizontalLayoutGroup, questionContent.transform) as GameObject;
            if (buttonGroup == null) return;

            // Previous Button
            var prevButton = ((GameObject)Instantiate(UIContent.Button, buttonGroup.transform)).GetComponent<Button>();
            prevButton.GetComponentInChildren<TMP_Text>().text = "Previous";
            prevButton.onClick.AddListener(() =>
            {
                ShowQuestion(--_questionIndex);
            });

            if (_questionIndex == 0)
                prevButton.interactable = false;

            // Next Button
            var nextButton = ((GameObject)Instantiate(UIContent.Button, buttonGroup.transform)).GetComponent<Button>();
            nextButton.GetComponentInChildren<TMP_Text>().text = "Next";
            nextButton.onClick.AddListener(() =>
            {
                if (CurrentQuestion.Mandatory && CurrentQuestion.Answer == null && 
                    !CurrentQuestion.SubQuestions.TrueForAll(subQuestion => subQuestion.Answer != null)) return;
                ShowQuestion(++_questionIndex);
            });
            if (_questionIndex == _questions.Count - 1)
            {
                nextButton.interactable = false;

                // Submit Button
                var submitButton = ((GameObject)Instantiate(UIContent.Button, questionContent.transform)).GetComponent<Button>();
                submitButton.GetComponentInChildren<TMP_Text>().text = "Submit";
                submitButton.onClick.AddListener(() =>
                {
                    LimeSurveyManager.Instance.UploadQuestionResponses(_questions);
                    ShowThanks();
                });
            }
        }

        private void ShowThanks()
        {
            ClearQuestionContent();
            questionText.text = "Thank you for submitting.";
            var closeButton = ((GameObject)Instantiate(UIContent.Button, questionContent.transform)).GetComponent<Button>();
            closeButton.GetComponentInChildren<TMP_Text>().text = "Close";
            closeButton.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });
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