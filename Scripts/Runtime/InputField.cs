using TMPro;

namespace GameLabGraz.UI
{
    public class InputField : TMP_InputField
    {
        public void SetText(float value)
        {
            text = $"{value:0.###}";
        }
    }
}
