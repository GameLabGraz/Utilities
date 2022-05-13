using System;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Data
{
    [Serializable]
    public class AnswerOption
    {
        [SerializeField] private string answer_code;
        [SerializeField] private string answer;
        [SerializeField] private int order;

        public AnswerOption(string answerCode, string answerText, int order)
        {
            this.answer_code = answerCode;
            this.answer = answerText;
            this.order = order;
        }

        public string AnswerCode => answer_code;
        public string AnswerText => answer;
        public int Order => order;
    }
}
