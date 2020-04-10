using UnityEngine;

namespace GEAR.QuestManager.NodeGraph
{
    [System.Serializable]
    public abstract class QMNode : XNode.Node
    {
        [SerializeField] protected int questNumber;

        [TextArea] public string questName;
    }
}