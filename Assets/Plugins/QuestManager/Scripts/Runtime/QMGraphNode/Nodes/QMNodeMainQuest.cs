using UnityEngine;

namespace GEAR.QuestManager.NodeGraph
{
    [System.Serializable]
    public class QMNodeMainQuest : QMNode
    {
        [Input] public string questBody;
        [Space]
        [Output] public string mainQuest;

        public override object GetValue(XNode.NodePort port)
        {
            return port.fieldName == "mainQuest" ? questName : null;
        }
    }
}
