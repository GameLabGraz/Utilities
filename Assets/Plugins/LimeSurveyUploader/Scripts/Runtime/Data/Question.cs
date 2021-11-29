using System;

namespace GameLabGraz.LimeSurvey.Data
{
    [Serializable]
    public class Question
    {
        public int id;
        public int sid;
        public string title;
        public string question;
        public string type;
        public string mandatory;

        public bool IsMandatory => mandatory == "Y";
    }
}
