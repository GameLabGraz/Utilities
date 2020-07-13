using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using GEAR.Serialize;
using UnityEngine;
using UnityEngine.Events;
using TranslationDict = System.Collections.Generic.Dictionary<string, GEAR.Localization.Translation>;

namespace GEAR.Localization
{
    [Serializable] public class LanguageChangedEvent : UnityEvent<SystemLanguage> { }

    
    [ExecuteAlways]
    public class LanguageManager : MonoBehaviour
    {
        private const string XmlSchemaFile = "mlgSchema";
        private readonly XmlSchemaSet _xmlSchemaSet = new XmlSchemaSet();

        [SerializeField]
        [SerializeProperty("CurrentLanguage")]
        private SystemLanguage _currentLanguage = SystemLanguage.English;

        public SystemLanguage CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                _currentLanguage = value;
                OnLanguageChanged.Invoke(_currentLanguage);
            }
        }

        public void SetLanguage(string languageKey)
        {
            CurrentLanguage = Enum.TryParse(languageKey, out SystemLanguage language) 
                ? language : SystemLanguage.Unknown;
        }

        [SerializeField]
        private List<TextAsset> _mlgFiles = new List<TextAsset>();

        public static LanguageManager Instance { get; private set; }

        public LanguageChangedEvent OnLanguageChanged = new LanguageChangedEvent();

        public TranslationDict Translations { get; private set; } = new TranslationDict();

        private void Awake()
        {
            Debug.Log("AWAKE LANG MANAGER !");
            if(Application.isPlaying)
                DontDestroyOnLoad(this);

            if (Instance == null)
            {
                Instance = this;
                Instance.CurrentLanguage = Application.systemLanguage;
                Instance._xmlSchemaSet.Add("", XmlReader.Create(
                        new MemoryStream(Resources.Load<TextAsset>(XmlSchemaFile).bytes)));
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            ClearTranslations();

            foreach (var mlgFile in _mlgFiles)
            {
                Instance.LoadMlgFile(mlgFile);
            }
        }

        private void Start()
        {
            if (!Application.isEditor) return;
            Debug.Log("Editor Start");
            Awake();
        }

        private void Update()
        {
            if (!Application.isEditor) return;
            Debug.Log("Editor Update");
            Awake();
        }

        public void ClearTranslations()
        {
            Instance.Translations.Clear();
        }

        public string GetString(string key)
        {
            return GetString(key, CurrentLanguage);
        }

        public string GetString(string key, SystemLanguage language)
        {
            return Translations.ContainsKey(key) ? Translations[key].GetValue(language) : key;
        }

        public bool LoadMlgFile(TextAsset mlgFile)
        {
            Translations = LoadMlgFile(mlgFile, _xmlSchemaSet, out var error);
            return !error;
        }

        // public bool SaveMlgFile(string path)
        // {
        //     SaveMlgFile(path, _translations, out var error);
        //     return !error;
        // }

        public static TranslationDict LoadMlgFile(TextAsset mlgFile, XmlSchemaSet xmlSchemaSet, out bool error)
        {
            var loadingError = false;
            var translations = new TranslationDict();

            if(mlgFile)
            {
                try
                {
                    var doc = XDocument.Load(new MemoryStream(mlgFile.bytes));
                    doc.Validate(xmlSchemaSet, (o, e) =>
                    {
                        Debug.Log(e.Message);
                        loadingError = true;
                    });

                    foreach (var translationElement in doc.Descendants("Translation"))
                    {
                        var key = translationElement.Attribute("Key")?.Value;
                        if (key == null)
                            continue;

                        var translation = new Translation(key);
                        foreach (var languageElement in translationElement.Elements())
                        {
                            if (!Enum.TryParse(languageElement.Name.ToString(), out SystemLanguage language))
                            {
                                Debug.Log($"LanguageManager::LoadMlgFile: Unsupported Language {languageElement.Name}");
                                loadingError = true;
                                continue;
                            }

                            translation.AddTranslation(language, Regex.Unescape(languageElement.Value));
                        }

                        translations[key] = translation;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    loadingError = true;
                }
            }
            else
            {
                Debug.LogError($"LanguageManager::LoadMlgFile: Unable to load language file");
                loadingError = true;
            }

            error = loadingError;
            return translations;
        }

        public static void SaveMlgFile(string path, TranslationDict translations, List<SystemLanguage> writingLanguages,
            out bool error)
        {
            var saveError = false;

            if (path.EndsWith(".xml"))
            {
                var writer = XmlWriter.Create(path, 
                    new XmlWriterSettings { NewLineOnAttributes = true, Indent = true });
                
                writer.WriteStartDocument();
                writer.WriteStartElement("LanguageManager");
                writer.WriteStartElement("Translations");

                foreach (var translation in translations)
                {
                    writer.WriteStartElement("Translation");
                    writer.WriteAttributeString("Key", translation.Key);

                    if (writingLanguages.Any())
                    {
                        foreach (var language in translation.Value.Values.Where(
                            language => writingLanguages.Contains(language.Key)))
                        {
                            writer.WriteElementString(language.Key.ToString(), language.Value);
                        }
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
            else
            {
                Debug.Log($"LanguageManager::SaveMlgFile: No xml file.");
                saveError = true;
            }
            error = saveError;
        }
    }
}