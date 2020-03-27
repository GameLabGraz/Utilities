using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using GEAR.Serialize;
using UnityEngine;
using UnityEngine.Events;

namespace GEAR.Localization
{
    [Serializable] public class LanguageChangedEvent : UnityEvent<SystemLanguage> { }

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

        private readonly Dictionary<string, Translation> _translations = new Dictionary<string, Translation>();

        private void Awake()
        {
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

        public void ClearTranslations()
        {
            Instance._translations.Clear();
        }

        public string GetString(string key)
        {
            return GetString(key, CurrentLanguage);
        }

        public string GetString(string key, SystemLanguage language)
        {
            return _translations.ContainsKey(key) ? _translations[key].GetValue(language) : key;
        }

        public bool LoadMlgFile(TextAsset mlgFile)
        {
            var result = true;
            if (!mlgFile)
            {
                Debug.LogError($"LanguageManager::LoadMlgFile: Unable to load language file");
                return false;
            }

            try
            {
                var doc = XDocument.Load(new MemoryStream(mlgFile.bytes));
                doc.Validate(_xmlSchemaSet, (o, e) =>
                {
                    Debug.Log(e.Message);
                    result = false;
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
                            result = false;
                            continue;
                        }

                        translation.AddTranslation(language, languageElement.Value);
                    }

                    _translations[key] = translation;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }

            return result;
        }
    }
}