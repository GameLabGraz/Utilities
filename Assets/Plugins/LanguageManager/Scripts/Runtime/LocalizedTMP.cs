using TMPro;
using UnityEngine;

namespace GEAR.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTMP : LocalizedTextBase
    {
        private TMP_Text _text;

        private void Start()
        {
            _text = GetComponent<TMP_Text>();
            base.Start();
        }

        public override void UpdateLocalizedText()
        {
            if (_text == null)
                return;
            _text.text = GetText();
        }
    }
}