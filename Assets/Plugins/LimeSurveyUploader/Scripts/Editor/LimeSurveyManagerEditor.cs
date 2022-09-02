using UnityEditor;
using UnityEngine;

namespace GameLabGraz.LimeSurvey.Editor
{
    [CustomEditor(typeof(LimeSurveyManager))]
    public class LimeSurveyManagerEditor : UnityEditor.Editor
    {
        private const string TexturePathDarkTheme = "images/LIME_logo_darkTheme";
        private const string TexturePathLightTheme = "images/LIME_logo_lightTheme";
        private const float logoHeight = 48;
        private Texture2D _logoTexture;
        private SerializedProperty surveyId;

        private void OnEnable()
        {
            _logoTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? TexturePathDarkTheme : TexturePathLightTheme);
            surveyId = serializedObject.FindProperty("surveyId");
        }

        public override void OnInspectorGUI()
        {
            DrawLogoTexture();

            serializedObject.Update();

            // Server Configuration
            EditorGUILayout.LabelField("Server Config", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Please enter the server configuration in the \"LimeSurveyServerConfig.json\" file");
            EditorGUILayout.Space();

            // Survey Configuration
            EditorGUILayout.LabelField("Survey Config", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(surveyId);
            EditorGUILayout.Space();
            
            serializedObject.ApplyModifiedProperties();
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