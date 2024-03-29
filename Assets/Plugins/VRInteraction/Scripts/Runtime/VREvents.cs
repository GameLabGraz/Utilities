﻿using UnityEngine;
using UnityEngine.Events;

namespace GameLabGraz.VRInteraction
{
    [System.Serializable]
    public class ValueChangeEventFloat : UnityEvent<float>
    {
    }

    [System.Serializable]
    public class ValueChangeEventInt : UnityEvent<int>
    {
    }
    
    [System.Serializable]
    public class ValueChangeEventBool : UnityEvent<bool>
    {
    }
    
    [System.Serializable]
    public class ValueChangeEventVector3 : UnityEvent<Vector3>
    {
    }

    [System.Serializable]
    public class SnapZoneEvent : UnityEvent<VRSnapDropZone>
    {
    }
    
    [System.Serializable]
    public class SnapUnsnapEvent : UnityEvent<VRSnapDropZone, GameObject>
    {
    }
    
    [System.Serializable]
    public class ValueChangeEventColor : UnityEvent<Color>
    {
    }
    
    [System.Serializable]
    public class BarcodeScanEvent : UnityEvent<Barcode>
    {
    }
}
