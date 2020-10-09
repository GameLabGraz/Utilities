using UnityEngine;
using UnityEngine.Events;

namespace GEAR.VRInteraction
{
    [System.Serializable]
    public class ValueChangeEventFloat : UnityEvent<float>
    {
    }

    [System.Serializable]
    public class ValueChangeEventInt : UnityEvent<float>
    {
    }
    
    [System.Serializable]
    public class ValueChangeEventBool : UnityEvent<bool>
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
}
