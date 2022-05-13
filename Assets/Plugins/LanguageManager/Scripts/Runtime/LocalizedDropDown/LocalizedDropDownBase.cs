using System.Collections.Generic;
using UnityEngine;

namespace GEAR.Localization.DropDown
{
    public abstract class LocalizedDropDownBase : MonoBehaviour
    {
        protected List<string> _keys = new List<string>();

        protected void Start()
        {
            if(LanguageManager.Instance)
                LanguageManager.Instance.OnLanguageChanged.AddListener(language => UpdateLocalizedOptions());

            UpdateLocalizedOptions();
        }

        public abstract void UpdateLocalizedOptions();

        protected string GetOptionText(string key)
        {
            return !LanguageManager.Instance ? key : LanguageManager.Instance.GetString(key);
        }
    }
}
