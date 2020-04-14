using GEAR.Serialize;
using UnityEngine;

public class SerializePropertyExample : MonoBehaviour
{
    [SerializeField]
    [SerializeProperty("IntProperty")]
    public int intProperty = 1;

    [SerializeField]
    [Range(1, 5, order = 0)]
    [SerializeProperty("IntPropertyRange", order = 1)]
    public int intPropertyRange = 1;

    [SerializeField]
    [SerializeProperty("FloatProperty")]
    private float floatProperty = 1.0f;

    [SerializeField]
    [Range(0, 5, order = 0)]
    [SerializeProperty("FloatPropertyRange", order = 1)]
    private float floatPropertyRange = 1.0f;

    public int IntProperty
    {
        get => intProperty;
        set => intProperty = value;
    }

    public int IntPropertyRange
    {
        get => intPropertyRange;
        set => intPropertyRange = value;
    }

    public float FloatProperty
    {
        get => floatProperty;
        set => floatProperty = value;
    }

    public float FloatPropertyRange
    {
        get => floatPropertyRange;
        set => floatPropertyRange = value;
    }
}
