using System.Collections.Generic;
using RotaryHeart.Lib.AutoComplete;
using UnityEditor;
using UnityEngine;

namespace GEAR.Localization
{
    [CustomEditor(typeof(LocalizedTextBase), true)]
    public class LocalizedTextEditor : UnityEditor.Editor
    {
        private const string TexturePathDarkTheme = "images/LM_logo_darkTheme";
        private const string TexturePathLightTheme = "images/LM_logo_lightTheme";
        private const float logoHeight = 48;
        private Texture2D logo = null;
        
        SerializedProperty keyProperty;
        SerializedProperty suffixProperty;

        private readonly List<string> _options = new List<string>();

        public void OnEnable()
        {
            logo = Resources.Load(EditorGUIUtility.isProSkin? TexturePathDarkTheme : TexturePathLightTheme, typeof(Texture2D)) as Texture2D;
            keyProperty = serializedObject.FindProperty("key");
            suffixProperty = serializedObject.FindProperty("suffix");
            
            if (!LanguageManager.Instance) return;
            foreach (var translationPair in LanguageManager.Instance.Translations)
            {
                _options.Add(translationPair.Key);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if(logo)
                GUILayout.Label(logo, GUILayout.Height(logoHeight), GUILayout.MinHeight(logoHeight), GUILayout.MaxHeight(logoHeight), GUILayout.ExpandHeight(false));

            var style = new GUIStyle(GUI.skin.textField);
            if (keyProperty.stringValue != "" && !_options.Contains(keyProperty.stringValue))
                style.normal.textColor = Color.red;
            keyProperty.stringValue = AutoCompleteTextField.EditorGUILayout.AutoCompleteTextField("Key", keyProperty.stringValue, style, _options.ToArray(), "");
            EditorGUILayout.PropertyField(suffixProperty);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}