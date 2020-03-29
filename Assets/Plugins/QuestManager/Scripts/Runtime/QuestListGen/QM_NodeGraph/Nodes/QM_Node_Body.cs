using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gear.QuestManager
{
    [System.Serializable]
    public class QM_Node_Body : XNode.Node
    {
        [Output] public string questBody = "Quest Body";
        [Space]
        [TextArea]
        public string QuestBodyName;

        public override object GetValue(XNode.NodePort port)
        {
            if (QuestBodyName != null)
            {
                questBody = QuestBodyName;
            }
            return QuestBodyName;
        }
    }
}
