using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gear.QuestManager
{
    [System.Serializable]
    public class QM_Node_MainQuest : XNode.Node
    {
        public int QuestNumber;
        [Input] public string questBody;
        [Space]
        [Output] public string mainQuest = "Main Quest";
        [Space]
        [TextArea]
        public string MainQuestName;

        public override object GetValue(XNode.NodePort port)
        {
            if (MainQuestName != null)
            {
                mainQuest = MainQuestName;
            }
            return MainQuestName;
        }
    }
}
