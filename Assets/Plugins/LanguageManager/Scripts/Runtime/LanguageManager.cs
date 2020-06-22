using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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

        private string _lastError = "";
        public string LastError => _lastError;

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
            // return LoadMlgFile(mlgFile, _xmlSchemaSet, _translations, out _lastError, true);
            
            var result = true;
            if (!mlgFile)
            {
                _lastError = $"LanguageManager::LoadMlgFile: Unable to load language file";
                Debug.LogError(_lastError);
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
                            _lastError = $"LanguageManager::LoadMlgFile: Unsupported Language {languageElement.Name}";
                            Debug.Log(_lastError);
                            result = false;
                            continue;
                        }

                        translation.AddTranslation(language, Regex.Unescape(languageElement.Value));
                    }

                    _translations[key] = translation;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                _lastError = e.Message;
                return false;
            }

            return result;
        }
        
        public static bool LoadMlgFile(TextAsset mlgFile, XmlSchemaSet xmlSchemaSet, Dictionary<string, Translation> translations, out string lastError, out HashSet<SystemLanguage> supportedLanguages, bool outputDebug)
        {
            lastError = "";
            supportedLanguages = new HashSet<SystemLanguage>();
            var result = true;
            if (!mlgFile)
            {
                lastError = $"LanguageManager::LoadMlgFile: Unable to load language file";
                if(outputDebug)
                    Debug.LogError(lastError);
                return false;
            }
            
            try
            {
                var doc = XDocument.Load(new MemoryStream(mlgFile.bytes));
                var tmpError = "";
                doc.Validate(xmlSchemaSet, (o, e) =>
                {
                    tmpError = e.Message;
                    if(outputDebug)
                        Debug.Log(e.Message);
                    result = false;
                });
                lastError = tmpError;

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
                            lastError = $"LanguageManager::LoadMlgFile: Unsupported Language {languageElement.Name}";
                            if(outputDebug)
                                Debug.Log(lastError);
                            result = false;
                            continue;
                        }

                        supportedLanguages.Add(language);
                        translation.AddTranslation(language, Regex.Unescape(languageElement.Value));
                    }

                    translations[key] = translation;
                }
            }
            catch (Exception e)
            {
                if(outputDebug)
                    Debug.Log(e.Message);
                lastError = e.Message;
                return false;
            }

            return result;
        }

        public static bool SaveMlgFile(string path, Dictionary<string, Translation> translations, List<SystemLanguage> writingLanguages,
            out string lastError)
        {
            lastError = "";
            try
            {
                if (!path.EndsWith(".xml"))
                {
                    lastError = "No xml file.";
                    return false;
                }

                using (var file = new StreamWriter(path, false))
                {
                    file.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    file.WriteLine("<LanguageManager>");
                    file.WriteLine("  <Translations>");

                    foreach (var translation in translations)
                    {
                        file.WriteLine("    <Translation Key=\"" + translation.Key + "\">");

                        foreach (var value in translation.Value.Values)
                        {
                            if (writingLanguages.Any() && !writingLanguages.Contains(value.Key)) 
                                continue;
                            
                            var val = new StringBuilder();
                            if (value.Value.Any(c => c >= 255 || c == '&'))
                            {
                                foreach (var c in value.Value)
                                {
                                    if (c >= 255 || c == '&')
                                        val.Append($@"\u{(ushort) c:x4}");
                                    else
                                        val.Append(c);
                                }
                            }
                            else
                                val.Append(value.Value);

                            file.WriteLine("      <" + value.Key + ">" + val + "</" + value.Key + ">");
                        }

                        file.WriteLine("    </Translation>");
                    }

                    file.WriteLine("  </Translations>");
                    file.WriteLine("</LanguageManager>");
                }
                return true;
            } 
            catch (Exception e)
            {
                lastError = e.Message;
                Console.WriteLine(e);
                return false;
            }
        }
    }
}