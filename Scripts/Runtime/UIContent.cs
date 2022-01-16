using UnityEngine;

namespace GameLabGraz.UI
{
    public static class UIContent
    {
        // Layout Groups
        public static Object VerticalLayoutGroup => Resources.Load("LayoutGroup/VerticalLayoutGroup");
        public static Object HorizontalLayoutGroup => Resources.Load("LayoutGroup/HorizontalLayoutGroup");

        // UI Elements
        public static Object Text => Resources.Load("Text");
        public static Object Input => Resources.Load("InputField");
        public static Object SliderPrefab => Resources.Load("SliderGroup");
        public static Object ToggleButton => Resources.Load("ToggleButton");
        public static Object ToggleGroup => Resources.Load("ToggleGroup");
        public static Object RadioButton => Resources.Load("RadioButton");
        public static Object RadioButtonGroup => Resources.Load("RadioButtonGroup");
        public static Object Button => Resources.Load("Button");
    }
}