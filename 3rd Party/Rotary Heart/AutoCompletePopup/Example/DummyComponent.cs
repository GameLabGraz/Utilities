using RotaryHeart.Lib.AutoComplete;
using UnityEngine;

public class DummyComponent : MonoBehaviour
{
    public string guiTextField;
    public string guiDropdown;
    public string editorTextFieldSimple;
    public string editorTextFieldNoLabel;
    public string editorTextFieldAllowCustoms;
    public string editorDropDownSimple;
    public string editorDropDownNoLabel;
    public string editorDropDownAllowCustoms;

    string[] options = new string[] { "Option1", "Option 2/Option 2.1", "Option 2/Option 2.2", "Option 2/Option 2.2/Option 2.2.1", "Option2", "Option3", "Option4" };
    
    void OnGUI()
    {
        guiTextField = AutoCompleteTextField.GUI.AutoCompleteTextField(new Rect(15, 15, 200, 30), "Label", guiTextField, options,
            "This is the hint");
        
        AutoCompleteDropDown.GUI.AutoCompleteDropDown(new Rect(Screen.width - 215, 15, 200, 30), "Label", guiDropdown, options, s =>
        {
            guiDropdown = s;
        });
    }
}
