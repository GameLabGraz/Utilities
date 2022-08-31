using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Data
{
    public enum QuestionType
    {
        Text,
        ListDropdown,
        ListRadio,
        MultipleChoice,
        FivePointChoice,
        FivePointMatrix,
        TenPointMatrix,
        Matrix,
        Unknown
    }

    [Serializable]
    public class Question : BaseQuestion
    {
        [SerializeField] private int id;
        [SerializeField] private int sid;
        [SerializeField] private int gid;
        [SerializeField] private int parent_qid;
        [SerializeField] private int question_order;
        [SerializeField] private string type;
        [SerializeField] private string other;
        [SerializeField] private string mandatory;

        public int ID => id;
        public int SID => sid;
        public int GID => gid;
        public int ParentID => parent_qid;
        public int QuestionOrder => question_order;
        public bool Other => other == "Y";
        public bool Mandatory => mandatory == "Y";
        public List<SubQuestion> SubQuestions { get; } = new List<SubQuestion>();
        public List<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();

        public QuestionType QuestionType
        {
            get
            {
                switch (type)
                {
                    case "T":
                        return QuestionType.Text;
                    case "L":
                        return QuestionType.ListRadio;
                    case "!":
                        return QuestionType.ListDropdown;
                    case "M":
                        return QuestionType.MultipleChoice;
                    case "5":
                        return QuestionType.FivePointChoice;
                    case "A":
                        return QuestionType.FivePointMatrix;
                    case "B":
                        return QuestionType.TenPointMatrix;
                    case "F":
                        return QuestionType.Matrix;
                    default:
                        return QuestionType.Unknown;
                }
            }
        }
    }
}
