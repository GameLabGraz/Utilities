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

        private List<Question> _questions;

        private int _questionIndex;
        private Question CurrentQuestion => _questions[_questionIndex];

        private void Start()
        {
            _questions =  LimeSurveyManager.Instance.GetQuestionList();
            ShowQuestion(0);
        }
        
        public void Next()
        {
            ShowQuestion(++_questionIndex);
        }

        public void Previous()
        {
            ShowQuestion(--_questionIndex);
        }

        public void Submit()
        {
            // ToDo: Upload to LimeSurvey Server
        }

        private void ShowQuestion(int questionIndex)
        {
            if (questionIndex >= _questions.Count)
                return;

            ClearQuestionContent();

            questionText.text = CurrentQuestion.QuestionText;
            switch (CurrentQuestion.QuestionType)
            {
                case QuestionType.Text:
                    CreateFreeText();
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
            var inputFieldObj = Instantiate(UIContent.InputPrefab, questionContent.transform) as GameObject;
            inputFieldObj.GetComponent<InputField>().lineType = TMP_InputField.LineType.MultiLineNewline;
            inputFieldObj.GetComponent<LayoutElement>().minHeight = 300;
        }

        private void CreateMultipleChoice()
        {
            foreach (var subQuestion in CurrentQuestion.SubQuestions)
            {
                var toggleObj = Instantiate(UIContent.TogglePrefab, questionContent.transform) as GameObject;
                if (toggleObj != null) toggleObj.GetComponentInChildren<TMP_Text>().text = subQuestion.QuestionText;
            }
        }

        private void CreatePointMatrix(int optionSize)
        {
            var subQuestionGroup = Instantiate(UIContent.VerticalLayoutGroup, questionContent.transform) as GameObject;
            if (subQuestionGroup == null) return;

            foreach (var subQuestion in CurrentQuestion.SubQuestions)
            {
                var subQuestionObj = Instantiate(UIContent.TextPrefab, subQuestionGroup.transform) as GameObject;
                if (subQuestionObj != null) subQuestionObj.GetComponent<TMP_Text>().text = subQuestion.QuestionText;

                CreatePointOptions(optionSize, subQuestionGroup.transform);
            }
        }

        private void CreatePointOptions(int optionSize, Transform optionContent = null)
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
                option.isOn = true;
                option.GetComponentInChildren<TMP_Text>().text = "NA";
                options.Add(option);
            }
            SetupRadioButtonOnValueChange(options);
        }

        private static void SetupRadioButtonOnValueChange(List<Toggle> options)
        {
            foreach (var option in options)
            {
                option.onValueChanged.AddListener(value =>
                {
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
            prevButton.onClick.AddListener(Previous);
            prevButton.GetComponentInChildren<TMP_Text>().text = "Previous";
            if (_questionIndex == 0)
                prevButton.interactable = false;

            // Next Button
            var nextButton = ((GameObject)Instantiate(UIContent.ButtonPrefab, buttonGroup.transform)).GetComponent<Button>();
            nextButton.onClick.AddListener(Next);
            nextButton.GetComponentInChildren<TMP_Text>().text = "Next";
            if (_questionIndex == _questions.Count - 1)
            {
                nextButton.interactable = false;

                // Submit Button
                var submitButton = ((GameObject)Instantiate(UIContent.ButtonPrefab, questionContent.transform)).GetComponent<Button>();
                submitButton.GetComponentInChildren<TMP_Text>().text = "Submit";
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