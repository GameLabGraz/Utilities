using System.Collections.Generic;
using GEAR.Gadgets.ReferenceValue;
using UnityEngine;
using UnityEngine.Events;

public class ReferenceValueExample : MonoBehaviour
{
    [SerializeField] private IntegerReferenceValue intReferenceValue;
    [SerializeField] private List<FloatReferenceValue> floatReferenceValues;
    
    public UnityEvent onExample = new UnityEvent();

    private void Start()
    {
        intReferenceValue.Value = 1;

        foreach (var floatReferenceValue in floatReferenceValues)
        {
            floatReferenceValue.Value = 2.3f;
        }
    }
}
