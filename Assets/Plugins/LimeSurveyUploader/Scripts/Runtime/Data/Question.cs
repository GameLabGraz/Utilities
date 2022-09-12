using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Data
{
    public enum QuestionType
    {
        Text,
        ShortText,
        ListDropdown,
        ListRadio,
        MultipleChoice,
        FivePointChoice,
        FivePointMatrix,
        TenPointMatrix,
        Matrix,
        IntNumber,
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
        public bool RandomOrder { get; set; } = false;
        public List<SubQuestion> SubQuestions { get; set; } = new List<SubQuestion>();
        public List<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();

        public string GetTypeString()
        {
            return type;
        }
        
        public QuestionType QuestionType
        {
            get
            {
                switch (type)
                {
                    case "T":
                        return QuestionType.Text;
                    case "S":
                        return QuestionType.ShortText;
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
                    case "N":
                        return QuestionType.IntNumber;
                    default:
                        return QuestionType.Unknown;
                }
            }
        }
        
        public virtual bool HasAnswer()
        {
            if (SubQuestions.Count == 0)
                return base.HasAnswer();

            if (QuestionType == QuestionType.MultipleChoice)
            {
                return SubQuestions.Any(sub => sub.HasAnswer());
            }

            return SubQuestions.TrueForAll(sub => sub.HasAnswer());
        }
    }
}
