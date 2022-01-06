using System.Collections.Generic;
using GameLabGraz.LimeSurvey.Data;
using GameLabGraz.UI;
using UnityEngine;
using TMPro;
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
                default:
                    Debug.LogWarning($"Unknown Question Type: {_questions[questionIndex].QuestionType}");
                    break;
            }

            CreateButtons();
        }

        private void CreateFreeText()
        {
            var inputField = ((GameObject)Instantiate(UIContent.InputPrefab, questionContent.transform)).GetComponent<InputField>();
            inputField.GetComponent<LayoutElement>().minHeight = 300;
            inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
            if (CurrentQuestion.Answer != null)
                inputField.text = CurrentQuestion.Answer.ToString();

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
                var option = ((GameObject)Instantiate(UIContent.RadioButtonPrefab, questionContent.transform)).GetComponent<Toggle>();
                option.GetComponentInChildren<TMP_Text>().text = answerOption.AnswerText;
                options.Add(option);
            }
            if (!CurrentQuestion.Mandatory)
            {
                var option = ((GameObject)Instantiate(UIContent.RadioButtonPrefab, questionContent.transform)).GetComponent<Toggle>();
                option.GetComponentInChildren<TMP_Text>().text = "NA";

                if (string.IsNullOrEmpty(CurrentQuestion.Answer?.ToString()))
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

                var toggle = ((GameObject)Instantiate(UIContent.TogglePrefab, hGroup.transform)).GetComponent<Toggle>();
                toggle.GetComponentInChildren<TMP_Text>().text = subQuestion.QuestionText;

                if (subQuestion.Title == "other")
                {
                    var inputField = ((GameObject)Instantiate(UIContent.InputPrefab, hGroup.transform)).GetComponent<InputField>();
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
            foreach (var subQuestion in CurrentQuestion.SubQuestions)
            {
                var subQuestionGroup = Instantiate(UIContent.VerticalLayoutGroup, questionContent.transform) as GameObject;
                if (subQuestionGroup == null) return;

                var subQuestionObj = Instantiate(UIContent.TextPrefab, subQuestionGroup.transform) as GameObject;
                if (subQuestionObj != null) subQuestionObj.GetComponent<TMP_Text>().text = subQuestion.QuestionText;

                CreatePointOptions(optionSize, subQuestion, subQuestionGroup.transform);
            }
        }

        private void CreatePointOptions(int optionSize, SubQuestion subQuestion = null, Transform optionContent = null)
        {
            if (optionContent == null)
                optionContent = questionContent.transform;

            var optionGroup = Instantiate(UIContent.HorizontalLayoutGroup, optionContent) as GameObject;
            if (optionGroup == null) return;

            var options = new List<Toggle>();
            for (var value = 1; value <= optionSize; value++)
            {
                var optionObj = Instantiate(UIContent.RadioButtonPrefab, optionGroup.transform) as GameObject;
                if (optionObj == null) continue;

                var option = optionObj.GetComponent<Toggle>();
                option.GetComponentInChildren<TMP_Text>().text = $"{value}";
                options.Add(option);
            }

            if (!CurrentQuestion.Mandatory)
            {
                var optionObj = Instantiate(UIContent.RadioButtonPrefab, optionGroup.transform) as GameObject;
                if (optionObj == null) return;

                var option = optionObj.GetComponent<Toggle>();
                option.GetComponentInChildren<TMP_Text>().text = "NA";

                if (CurrentQuestion.SubQuestions.Count == 0 && string.IsNullOrEmpty(CurrentQuestion.Answer?.ToString()))
                    option.isOn = true;
                else if(subQuestion != null && string.IsNullOrEmpty(subQuestion.Answer?.ToString()))
                    option.isOn = true;

                options.Add(option);
            }
            SetupRadioButtonOnValueChange(options, subQuestion);
        }

        private void SetupRadioButtonOnValueChange(List<Toggle> options, SubQuestion subQuestion = null)
        {
            for (var optionIndex = 0; optionIndex < options.Count; optionIndex++)
            {
                var option = options[optionIndex];
                var answer = optionIndex + 1;

                if (subQuestion != null && subQuestion.Answer != null && (int)subQuestion.Answer == answer)
                    option.isOn = true;
                else if (CurrentQuestion.Answer != null && (int)CurrentQuestion.Answer == answer)
                    option.isOn = true;

                option.onValueChanged.AddListener(value =>
                {
                    if (value)
                    {
                        if (!CurrentQuestion.Mandatory && answer == options.Count)
                        {
                            if (subQuestion == null)
                                CurrentQuestion.Answer = string.Empty;
                            else
                                subQuestion.Answer = string.Empty;
                        }
                        else
                        {
                            if (subQuestion == null)
                                CurrentQuestion.Answer = answer;
                            else
                                subQuestion.Answer = answer;
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

        private void CreateButtons()
        {
            var buttonGroup = Instantiate(UIContent.HorizontalLayoutGroup, questionContent.transform) as GameObject;
            if (buttonGroup == null) return;

            // Previous Button
            var prevButton = ((GameObject)Instantiate(UIContent.ButtonPrefab, buttonGroup.transform)).GetComponent<Button>();
            prevButton.GetComponentInChildren<TMP_Text>().text = "Previous";
            prevButton.onClick.AddListener(() =>
            {
                ShowQuestion(--_questionIndex);
            });

            if (_questionIndex == 0)
                prevButton.interactable = false;

            // Next Button
            var nextButton = ((GameObject)Instantiate(UIContent.ButtonPrefab, buttonGroup.transform)).GetComponent<Button>();
            nextButton.GetComponentInChildren<TMP_Text>().text = "Next";
            nextButton.onClick.AddListener(() =>
            {
                if (CurrentQuestion.Mandatory && CurrentQuestion.Answer == null && 
                    CurrentQuestion.SubQuestions.TrueForAll(subQuestion => subQuestion.Answer == null)) return;
                ShowQuestion(++_questionIndex);
            });
            if (_questionIndex == _questions.Count - 1)
            {
                nextButton.interactable = false;

                // Submit Button
                var submitButton = ((GameObject)Instantiate(UIContent.ButtonPrefab, questionContent.transform)).GetComponent<Button>();
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
            var closeButton = ((GameObject)Instantiate(UIContent.ButtonPrefab, questionContent.transform)).GetComponent<Button>();
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