using System;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Data
{
    [Serializable]
    public class AnswerOption
    {
        [SerializeField] private string answer;
        [SerializeField] private int assessment_value;
        [SerializeField] private int scale_id;
        [SerializeField] private int order;

        public string AnswerText => answer;
        public int AssessmentValue => assessment_value;
        public int ScaleId => scale_id;
        public int Order => order;
    }
}
