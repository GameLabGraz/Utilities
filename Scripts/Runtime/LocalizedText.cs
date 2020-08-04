using UnityEngine;
using UnityEngine.UI;

namespace GEAR.Localization
{
    [RequireComponent(typeof(Text))]
    public class LocalizedText : LocalizedTextBase
    {
        private Text _text;
        
        protected new void Start()
        {
            _text = GetComponent<Text>();
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