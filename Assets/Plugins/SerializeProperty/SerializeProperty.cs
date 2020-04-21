using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
#endif

namespace GEAR.Serialize
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class SerializeProperty : PropertyAttribute
    {
        public string PropertyName { get; }

        public SerializeProperty(string propertyName)
        {
            PropertyName = propertyName;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SerializeProperty))]
    public class SerializePropertyAttributeDrawer : PropertyDrawer
    {
        private SerializedProperty _property;
        private PropertyInfo _propertyFieldInfo;

        private UnityEngine.Object Target => _property.serializedObject.targetObject;

        private object Value => _propertyFieldInfo?.GetValue(Target, null);

        private int ArrayIndex => int.Parse(Regex.Match(_property.propertyPath, @".+\[(\d+)]").Groups[1].Value);

        private bool HasRangeAttribute => RangeAttribute != null;
        
        private RangeAttribute RangeAttribute
        {
            get
            {
                var member = Value is IList ? _property.propertyPath.Split('.')[0] : _property.name;
                
                var memberInfo = Target.GetType().GetMember(member,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                return memberInfo.Length == 0 ? null : memberInfo[0].GetCustomAttribute<RangeAttribute>();
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _property = property;

            // Find the property field using reflection, in order to get access to its getter/setter.
            if (_propertyFieldInfo == null)
                _propertyFieldInfo = Target.GetType().GetProperty(((SerializeProperty)attribute).PropertyName,
                                                     BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (_propertyFieldInfo != null)
            {
                EditorGUI.BeginChangeCheck();

                var value = _propertyFieldInfo?.GetValue(Target, null);
                switch (value)
                {
                    case null:
                    case IList array when array.Count == 0 || ArrayIndex >= array.Count:
                        return;
                    case IList array:
                    {
                        var type = array.GetType().IsGenericType
                            ? array.GetType().GetGenericArguments()[0]
                            : array.GetType().GetElementType();

                        array[ArrayIndex] = DrawProperty(position, property.propertyType, type, array[ArrayIndex], label);
                        break;
                    }
                    default:
                        value = DrawProperty(position, property.propertyType, _propertyFieldInfo.PropertyType, value, label);
                        break;
                }

                // If any changes were detected, call the property setter:
                if (EditorGUI.EndChangeCheck() && _propertyFieldInfo != null)
                {
                    // Record object state for undo:
                    Undo.RecordObject(Target, "Inspector");

                    // Call property setter:
                    _propertyFieldInfo.SetValue(Target, value, null);
                }
            }
            else
            {
                EditorGUI.LabelField(position, "Error: could not retrieve property.");
            }
        }

        private object DrawProperty(Rect position, SerializedPropertyType propertyType, Type type, object value, GUIContent label)
        {
            switch (propertyType)
            {
                case SerializedPropertyType.Integer:
                    return HasRangeAttribute
                        ? EditorGUI.IntSlider(position, label, (int)value, (int)RangeAttribute.min, (int)RangeAttribute.max)
                        : EditorGUI.IntField(position, label, (int)value);

                case SerializedPropertyType.Boolean:
                    return EditorGUI.Toggle(position, label, (bool)value);

                case SerializedPropertyType.Float:
                    return HasRangeAttribute
                        ? EditorGUI.Slider(position, label, (float)value, RangeAttribute.min, RangeAttribute.max)
                        : EditorGUI.FloatField(position, label, (float)value);

                case SerializedPropertyType.String:
                    return EditorGUI.TextField(position, label, (string)value);

                case SerializedPropertyType.Color:
                    return EditorGUI.ColorField(position, label, (Color)value);

                case SerializedPropertyType.ObjectReference:
                    return EditorGUI.ObjectField(position, label, (UnityEngine.Object)value, type, true);
                
                case SerializedPropertyType.ExposedReference:
                    return EditorGUI.ObjectField(position, label, (UnityEngine.Object)value, type, true);
                
                case SerializedPropertyType.LayerMask:
                    return EditorGUI.LayerField(position, label, (int)value);
                
                case SerializedPropertyType.Enum:
                    return EditorGUI.EnumPopup(position, label, (Enum)value);
                
                case SerializedPropertyType.Vector2:
                    return EditorGUI.Vector2Field(position, label, (Vector2)value);
                
                case SerializedPropertyType.Vector3:
                    return EditorGUI.Vector3Field(position, label, (Vector3)value);
                
                case SerializedPropertyType.Vector4:
                    return EditorGUI.Vector4Field(position, label, (Vector4)value);
                
                case SerializedPropertyType.Rect:
                    return EditorGUI.RectField(position, label, (Rect)value);
               
                case SerializedPropertyType.AnimationCurve:
                    return EditorGUI.CurveField(position, label, (AnimationCurve)value);
                
                case SerializedPropertyType.Bounds:
                    return EditorGUI.BoundsField(position, label, (Bounds)value);
                
                default:
                    throw new NotImplementedException("Unimplemented propertyType " + propertyType + ".");
            }
        }
    }
#endif
}