using UnityEngine;

namespace GEAR.Localization
{
    public abstract class LocalizedTextBase : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The key that should be used to get the text from the language file.")]
        protected string key;

        [SerializeField]
        [Tooltip("The suffix will be inserted after the text that will be loaded from the language file.")]
        protected string suffix = string.Empty;

        public string Key
        {
            get => key;
            set
            {
                key = value;
                UpdateLocalizedText();
            }
        }
        
        protected void Start()
        {
            if (LanguageManager.Instance)
                LanguageManager.Instance.OnLanguageChanged.AddListener(language => UpdateLocalizedText());

            UpdateLocalizedText();
        }

        public abstract void UpdateLocalizedText();

        protected string GetText()
        {
            if (!LanguageManager.Instance) return Key;
            return LanguageManager.Instance.GetString(Key) + suffix;
        }
    }
}