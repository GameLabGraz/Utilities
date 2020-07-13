using System.Collections.Generic;
using RotaryHeart.Lib;
using RotaryHeart.Lib.AutoComplete;
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

        // private string[] options = new string[] { }; // { "Option1", "Option 2/Option 2.1", "Option 2/Option 2.2", "Option 2/Option 2.2/Option 2.2.1", "Option2", "Option3", "Option4" };
        private List<string> options = new List<string>(); // { "Option1", "Option 2/Option 2.1", "Option 2/Option 2.2", "Option 2/Option 2.2/Option 2.2.1", "Option2", "Option3", "Option4" };

        public void OnEnable()
        {
            Debug.Log("OnEnable Editor!!!!");
            logo = Resources.Load(TexturePath, typeof(Texture2D)) as Texture2D;
            keyProperty = serializedObject.FindProperty("key");
            suffixProperty = serializedObject.FindProperty("suffix");
            
            Debug.Log("Lang Instance: " + LanguageManager.Instance);
            if (!LanguageManager.Instance) return;
            foreach (var translationPair in LanguageManager.Instance.Translations)
            {
                options.Add(translationPair.Key);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Label(logo, GUILayout.Height(50), GUILayout.MinHeight(50), GUILayout.ExpandHeight(false));
            
            
            // EditorGUILayout.PropertyField(keyProperty);
            keyProperty.stringValue = AutoCompleteTextField.EditorGUILayout.AutoCompleteTextField("Key", keyProperty.stringValue, options.ToArray(), "");
            EditorGUILayout.PropertyField(suffixProperty);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}