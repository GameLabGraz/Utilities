using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Data
{
    public enum QuestionType
    {
        Text,
        MultipleChoice,
        FivePointChoice,
        FivePointMatrix,
        TenPointMatrix,
        Unknown
    }

    public class SubQuestion
    {
        public string Title { get; set; }
        public string QuestionText { get; set; }
        public object Answer { get; set; }
    }

    [Serializable]
    public class Question
    {
        [SerializeField] private int id;
        [SerializeField] private int sid;
        [SerializeField] private int gid;
        [SerializeField] private string title;
        [SerializeField] private string question;
        [SerializeField] private string type;
        [SerializeField] private string mandatory;

        public int ID => id;
        public int SID => sid;
        public int GID => gid;
        public string Title => title;
        public string QuestionText => question;
        public bool Mandatory => mandatory == "Y";
        public List<SubQuestion> SubQuestions { get; } = new List<SubQuestion>();
        public object Answer { get; set; }

        public QuestionType QuestionType
        {
            get
            {
                switch (type)
                {
                    case "T":
                        return QuestionType.Text;
                    case "M":
                        return QuestionType.MultipleChoice;
                    case "5":
                        return QuestionType.FivePointChoice;
                    case "A":
                        return QuestionType.FivePointMatrix;
                    case "B":
                        return QuestionType.TenPointMatrix;
                    default:
                        return QuestionType.Unknown;
                }
            }
        }
    }
}
