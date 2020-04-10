using System;
using System.Collections.Generic;
using System.Linq;
using GEAR.QuestManager.NodeGraph.Extensions;
using UnityEngine;

namespace GEAR.QuestManager.NodeGraph
{
    [Serializable, CreateAssetMenu (fileName = "QMNodeGraph", menuName = "QMNodeGraph/Quest Manager Graph")]
    public class QMNodeGraph : XNode.NodeGraph
    {
        public Texture2D Logo;
        private void OnGUI ()
        {
            GUI.Box (new Rect (0, 0, 100, 100), "This Box");
        }

        public List<QMNodeMainQuest> GetMainQuestNodes ()
        {
            var mainQuestNodes = new List<QMNodeMainQuest>();

            foreach (var node in nodes.GetNodeByName ("QM Node Body"))
            {
                foreach (var output in node.Outputs)
                {
                    mainQuestNodes.AddRange(
                        output.GetConnections().GetNodePortByName("QM Node Main Quest")
                            .Select(connection => (QMNodeMainQuest) connection.node));
                }
            }

            return mainQuestNodes;
        }
    }
}