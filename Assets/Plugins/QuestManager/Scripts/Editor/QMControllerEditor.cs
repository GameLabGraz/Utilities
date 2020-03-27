using UnityEditor;
using UnityEngine;

namespace GEAR.QuestManager.Editor
{
    [CustomEditor (typeof (QMController))]
    public class QMControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI ()
        {
            DrawDefaultInspector ();
            if (GUILayout.Button ("Generate Quest Manager"))
            {
                ((QMController)target).GenerateQM ();
            }
        }
    }
}