using System;
using UnityEngine;


[Serializable]
public class MyTestClass {
    public string testString;
    public float testFloat;
    
    MyTestClass(string str, float f)
    {
        testString = str;
        testFloat = f;
    }
}

public class ReferenceValueScript2 : MonoBehaviour
{
    [SerializeField] private int intValue1 = 1;
    [SerializeField] private int intValue2 = 2;

    [SerializeField] private float floatValue1 = 1.1f;
    [SerializeField] private float floatValue2 = 2.2f;
    [SerializeField] private float floatValue3 = 3.3f;
    [SerializeField] private float floatValue4 = 4.4f;

    [SerializeField] public MyTestClass testVariable;
}
