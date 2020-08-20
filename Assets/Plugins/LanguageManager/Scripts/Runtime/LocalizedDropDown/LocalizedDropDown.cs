using UnityEngine;
using UnityEngine.UI;

namespace GEAR.Localization.DropDown
{
    [RequireComponent(typeof(Dropdown))]
    public class LocalizedDropDown : LocalizedDropDownBase
    {
        private Dropdown _dropDown;

        protected new void Start()
        {
            _dropDown = GetComponent<Dropdown>();
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
                _dropDown.options.Add(new Dropdown.OptionData(
                    GetOptionText(key)));
            }
            _dropDown.RefreshShownValue();
        }
    }
}
