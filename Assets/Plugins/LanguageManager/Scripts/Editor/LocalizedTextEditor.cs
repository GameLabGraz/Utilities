using UnityEditor;
using UnityEngine;

namespace GEAR.Localization
{
    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextEditor : UnityEditor.Editor
    {
        private const string TexturePath = "images/logoLanguageManager";
        private Texture2D logo = null;
        
        SerializedProperty keyProperty;
        SerializedProperty suffixProperty;

        public void OnEnable()
        {
            logo = Resources.Load(TexturePath, typeof(Texture2D)) as Texture2D;
            keyProperty = serializedObject.FindProperty("key");
            suffixProperty = serializedObject.FindProperty("suffix");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Label(logo, GUILayout.Height(50), GUILayout.MinHeight(50), GUILayout.ExpandHeight(false));
            
            EditorGUILayout.PropertyField(keyProperty);
            EditorGUILayout.PropertyField(suffixProperty);
        }
    }
}