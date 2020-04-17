using System;

namespace GEAR.QuestManager.Data
{
    [Serializable]
    public class SubQuestInfo : QuestInfo
    {
        public Type Script { get; }
        public bool AdditionalInfo { get; }

        public SubQuestInfo(int questNumber, string questName, Type script, bool additionalInfo = false) : base(questNumber, questName)
        {
            Script = script;
            AdditionalInfo = additionalInfo;
        }
    }
}
