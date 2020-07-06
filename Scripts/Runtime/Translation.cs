using System;
using System.Collections.Generic;
using UnityEngine;

namespace GEAR.Localization
{
    [Serializable]
    public class Translation
    {
        private string _key;
        private readonly Dictionary<SystemLanguage, string> _values = new Dictionary<SystemLanguage, string>();

        public Dictionary<SystemLanguage, string> Values => _values;
        public string Key
        {
            get => _key;
            set => _key = value;
        }

        public Translation(string key)
        {
            _key = key;
        }

        public void AddTranslation(SystemLanguage language, string value)
        {
            if(_values.ContainsKey(language))
                Debug.LogWarning($"Translation: contains {language} already");

            _values[language] = value;
        }

        public string GetValue(SystemLanguage language)
        {
            if (_values.ContainsKey(language))
                return _values[language];
            if (_values.ContainsKey(SystemLanguage.English))
                return _values[SystemLanguage.English];
            return _key;
        }

        public string GetValueIfAvailable(SystemLanguage language)
        {
            return _values.ContainsKey(language) ? _values[language] : "";
        }
    }
}
