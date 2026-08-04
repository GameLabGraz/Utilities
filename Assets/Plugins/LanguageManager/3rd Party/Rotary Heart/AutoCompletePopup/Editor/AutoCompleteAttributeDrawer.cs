using UnityEditor;
using UnityEngine;

namespace RotaryHeart.Lib.AutoComplete
{
    [CustomPropertyDrawer(typeof(AutoCompleteAttribute))]
    [CustomPropertyDrawer(typeof(AutoCompleteTextFieldAttribute))]
    [CustomPropertyDrawer(typeof(AutoCompleteDropDownAttribute))]
    public class AutoCompleteAttributeDrawer : PropertyDrawer
    {
        enum AttributeType
        {
            TextField,
            Dropdown
        }
        
        string[] m_entries;
        AttributeType m_attributeType;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_entries == null)
            {
                AutoCompleteAttribute attribute = System.Attribute.GetCustomAttribute(fieldInfo, typeof(AutoCompleteAttribute)) as AutoCompleteAttribute;
                m_entries = attribute.Entries;

                if (System.Attribute.GetCustomAttribute(fieldInfo, typeof(AutoCompleteTextFieldAttribute)) != null)
                {
                    m_attributeType = AttributeType.TextField;
                }
                else if (System.Attribute.GetCustomAttribute(fieldInfo, typeof(AutoCompleteDropDownAttribute)) != null)
                {
                    m_attributeType = AttributeType.Dropdown;
                }
            }

            switch (m_attributeType)
            {
                case AttributeType.TextField:
                    property.stringValue = AutoCompleteTextField.EditorGUI.AutoCompleteTextField(position, label, property.stringValue, GUI.skin.textField, m_entries, "Type something here");
                    break;
                case AttributeType.Dropdown:
                    AutoCompleteDropDown.EditorGUI.AutoCompleteDropDown(position, label, property.stringValue, m_entries, s =>
                    {
                        property.stringValue = s;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                    break;
            }
        }
    }
}