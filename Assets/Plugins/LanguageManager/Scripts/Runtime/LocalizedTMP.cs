using TMPro;
using UnityEngine;

namespace GEAR.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTMP : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The key that should be used to get the text from the language file.")]
        private string key;

        [SerializeField]
        [Tooltip("The suffix will be inserted after the text that will be loaded from the language file.")]
        private string suffix = string.Empty;

        private TMP_Text _text;

        public string Key
        {
            get => key;
            set
            {
                key = value;
                UpdateLocalizedText();
            }
        }

        private void Start()
        {
            _text = GetComponent<TMP_Text>();

            if (LanguageManager.Instance)
                LanguageManager.Instance.OnLanguageChanged.AddListener(language => UpdateLocalizedText());

            UpdateLocalizedText();
        }

        public void UpdateLocalizedText()
        {
            if (_text == null)
                return;
            if (!LanguageManager.Instance)
                _text.text = Key;
            else
                _text.text = LanguageManager.Instance.GetString(Key) + suffix;
        }
    }
}