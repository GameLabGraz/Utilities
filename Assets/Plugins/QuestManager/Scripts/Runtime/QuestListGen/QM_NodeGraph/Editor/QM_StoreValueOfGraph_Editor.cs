using UnityEngine;
using UnityEditor;

namespace Gear.QuestManager
{
    [CustomEditor(typeof(QM_StoreValueOfGraph))]
    public class QM_StoreValueOfGraph_Editor : Editor
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

