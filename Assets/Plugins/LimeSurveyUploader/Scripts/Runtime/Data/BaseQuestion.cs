using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Data
{
    [Serializable]
    public class BaseQuestion
    {
        [SerializeField] private string title;
        [SerializeField] private string question;

        public string Title => title;
        public string QuestionText => question;

        public string Answer { get; set; }
        
        public virtual bool HasAnswer()
        {
            return !string.IsNullOrEmpty(Answer);
        }
    }
}
