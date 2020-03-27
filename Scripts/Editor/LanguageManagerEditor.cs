using UnityEngine;
using UnityEditor;

namespace GEAR.Localization.Editor
{
    [CustomEditor(typeof(LanguageManager))]
    public class LanguageManagerEditor : UnityEditor.Editor
    {
        private const string TexturePath = "images/logo";
        private Texture2D _logoTexture;
        private void Awake()
        {
            _logoTexture = Resources.Load<Texture2D>(TexturePath);
        }

        public override void OnInspectorGUI()
        {
            DrawLogoTexture();
            DrawDefaultInspector();
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