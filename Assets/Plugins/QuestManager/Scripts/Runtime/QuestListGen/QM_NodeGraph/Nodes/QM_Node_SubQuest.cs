using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Gear.QuestManager
{
    [System.Serializable]
    public class QM_Node_SubQuest : XNode.Node
    {
        public int QuestNumber;
        [Input] public string mainQuest;
        [Space]

        [TextArea]
        public string SubQuestName = "Sub Quest";
        
        public UnityEngine.Object scriptFile;
        public bool AdditionalInformation;
    }
}
