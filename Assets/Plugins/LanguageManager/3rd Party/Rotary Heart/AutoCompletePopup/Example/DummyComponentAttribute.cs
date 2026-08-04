using RotaryHeart.Lib.AutoComplete;
using TMPro;
using UnityEngine;

public class DummyComponentAttribute : MonoBehaviour
{
#if UNITY_EDITOR
    [AutoCompleteTextField(new string[] { "Entry 1", "Entry 2/Entry 2.1", "Entry 2/Entry 2.2", "Entry 2/Entry 2.3", "Entry 3/Entry3.1" })]
#endif
    public string editorTextFieldNoLabel;
    [DummyProperty(true, false, false)]
    public string dummyEditorTextFieldSimple;
    [DummyProperty(false, false, false)]
    public string dummyEditorTextFieldNoLabel;
    [DummyProperty(true, false, true)]
    public string dummyEditorTextFieldAllowCustoms;
    
    [Space]
#if UNITY_EDITOR
    [AutoCompleteDropDown(new string[] { "Entry 1", "Entry 2/Entry 2.1", "Entry 2/Entry 2.2", "Entry 2/Entry 2.3", "Entry 3/Entry3.1" })]
#endif
    public string editorDropDownNoLabel;
    [DummyProperty(true, true, false)]
    public string dummyEditorDropDownSimple;
    [DummyProperty(false, true, false)]
    public string dummyEditorDropDownNoLabel;
    [DummyProperty(true, true, true)]
    public string dummyEditorDropDownAllowCustoms;
    
    [Space]
    [SerializeField]
    TMP_InputField prefabTest;
    [SerializeField]
    AutoCompletePrefab uiComplete;

    public void ShowWindow()
    {
        uiComplete.AutoCompleteWindow(new string[] { "Entry 1", "Entry 2/Entry 2.1", "Entry 2/Entry 2.2", "Entry 2/Entry 2.3", "Entry 3/Entry3.1" }, null,
            s =>
            {
                prefabTest.text = s;
            }, separator:"/");
    }
}
