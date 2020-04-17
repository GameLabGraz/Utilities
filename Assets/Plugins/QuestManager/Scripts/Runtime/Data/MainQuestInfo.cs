using System;
using System.Collections.Generic;

namespace GEAR.QuestManager.Data
{
    [Serializable]
    public class MainQuestInfo : QuestInfo
    {
        private List<SubQuestInfo> _subQuestInfos = new List<SubQuestInfo>();

        public MainQuestInfo(int questNumber, string questName) : base(questNumber, questName)
        {
        }

        public void AddSubQuestInfo(SubQuestInfo subQuestInfo)
        {
            _subQuestInfos.Add(subQuestInfo);
        }

        public List<SubQuestInfo> GetSubQuestInfos()
        {
            return _subQuestInfos;
        }
    }
}