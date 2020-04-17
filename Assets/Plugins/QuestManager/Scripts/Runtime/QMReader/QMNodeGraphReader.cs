using System.Collections.Generic;
using UnityEngine;
using GEAR.QuestManager.Data;
using GEAR.QuestManager.NodeGraph;
using GEAR.QuestManager.Extensions;

namespace GEAR.QuestManager.Reader
{
    [ExecuteInEditMode]
    public class QMNodeGraphReader : QMReader
    {
        [SerializeField] private QMNodeGraph nodeGraph;

        public QMNodeGraph NodeGraph
        {
            get => nodeGraph;
            set => nodeGraph = value;
        }

        public override List<MainQuestInfo> ReadData()
        {
            var mainQuestInfos = new List<MainQuestInfo>();

            if (!nodeGraph)
            {
                Debug.LogError($"QMNodeGraphReader::ReadData: Unable to load node graph.");
                return new List<MainQuestInfo>();
            }

            foreach (var node in NodeGraph.nodes.GetNodeByName(NodeType.QuestBody.GetStringValue()))
            {
                foreach (QMNodeMainQuest mainQuestNode in NodeGraph.GetConnectedNodes(node, NodeType.MainQuest))
                {
                    var mainQuestInfo = new MainQuestInfo(
                        mainQuestNode.questNumber, 
                        mainQuestNode.questName);

                    foreach (QMNodeSubQuest subQuestNode in NodeGraph.GetConnectedNodes(mainQuestNode, NodeType.SubQuest))
                    {
                          mainQuestInfo.AddSubQuestInfo(new SubQuestInfo(
                            subQuestNode.questNumber,
                            subQuestNode.questName,
                            subQuestNode.scriptFile.GetType(),
                            subQuestNode.AdditionalInformation));
                    }

                    mainQuestInfos.Add(mainQuestInfo);
                }
            }

            return mainQuestInfos;
        }
    }
}