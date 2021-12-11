using UnityEngine;

namespace GameLabGraz.UI
{
    public static class UIContent
    {
        // Layout Groups
        public static Object VerticalLayoutGroup => Resources.Load("LayoutGroup/VerticalLayoutGroup");
        public static Object HorizontalLayoutGroup => Resources.Load("LayoutGroup/HorizontalLayoutGroup");

        // UI Elements
        public static Object TextPrefab => Resources.Load("Text");
        public static Object InputPrefab => Resources.Load("InputField");
        public static Object SliderPrefab => Resources.Load("SliderGroup");
        public static Object TogglePrefab => Resources.Load("ToggleGroup");
        public static Object RadioButtonPrefab => Resources.Load("RadioButtonGroup");
        public static Object ButtonPrefab => Resources.Load("Button");
    }
}