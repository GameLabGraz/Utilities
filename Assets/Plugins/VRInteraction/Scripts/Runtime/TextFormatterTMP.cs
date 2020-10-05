using TMPro;
using UnityEngine;

namespace GEAR.VRInteraction
{
    public class TextFormatterTMP : MonoBehaviour
    {
        [Tooltip("The string format in which the values should be shown. (e.g. '0.00')")]
        public string format = "F";
        [Tooltip("Any string that should be added after the formatted value.")]
        public string unit = "";
        [Tooltip("Defines if a space character should be added between formatted value and unit.")]
        public bool addSpaceBetweenUnit = false;

        public void FormatString(float number)
        {
            var text = number.ToString(format) + (addSpaceBetweenUnit ? " " : "") + unit;
            if (GetComponent<TMP_InputField>())
                GetComponent<TMP_InputField>().text = text;
            else if (GetComponent<TextMeshProUGUI>())
                GetComponent<TextMeshProUGUI>().text = text;
            else if (GetComponent<TextMeshPro>())
                GetComponent<TextMeshPro>().text = text;
        }
    }
}