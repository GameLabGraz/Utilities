using UnityEngine;

namespace GEAR.Localization.Text
{
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class LocalizedText : LocalizedTextBase
    {
        private UnityEngine.UI.Text _text;
        
        protected new void Start()
        {
            _text = GetComponent<UnityEngine.UI.Text>();
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