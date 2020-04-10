using System.Collections.Generic;
using UnityEngine;

namespace GEAR.QuestManager.NodeGraph
{
    public class QM_StoreValueOfGraph : MonoBehaviour
    {
        [SerializeField] private QMNodeGraph QuestGraph;

        public List<XNode.Node> MainQuests;
        public Dictionary<XNode.Node, List<XNode.Node>> QuestDictionary = new Dictionary<XNode.Node, List<XNode.Node>>();

        public void GenerateQuestsList()
        {
            MainQuests.Clear();

            var Nodes = QuestGraph.nodes;

            foreach (var node in Nodes)
            {
                if (node.name.Contains("QM_Node_Body"))
                {
                    foreach (var thisPort in node.Outputs)
                    {
                        foreach (var con in thisPort.GetConnections())
                        {
                            if (con.node.name.Contains("QM_Node_Main Quest"))
                            {
                                var node_i = con.node;
                                MainQuests.Add(con.node);

                                SubQuestGen(con.node);
                            }
                        }

                    }
                }
            }
        }

        public void SubQuestGen(XNode.Node node)
        {
            var subQuestList = new List<XNode.Node>();

            foreach (var outPut in node.Outputs)
            {
                foreach (var con in outPut.GetConnections())
                {
                    subQuestList.Add(con.node);

                    // For Test
                    var subQuestData = con.node as QMNodeSubQuest;
                    Debug.Log(subQuestData.questName);
                }
            }

            QuestDictionary[node] = subQuestList;
        }
    }

}
