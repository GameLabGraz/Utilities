using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFormatter : MonoBehaviour
{
    public string objName = "Switch";

    private TextMeshPro _tmp;
    protected void Start()
    {
        _tmp = GetComponent<TextMeshPro>();
    }

    public void OnBoolean(bool newValue)
    {
        if (_tmp)
        {
            _tmp.text = objName + " is " + (newValue ? "on" : "off");
        }
    }

    public void OnFloat(float newValue)
    {
        if (_tmp)
        {
            _tmp.text = objName + "'s value = " + newValue;
        }
    }
}
