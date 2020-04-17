using System.Collections.Generic;
using GEAR.QuestManager.NodeGraph;
using GEAR.QuestManager.NodeGraph.Extensions;
using TMPro;
using UnityEngine;

namespace GEAR.QuestManager.Reader
{
    [ExecuteInEditMode]
    public class QMNodeGraphReader : QMReader
    {
        public QMNodeGraph NodeGraph { get; set; }

        private Vector3 MainQuestSize => MainQuestPrefab.GetComponent<MeshRenderer> ().bounds.size;
        private Vector3 SubQuestSize => SubQuestPrefab.GetComponent<MeshRenderer> ().bounds.size;

        public override void ReadData ()
        {
            var mainQuestPosition = MainQuestIniOffset + MainQuestBodyOffset;

            if (autoPositionQuests)
            {
                mainQuestPosition = new Vector3 (
                    MainQuestIniOffset.x - 0.2f * MainQuestSize.x,
                    MainQuestIniOffset.y - 0.5f * CoverSize.y - 0.7f * MainQuestSize.y,
                    MainQuestIniOffset.z - 0.05f * MainQuestSize.z);
            }

            foreach (var node in NodeGraph.nodes.GetNodeByName (NodeType.QuestBody.GetStringValue ()))
            {
                foreach (QMNodeMainQuest mainQuestNode in NodeGraph.GetConnectedNodes (node, NodeType.MainQuest))
                {
                    var mainQuest = Instantiate (MainQuestPrefab, Root.transform);
                    mainQuest.transform.position = mainQuestPosition;

                    AddTextComponent (mainQuest, mainQuestNode.questName);

                    var subQuestOffsetCount = 0;
                    var nextOffset = Vector3.zero;
                    foreach (QMNodeSubQuest subQuestNode in NodeGraph.GetConnectedNodes (mainQuestNode, NodeType.SubQuest))
                    {
                        var subQuest = Instantiate (SubQuestPrefab, mainQuest.transform);
                        subQuest.transform.localScale = Vector3.one;
                        subQuest.transform.localRotation = Quaternion.Euler (Vector3.zero);

                        var subQuestPosition = subQuestOffsetCount * SubQuestBodyOffset;
                        if (autoPositionQuests)
                        {
                            subQuestPosition = new Vector3 (
                                mainQuestPosition.x - 0.2f * MainQuestSize.x,
                                mainQuestPosition.y - 0.5f * MainQuestSize.y - 0.7f * SubQuestSize.y - 1.2f * SubQuestSize.y * subQuestOffsetCount,
                                mainQuestPosition.z - 0.05f * MainQuestSize.z);
                        }

                        subQuest.transform.position = subQuestPosition;

                        nextOffset = SubQuestBodyOffset * subQuestOffsetCount;
                        if (autoPositionQuests) { nextOffset = new Vector3 (MainQuestIniOffset.x - 0.2f * MainQuestSize.x, subQuestPosition.y - 1.2f * MainQuestSize.y, MainQuestIniOffset.z - 0.05f * MainQuestSize.z); }

                        AddTextComponent (subQuest, subQuestNode.questName);

                        subQuestOffsetCount++;
                    }

                    //mainQuestPosition += MainQuestBodyOffset + SubQuestBodyOffset * subQuestOffsetCount;
                    mainQuestPosition = nextOffset;
                }
            }

        }

        private void AddTextComponent (GameObject quest, string text)
        {
            var textObj = new GameObject ("Text");
            textObj.transform.parent = quest.transform;
            textObj.transform.localPosition = Vector3.zero;
            textObj.transform.forward = quest.transform.right;

            var textMeshComp = textObj.AddComponent<TextMeshPro> ();
            textMeshComp.text = text;
            textMeshComp.alignment = TextAlignmentOptions.Left;
            textMeshComp.enableAutoSizing = true;
            textMeshComp.fontSizeMin = 0.0f;

            var rt = textMeshComp.GetComponent<RectTransform> ();
            rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, quest.GetComponent<MeshRenderer> ().bounds.size.z * 0.8f);
            rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, quest.GetComponent<MeshRenderer> ().bounds.size.y * 0.7f);
        }
    }
}