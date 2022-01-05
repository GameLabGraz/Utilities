using System;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Data
{
    [Serializable]
    public class SubQuestion
    {
        [SerializeField] private string title;
        [SerializeField] private string question;
        [SerializeField] private int scale_id;

        public string Title => title;
        public string QuestionText => question;
        public int ScaleId => scale_id;
        public object Answer { get; set; }
    }
}
