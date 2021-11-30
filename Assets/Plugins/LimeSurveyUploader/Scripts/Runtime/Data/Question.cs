using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Data
{
    public enum QuestionType
    {
        Text,
        MultipleCoise,
        Unknown
    }

    public class SubQuestion
    {
        public string Title { get; set; }
        public string QuestionText { get; set; }
    }

    [Serializable]
    public class Question
    {
        [SerializeField] private int id;
        [SerializeField] private int sid;
        [SerializeField] private string title;
        [SerializeField] private string question;
        [SerializeField] private string type;
        [SerializeField] private string mandatory;

        public int ID => id;
        public int SID => sid;
        public string Title => title;
        public string QuestionText => question;
        public bool Mandatory => mandatory == "Y";
        public List<SubQuestion> SubQuestions { get; } = new List<SubQuestion>();
        public QuestionType QuestionType
        {
            get
            {
                switch (type)
                {
                    case "T":
                        return QuestionType.Text;
                    case "M":
                        return QuestionType.MultipleCoise;
                    default:
                        return QuestionType.Unknown;
                }
            }
        }
    }
}
