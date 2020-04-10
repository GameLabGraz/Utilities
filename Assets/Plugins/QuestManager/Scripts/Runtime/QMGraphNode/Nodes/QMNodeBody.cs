using UnityEngine;

namespace GEAR.QuestManager.NodeGraph
{
    [System.Serializable]
    public class QMNodeBody : XNode.Node
    {
        [Output] public string questBody = "Quest Body";
        [Space]
        [TextArea]
        public string questBodyName;

        public override object GetValue(XNode.NodePort port)
        {
            return port.fieldName == "questBody" ? questBodyName : null;
        }
    }
}
