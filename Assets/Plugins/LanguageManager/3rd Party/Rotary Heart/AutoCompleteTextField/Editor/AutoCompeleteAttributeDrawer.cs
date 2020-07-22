using UnityEditor;
using UnityEngine;
using EditorGUI = RotaryHeart.Lib.AutoComplete.AutoCompleteTextField.EditorGUI;

namespace RotaryHeart.Lib.AutoComplete
{
    [CustomPropertyDrawer(typeof(AutoCompleteAttribute))]
    public class AutoCompeleteAttributeDrawer : PropertyDrawer
    {
        string[] entries;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (entries == null)
            {
                AutoCompleteAttribute attribute = System.Attribute.GetCustomAttribute(fieldInfo, typeof(AutoCompleteAttribute)) as AutoCompleteAttribute;
                entries = attribute.Entries;
            }

            property.stringValue = EditorGUI.AutoCompleteTextField(position, label, property.stringValue, GUI.skin.textField, entries, "Type something here");
        }
    }
}