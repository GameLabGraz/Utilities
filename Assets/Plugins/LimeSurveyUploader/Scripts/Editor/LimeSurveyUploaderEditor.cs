using UnityEditor;
using UnityEngine;

namespace GEAR.LimeSurvey.Editor
{
    [CustomEditor(typeof(LimeSurveyUploader))]
    public class LimeSurveyUploaderEditor : UnityEditor.Editor
    {
        private const string TexturePath = "images/logo";
        private Texture2D _logoTexture;

        private SerializedProperty user;
        private SerializedProperty password;
        private SerializedProperty surveyId;

        private SerializedProperty insert;
        private SerializedProperty charset;

        private void OnEnable()
        {
            _logoTexture = Resources.Load<Texture2D>(TexturePath);

            user = serializedObject.FindProperty("user");
            password = serializedObject.FindProperty("password");
            surveyId = serializedObject.FindProperty("surveyId");

            insert = serializedObject.FindProperty("insert");
            charset = serializedObject.FindProperty("charset");
        }

        public override void OnInspectorGUI()
        {
            DrawLogoTexture();

            serializedObject.Update();

            EditorGUILayout.PropertyField(user);
            password.stringValue = EditorGUILayout.PasswordField(password.displayName, password.stringValue);
            EditorGUILayout.PropertyField(surveyId);

            EditorGUILayout.PropertyField(insert);
            EditorGUILayout.PropertyField(charset);

            serializedObject.ApplyModifiedProperties();
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