using System.Collections.Generic;
using UnityEngine;

namespace GameLabGraz.QuestManager
{
    public class QuestData
    {
        public Quest QuestBody { get; set; }
        public string DefaultText { get; set; }
        public string TranslationKey { get; set; }
        public List<QuestData> SubQuestData = new List<QuestData>();
        public string Script { get; set; }

        public string AdditionalInformation { get; set; }
        public string AdditionalInfoTranslationKey { get; set; }
        public string AdditionalImagePath { get; set; }
        public int AdditionalImageWidth { get; set; }
        public int AdditionalImageHeight { get; set; }
        

        public GameObject QuestHint { get; set; }
        public GameObject QuestAchievement { get; set; }
        
        public bool IsCompleted { get; set; }
    }
}