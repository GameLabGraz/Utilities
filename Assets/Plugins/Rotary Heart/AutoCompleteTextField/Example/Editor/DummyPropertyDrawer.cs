using UnityEngine;
using UnityEditor;
using RotaryHeart.Lib.AutoComplete;

[CustomPropertyDrawer(typeof(DummyPropertyAttribute))]
public class DummyPropertyDrawer : PropertyDrawer
{
    string[] options = new string[] { "Option1", "Option 1/Option 1.1", "Option 1/Option 1.2", "Option 1/Option 1.1/Option 1.1.1", "Option2", "Option3", "Option4" };

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DummyPropertyAttribute attribute = System.Attribute.GetCustomAttribute(fieldInfo, typeof(DummyPropertyAttribute)) as DummyPropertyAttribute;
        property.stringValue = AutoCompleteTextField.EditorGUI.AutoCompleteTextField(position, attribute.DrawName ? label : GUIContent.none, property.stringValue, GUI.skin.textField, options, "Type something here");
    }
}
