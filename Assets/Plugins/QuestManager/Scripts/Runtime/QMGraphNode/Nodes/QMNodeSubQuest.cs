using UnityEngine;

namespace GEAR.QuestManager.NodeGraph
{
    [System.Serializable]
    public class QMNodeSubQuest : QMNode
    {
        [Input] public string mainQuest;

        public UnityEngine.Object scriptFile;
        public bool AdditionalInformation;
    }
}
