using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Data
{
    [Serializable]
    public class QuestionGroup
    {
        [SerializeField] private int id;
        [SerializeField] private int sid;
        [SerializeField] private int gid;
        [SerializeField] private int group_order;
        [SerializeField] private string group_name;

        public int ID => id;
        public int SID => sid;
        public int GID => gid;
        public int GroupOrder => group_order;
        public string GroupName => group_name;

        public List<Question> Questions { get; } = new List<Question>();
    }
}
