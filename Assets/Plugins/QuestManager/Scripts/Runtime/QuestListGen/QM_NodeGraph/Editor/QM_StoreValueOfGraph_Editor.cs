using UnityEngine;
using UnityEditor;

namespace GEAR.QuestManager.NodeGraph.Editor
{
    [CustomEditor(typeof(QM_StoreValueOfGraph))]
    public class QM_StoreValueOfGraph_Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate Quests List"))
            {
                ((QM_StoreValueOfGraph)target).GenerateQuestsList();
            }
        }
    }
}

