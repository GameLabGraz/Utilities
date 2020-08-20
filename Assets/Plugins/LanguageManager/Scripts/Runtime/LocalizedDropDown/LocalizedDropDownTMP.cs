using TMPro;
using UnityEngine;

namespace GEAR.Localization.DropDown
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LocalizedDropDownTMP : LocalizedDropDownBase
    {
        private TMP_Dropdown _dropDown;

        protected new void Start()
        {
            _dropDown = GetComponent<TMP_Dropdown>();
            foreach (var option in _dropDown.options)
                _keys.Add(option.text);

            base.Start();
        }

        public override void UpdateLocalizedOptions()
        {
            if (_dropDown == null) return;

            _dropDown.options.Clear();
            foreach (var key in _keys)
            {
                _dropDown.options.Add(new TMP_Dropdown.OptionData(
                    GetOptionText(key)));
            }
            _dropDown.RefreshShownValue();
        }
    }
}