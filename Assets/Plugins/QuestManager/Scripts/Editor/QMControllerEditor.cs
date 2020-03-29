using System.IO;
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

            LogoTexture();

            if (GUILayout.Button ("Generate Quest Manager"))
            {
                ((QMController)target).GenerateQM ();
            }
        }

        private static void LogoTexture()
        {
            string texturePath = "gearLogo";

            
            var tex = Resources.Load<Texture2D>(texturePath);
            if (tex)
            {
                Debug.Log("Logo Texture is found");

                GUILayout.Box(tex,GUILayout.Width(90f), GUILayout.Height(60f), GUILayout.ExpandWidth(true));
            }

            else
            {
                Debug.Log("No texture is found");
            }

        }
    }
}