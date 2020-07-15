using UnityEngine;

public class DummyPropertyAttribute : PropertyAttribute
{
    bool m_drawName;

    public bool DrawName
    {
        get { return m_drawName; }
    }

    public DummyPropertyAttribute(bool drawName)
    {
        m_drawName = drawName;
    }
}
