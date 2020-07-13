using UnityEditor;
using RotaryHeart.Lib.AutoComplete;
using UnityEngine;

[CustomEditor(typeof(DummyComponent))]
public class DummyComponentEditor : Editor
{
    SerializedProperty editorField1;
    SerializedProperty editorField2;
    SerializedProperty editorField3;

    string[] options = new string[] { "Option1", "Option 2/Option 2.1", "Option 2/Option 2.2", "Option 2/Option 2.2/Option 2.2.1", "Option2", "Option3", "Option4" };

    void OnEnable()
    {
        editorField1 = serializedObject.FindProperty("dummyEditorField1");
        editorField2 = serializedObject.FindProperty("dummyEditorField2");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        editorField1.stringValue = AutoCompleteTextField.EditorGUILayout.AutoCompleteTextField("Field 1", editorField1.stringValue, options, "Type something");
        editorField2.stringValue = AutoCompleteTextField.EditorGUILayout.AutoCompleteTextField(editorField2.stringValue, options, "Type something here too", GUILayout.Width(100));

        serializedObject.ApplyModifiedProperties();
    }
}
