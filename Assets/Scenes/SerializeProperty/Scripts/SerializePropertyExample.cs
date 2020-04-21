using System.Collections.Generic;
using UnityEngine;
using GEAR.Serialize;

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

    [SerializeField]
    [SerializeProperty("ObjectArray")]
    private GameObject[] objectArray;

    [SerializeField]
    [SerializeProperty("ObjectList")]
    private List<GameObject> objectList;

    [SerializeField]
    [Range(0, 5, order = 0)]
    [SerializeProperty("IntArray", order = 1)]
    private int[] intArray;

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

    public GameObject[] ObjectArray
    {
        get => objectArray;
        set => objectArray = value;
    }

    public List<GameObject> ObjectList
    {
        get => objectList;
        set => objectList = value;
    }

    public int[] IntArray
    {
        get => intArray;
        set => intArray = value;
    }
}
