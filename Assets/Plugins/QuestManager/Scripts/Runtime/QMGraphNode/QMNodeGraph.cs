using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GEAR.QuestManager.Attribute;
using GEAR.QuestManager.Extensions;

namespace GEAR.QuestManager.NodeGraph
{
    public enum NodeType
    {
        [StringValue ("QM Node Body")]
        QuestBody,

        [StringValue ("QM Node Main Quest")]
        MainQuest,

        [StringValue ("QM Node Sub Quest")]
        SubQuest
    }

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
            var mainQuestNodes = new List<QMNodeMainQuest> ();

            foreach (var node in nodes.GetNodeByName ("QM Node Body"))
            {
                foreach (var output in node.Outputs)
                {
                    mainQuestNodes.AddRange (
                        output.GetConnections ().GetNodePortByName ("QM Node Main Quest")
                        .Select (connection => (QMNodeMainQuest) connection.node));
                }
            }

            return mainQuestNodes;
        }

        public List<XNode.Node> GetConnectedNodes (XNode.Node node, NodeType nodeType)
        {
            var connectedNodes = new List<XNode.Node> ();
            var strNodeType = nodeType.GetStringValue ();

            foreach (var output in node.Outputs)
            {
                connectedNodes.AddRange (
                    output.GetConnections ().GetNodePortByName (strNodeType)
                    .Select (connection => connection.node));
            }
            return connectedNodes;
        }

        // public void SubQuestGen (XNode.Node node)
        // {
        //     var subQuestList = new List<XNode.Node> ();

        //     foreach (var outPut in node.Outputs)
        //     {
        //         foreach (var con in outPut.GetConnections ())
        //         {
        //             subQuestList.Add (con.node);

        //             // For Test
        //             var subQuestData = con.node as QMNodeSubQuest;
        //             Debug.Log (subQuestData.questName);
        //         }
        //     }

        //     QuestDictionary[node] = subQuestList;
        // }
    }
}