using GEAR.QuestManager.NodeGraph;
using GEAR.QuestManager.NodeGraph.Extensions;
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
            foreach (var node in NodeGraph.nodes.GetNodeByName (NodeType.QuestBody.GetStringValue ()))
            {
                foreach (var mainQuestNode in NodeGraph.GetConnectedNodes (node, NodeType.MainQuest))
                {
                    var mainQuest = Instantiate (MainQuestPrefab, Root.transform);
                    mainQuest.transform.position = mainQuestPosition;

                    var subQuestOffsetCount = 0;
                    foreach (var subQuestNode in NodeGraph.GetConnectedNodes (mainQuestNode, NodeType.SubQuest))
                    {
                        var subQuest = Instantiate (SubQuestPrefab, mainQuest.transform);
                        subQuest.transform.position = subQuestOffsetCount * SubQuestBodyOffset;
                        subQuest.transform.localScale = Vector3.one;
                        subQuest.transform.localRotation = Quaternion.Euler (Vector3.zero);

                        subQuestOffsetCount++;
                    }

                    mainQuestPosition += MainQuestBodyOffset + SubQuestBodyOffset * subQuestOffsetCount;

                }
            }

        }
    }
}