using System;

namespace GameLabGraz.LimeSurvey.Data
{
    [Serializable]
    public class SubQuestion : BaseQuestion
    {
        public int ID { get; set; }

        public string SubquestionName { set; get; } = "";
    }
}
