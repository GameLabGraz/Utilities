using GEAR.Localization.DropDown;
using UnityEditor;
using UnityEngine;

namespace GEAR.Localization.Editor
{
    [CustomEditor(typeof(LocalizedDropDownBase), true)]
    public class LocalizedDropDownEditor : UnityEditor.Editor
    {
        private const string TexturePathDarkTheme = "images/LM_logo_darkTheme";
        private const string TexturePathLightTheme = "images/LM_logo_lightTheme";
        private const float logoHeight = 48;
        private Texture2D _logoTexture;
        private void Awake()
        {
            _logoTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? TexturePathDarkTheme : TexturePathLightTheme);
        }

        public override void OnInspectorGUI()
        {
            DrawLogoTexture();

            GUILayout.Label("Note: The options entered in the DropDown component are used as keys in the translation process.");

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
