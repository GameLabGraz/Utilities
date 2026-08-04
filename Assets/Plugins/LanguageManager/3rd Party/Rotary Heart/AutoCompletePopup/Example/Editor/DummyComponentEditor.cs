using UnityEditor;
using RotaryHeart.Lib.AutoComplete;
using UnityEngine;

[CustomEditor(typeof(DummyComponent))]
public class DummyComponentEditor : Editor
{
    SerializedProperty editorTextFieldSimple;
    SerializedProperty editorTextFieldNoLabel;
    SerializedProperty editorTextFieldAllowCustoms;
    
    SerializedProperty editorDropDownSimple;
    SerializedProperty editorDropDownNoLabel;
    SerializedProperty editorDropDownAllowCustoms;

    string[] options = new string[] { "Option1", "Option 2/Option 2.1", "Option 2/Option 2.2", "Option 2/Option 2.2/Option 2.2.1", "Option2", "Option3", "Option4" };

    void OnEnable()
    {
        editorTextFieldSimple = serializedObject.FindProperty(nameof(DummyComponent.editorTextFieldSimple));
        editorTextFieldNoLabel = serializedObject.FindProperty(nameof(DummyComponent.editorTextFieldNoLabel));
        editorTextFieldAllowCustoms = serializedObject.FindProperty(nameof(DummyComponent.editorTextFieldAllowCustoms));
        
        editorDropDownSimple = serializedObject.FindProperty(nameof(DummyComponent.editorDropDownSimple));
        editorDropDownNoLabel = serializedObject.FindProperty(nameof(DummyComponent.editorDropDownNoLabel));
        editorDropDownAllowCustoms = serializedObject.FindProperty(nameof(DummyComponent.editorDropDownAllowCustoms));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        editorTextFieldSimple.stringValue = AutoCompleteTextField.EditorGUILayout.AutoCompleteTextField("Field 1", editorTextFieldSimple.stringValue, options, "Type something");
        editorTextFieldNoLabel.stringValue = AutoCompleteTextField.EditorGUILayout.AutoCompleteTextField(editorTextFieldNoLabel.stringValue, options, "Type something here too", false, true, options: GUILayout.Width(200));
        editorTextFieldAllowCustoms.stringValue = AutoCompleteTextField.EditorGUILayout.AutoCompleteTextField("Allow Custom", editorTextFieldAllowCustoms.stringValue, options, "Type something", true);
        
        AutoCompleteDropDown.EditorGUILayout.AutoCompleteDropDown("Field 1", editorDropDownSimple.stringValue, options, s =>
        {
            editorDropDownSimple.stringValue = s;
            serializedObject.ApplyModifiedProperties();
        });
        AutoCompleteDropDown.EditorGUILayout.AutoCompleteDropDown(editorDropDownNoLabel.stringValue, options, s =>
        {
            editorDropDownNoLabel.stringValue = s;
            serializedObject.ApplyModifiedProperties();
        }, false, true, options: GUILayout.Width(200));
        AutoCompleteDropDown.EditorGUILayout.AutoCompleteDropDown("Allow Custom", editorDropDownAllowCustoms.stringValue, options, s =>
        {
            editorDropDownAllowCustoms.stringValue = s;
            serializedObject.ApplyModifiedProperties();
        }, true);

        serializedObject.ApplyModifiedProperties();
    }
}