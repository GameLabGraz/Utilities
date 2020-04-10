using UnityEditor;
using UnityEngine;

namespace GEAR.QuestManager.Editor
{
    [CustomEditor (typeof (QMController))]
    public class QMControllerEditor : UnityEditor.Editor
    {
        private const string TexturePath = "images/logo";
        private Texture2D _logoTexture;

        private void Awake()
        {
            _logoTexture = Resources.Load<Texture2D>(TexturePath);
        }

        public override void OnInspectorGUI ()
        {
            DrawLogoTexture();
            DrawDefaultInspector ();

            if (GUILayout.Button ("Generate Quest Manager"))
            {
                ((QMController)target).GenerateQuestManager ();
            }
        }

        private void DrawLogoTexture()
        {
            if (_logoTexture)
            {
                GUILayout.Box(_logoTexture, GUILayout.Width(90f), GUILayout.Height(60f), GUILayout.ExpandWidth(true));
            }
        }
    }
}