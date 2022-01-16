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

        private SerializedProperty url;
        private SerializedProperty userName;
        private SerializedProperty password;
        private SerializedProperty surveyId;

        private void OnEnable()
        {
            _logoTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? TexturePathDarkTheme : TexturePathLightTheme);

            url = serializedObject.FindProperty("url");
            userName = serializedObject.FindProperty("userName");
            password = serializedObject.FindProperty("password");
            surveyId = serializedObject.FindProperty("surveyId");
        }

        public override void OnInspectorGUI()
        {
            DrawLogoTexture();

            serializedObject.Update();

            EditorGUILayout.PropertyField(url);
            EditorGUILayout.PropertyField(userName);
            password.stringValue = EditorGUILayout.PasswordField(password.displayName, password.stringValue);
            EditorGUILayout.PropertyField(surveyId);
            
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