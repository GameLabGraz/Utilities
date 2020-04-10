using GEAR.QuestManager.NodeGraph;
using UnityEngine;

namespace GEAR.QuestManager.Reader
{
    [ExecuteInEditMode]
    public class QMNodeGraphReader : QMReader
    {
        public QMNodeGraph NodeGraph { get; set; }

        public override void ReadData ()
        {
            var mainQuestPosition = MainQuestIniOffset + MainQuestBodyOffset;
            foreach (var mainQuestNode in NodeGraph.GetMainQuestNodes())
            {
                var subQuestOffsetCount = 0;

                var mainQuest = Instantiate(MainQuestPrefab, Root.transform);
                mainQuest.transform.position = mainQuestPosition;

                // SubQuest code
                // subQuestOffsetCount++

                mainQuestPosition += MainQuestBodyOffset + SubQuestBodyOffset * subQuestOffsetCount;

            }
            
        }
    }
}