using RotaryHeart.Lib.AutoComplete;
using UnityEngine;

public class DummyComponentAttribute : MonoBehaviour
{
    [DummyProperty(true)]
    public string dummyEditorField3;
#if UNITY_EDITOR
    [AutoComplete(new string[] { "Entry 1", "Entry 2", "Entry 2/Entry 2.1", "Entry 2/Entry 2.2", "Entry 2/Entry 2.3", "Entry 3", "Entry 3/Entry3.1" })]
#endif
    public string textField;
    [DummyProperty(false)]
    public string dummyEditorField2;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
