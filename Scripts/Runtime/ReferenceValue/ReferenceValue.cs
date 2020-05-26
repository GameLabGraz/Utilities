using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GEAR.Gadgets.ReferenceValue
{
    [Serializable]
    public struct ObjectInfo
    {
        public Object Object;

        public string ComponentName;

        public string FieldName;
    }

    [Serializable]
    public abstract class ReferenceValue<T>
    {
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.Public |
                                                  System.Reflection.BindingFlags.NonPublic;

        public int index;

        public ObjectInfo objectInfo;

        public T Value
        {
            get
            {
                if (objectInfo.Object is GameObject gameObject && !string.IsNullOrEmpty(objectInfo.ComponentName))
                {
                    var component = gameObject.GetComponent(objectInfo.ComponentName);
                    var fieldInfo = component.GetType().GetField(objectInfo.FieldName, BindingFlags);
                    return (T) fieldInfo?.GetValue(component);
                }
                else
                {
                    var fieldInfo = objectInfo.Object.GetType().GetField(objectInfo.FieldName, BindingFlags);
                    return (T)fieldInfo?.GetValue(objectInfo.Object);
                }
            }
            set
            {
                if (objectInfo.Object is GameObject gameObject && !string.IsNullOrEmpty(objectInfo.ComponentName))
                {
                    var component = gameObject.GetComponent(objectInfo.ComponentName);
                    var fieldInfo = component.GetType().GetField(objectInfo.FieldName, BindingFlags);
                    fieldInfo?.SetValue(component, value);
                }
                else
                {
                    var fieldInfo = objectInfo.Object.GetType().GetField(objectInfo.FieldName, BindingFlags);
                    fieldInfo?.SetValue(objectInfo.Object, value);
                }
            }
        }
    }

    [Serializable] public class BooleanReferenceValue : ReferenceValue<bool> { }
    [Serializable] public class StringReferenceValue : ReferenceValue<string> { }
    [Serializable] public class IntegerReferenceValue : ReferenceValue<int> { }
    [Serializable] public class FloatReferenceValue : ReferenceValue<float> { }
    [Serializable] public class Vector2ReferenceValue : ReferenceValue<Vector2> { }
    [Serializable] public class Vector3ReferenceValue : ReferenceValue<Vector3> { }
}