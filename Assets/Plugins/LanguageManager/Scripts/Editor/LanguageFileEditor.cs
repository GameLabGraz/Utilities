using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;

namespace GEAR.Localization.Editor
{
    public class LanguageFileEditor : EditorWindow
    {
        [MenuItem("Window/Language Editor")]
        public static void ShowWindow()
        {
            GetWindow<LanguageFileEditor>("Language Editor");
        }

        Vector2 scrollPositionContent = Vector2.zero;
        Vector2 scrollPositionLanguage = Vector2.zero;
        private Translation addingTranslation = null;
        private string newLanguage = "";


        private bool languageFoldout = true;
        private string currentPath = "";
        private readonly HashSet<SystemLanguage> removedLanguages = new HashSet<SystemLanguage>();
        private readonly HashSet<SystemLanguage> addedLanguages = new HashSet<SystemLanguage>();
        private HashSet<SystemLanguage> supportedLanguages = new HashSet<SystemLanguage>();
        private readonly Dictionary<string, Translation> translations = new Dictionary<string, Translation>();
        private bool reloaded = true;
        
        // Start is called before the first frame update
        void OnGUI()
        {
            var style = EditorStyles.miniButton;
            style.fixedWidth = 55;
            style.alignment = TextAnchor.MiddleCenter;
            
            GUILayout.Label("Language Editor", EditorStyles.largeLabel);
            GUILayout.Label("You can edit your MLG files within this editor. If you make any changes, " +
                            "please do not forget to save them before selecting another file.", EditorStyles.helpBox);
            
            var path = "";
            var obj = Selection.activeObject;
            path = obj == null ? "Assets" : AssetDatabase.GetAssetPath(obj.GetInstanceID());
            if (path.Length <= 0)
            {
                EditorGUILayout.TextField("File", "Invalid Selection");
                return;
            }
            
            EditorGUILayout.TextField("File", path);
            var textAsset = Selection.activeObject as TextAsset;
            if (Directory.Exists(path) || !textAsset) {
                GUILayout.Label("Please select a MLG file to edit the content.", EditorStyles.helpBox);
                return;
            }

            if (reloaded)
            {
                addingTranslation = null;
                currentPath = "";
                translations.Clear();
                supportedLanguages.Clear();
                addedLanguages.Clear();
                removedLanguages.Clear();
                textAsset = Selection.activeObject as TextAsset;
        
                var xmlSchemaSet = new XmlSchemaSet();
                xmlSchemaSet.Add("", XmlReader.Create(new MemoryStream(Resources.Load<TextAsset>("mlgSchema").bytes)));
                if (!LanguageManager.LoadMlgFile(textAsset, xmlSchemaSet, translations, out var lastError,
                    out supportedLanguages, false))
                {
                    GUILayout.Label("Error: " + lastError, EditorStyles.helpBox);
                    return;
                }

                reloaded = false;
            }

            foreach (var removedLang in removedLanguages)
            {
                supportedLanguages.Remove(removedLang);
            }
            foreach (var addedLang in addedLanguages)
            {
                supportedLanguages.Add(addedLang);
            }
            
            if (GUILayout.Button("Save Changes"))
            {
                currentPath = path;
                SaveChanges();
            }
            if (GUILayout.Button("Reload File"))
            {
                reloaded = true;
                Repaint();
                return;
            }
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            /*
             * Language Specific
             */
            languageFoldout = EditorGUILayout.Foldout(languageFoldout, new GUIContent("Languages"));
            if (languageFoldout)
            {
                foreach (var language in supportedLanguages)
                {
                    EditorGUILayout.BeginHorizontal("HelpBox");
                    GUILayout.Label(language.ToString());
                    if (GUILayout.Button("Remove", EditorStyles.miniButton))
                    {
                        RemoveLanguage(language);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.BeginHorizontal("HelpBox");
                newLanguage = EditorGUILayout.TextField("New Language", newLanguage);
                if (GUILayout.Button("Add", EditorStyles.miniButton))
                {
                    AddLanguage(supportedLanguages);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            /*
             * Content Specific
             */
            scrollPositionContent = EditorGUILayout.BeginScrollView(scrollPositionContent);
            EditorGUILayout.BeginVertical();
            foreach (var t in translations)
            {               
                EditorGUILayout.BeginVertical("HelpBox");
                var languageKeyword = t.Key;
                var translation = t.Value;

                EditorGUILayout.BeginHorizontal();
                GUILayout.TextField(languageKeyword, EditorStyles.largeLabel);

                if (GUILayout.Button("Remove", style))
                {
                    translations.Remove(languageKeyword);
                    Repaint();
                }
                EditorGUILayout.EndHorizontal();
                
                foreach (var lang in supportedLanguages)
                {  
                    if(!translation.Values.ContainsKey(lang))
                        translation.Values.Add(lang, "");
                    translation.Values[lang] = EditorGUILayout.TextField(lang.ToString(), translation.Values[lang]);
                }
                EditorGUILayout.EndVertical();
            }
            
            if(addingTranslation == null)
            {
                if (GUILayout.Button("Add Element"))
                {
                    addingTranslation = new Translation("");
                    Repaint();
                }
            }
            else
            {
                EditorGUILayout.BeginVertical("HelpBox");
                addingTranslation.Key = EditorGUILayout.TextField("Key", addingTranslation.Key);
                foreach (var lang in supportedLanguages)
                {  
                    if(!addingTranslation.Values.ContainsKey(lang))
                        addingTranslation.Values.Add(lang, "");
                    addingTranslation.Values[lang] = EditorGUILayout.TextField(lang.ToString(), addingTranslation.Values[lang]);
                }

                if (GUILayout.Button("Add"))
                {
                    if(translations.ContainsKey(addingTranslation.Key))
                        ShowNotification(new GUIContent("Key already used"));
                    else if (addingTranslation.Key == "")
                        ShowNotification(new GUIContent("Empty key"));
                    else 
                    {
                        translations.Add(addingTranslation.Key, addingTranslation);
                        addingTranslation = null;
                        Repaint();
                    }
                }
                if (GUILayout.Button("Remove"))
                {
                    addingTranslation = null;
                    Repaint();
                }
                
                EditorGUILayout.EndVertical();
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void OnSelectionChange()
        {
            reloaded = true;
            Repaint();
        }

        private void RemoveLanguage(SystemLanguage language)
        {
            if(addedLanguages.Contains(language)) addedLanguages.Remove(language);
            removedLanguages.Add(language);
            Repaint();
        }
        
        private void AddLanguage(ICollection<SystemLanguage> supportedLanguages)
        {
            if (SystemLanguage.TryParse<SystemLanguage>(newLanguage, true, out var parsedLanguage))
            {
                addedLanguages.Add(parsedLanguage);
                if (removedLanguages.Contains(parsedLanguage))
                    removedLanguages.Remove(parsedLanguage);
                    
                if(supportedLanguages.Contains(parsedLanguage))
                    ShowNotification(new GUIContent("Language is already added!"));
                Repaint();
            }
            else
            {
                ShowNotification(new GUIContent("Cannot add unsupported Language!"));
            }
        }

        private void SaveChanges()
        {
            if (!LanguageManager.SaveMlgFile(currentPath, translations, supportedLanguages.ToList(), out var lastError))
            {
                ShowNotification(new GUIContent("Could not save file:\n" + lastError));
            }
            else
                ShowNotification(new GUIContent("Saved\n" + currentPath));

            reloaded = true;
            addingTranslation = null;
            AssetDatabase.Refresh();
            UndoChanges();
        }

        private void UndoChanges()
        {
            newLanguage = "";
            addedLanguages.Clear();
            removedLanguages.Clear();
            addingTranslation = null;
            Repaint();
        }
    }
}