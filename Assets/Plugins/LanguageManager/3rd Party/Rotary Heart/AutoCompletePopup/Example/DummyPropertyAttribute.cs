using UnityEngine;

public class DummyPropertyAttribute : PropertyAttribute
{
    public bool DrawName { get; }
    public bool DropDown { get; }
    public bool AllowCustom { get; }

    public DummyPropertyAttribute(bool drawName, bool dropDown, bool allowCustom)
    {
        DrawName = drawName;
        DropDown = dropDown;
        AllowCustom = allowCustom;
    }
}
