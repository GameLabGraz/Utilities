using UnityEngine;
using UnityEditor;

namespace GEAR.Localization.Editor
{
    [CustomEditor(typeof(LanguageManager))]
    public class LanguageManagerEditor : UnityEditor.Editor
    {
        private const string TexturePathDarkTheme = "images/logo_darkTheme";
        private const string TexturePathLightTheme = "images/logo_lightTheme";
        private const float logoHeight = 40;
        private Texture2D _logoTexture;
        private void Awake()
        {
            _logoTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin? TexturePathDarkTheme : TexturePathLightTheme);
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
                GUILayout.Label(_logoTexture, GUILayout.Height(logoHeight), GUILayout.MinHeight(logoHeight), GUILayout.MaxHeight(logoHeight), GUILayout.ExpandHeight(false));
            }
        }
    }
}