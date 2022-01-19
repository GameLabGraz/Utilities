using System;
using System.Collections.Generic;
using GEAR.Gadgets.ReferenceValue;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using GEAR.Gadgets.ReferenceValue.Editor;
[CustomPropertyDrawer(typeof(MyClassReferenceValue))]
public class MyClassReferenceValuePropertyDrawer : ReferenceValuePropertyDrawer<MyTestClass> { }
#endif

[Serializable] public class MyClassReferenceValue : ReferenceValue<MyTestClass> { }

public class ReferenceValueExample : MonoBehaviour
{
    [SerializeField] private IntegerReferenceValue intReferenceValue;
    [SerializeField] private List<FloatReferenceValue> floatReferenceValues;
    [SerializeField] private MyClassReferenceValue referenceTestViaGameObject;
    [SerializeField] private MyClassReferenceValue referenceTestViaComponent;
    
    private void Start()
    {
        intReferenceValue.Value = 1;

        foreach (var floatReferenceValue in floatReferenceValues)
        {
            floatReferenceValue.Value = 2.3f;
        }

        var value1 = referenceTestViaGameObject.Value;
        Debug.Assert(value1 != null && value1.testString.CompareTo("Sandra's Test") == 0 
                                    && Math.Abs(value1.testFloat - 20.1) < 0.0001,
            "Getting value of reference via GameObject is not working.");
        var value2 = referenceTestViaComponent.Value;
        Debug.Assert(value2 != null && value2.testString.CompareTo("Sandra's Test") == 0 
                                    && Math.Abs(value2.testFloat - 20.1) < 0.0001,
            "Getting value of reference via Component is not working.");

    }
}
