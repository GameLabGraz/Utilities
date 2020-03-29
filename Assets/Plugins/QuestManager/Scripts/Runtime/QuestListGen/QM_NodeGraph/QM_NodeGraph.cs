using UnityEngine;
using System;

namespace Gear.QuestManager
{
    [Serializable, CreateAssetMenu(fileName = "QM_NodeGraph", menuName = "QM_NodeGraph/Quest Manager Graph")]
    public class QM_NodeGraph : XNode.NodeGraph
    {
        public Texture2D Logo;
        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, 100, 100), "This Box");
        }
    }
}


