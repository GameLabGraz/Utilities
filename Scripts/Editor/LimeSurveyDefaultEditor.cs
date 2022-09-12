using UnityEditor;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Editor
{
    public class LimeSurveyDefaultEditor : UnityEditor.Editor
    {
        private const string TexturePathDarkTheme = "images/LIME_logo_darkTheme";
        private const string TexturePathLightTheme = "images/LIME_logo_lightTheme";
        private const float logoHeight = 48;
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
    
    [CustomEditor(typeof(LimeSurveyView))]
    public class LimeSurveyViewEditor : LimeSurveyDefaultEditor
    {
    }
    
    [CustomEditor(typeof(LimeSurveyEvents))]
    public class LimeSurveyEventsEditor : LimeSurveyDefaultEditor
    {
    }
}