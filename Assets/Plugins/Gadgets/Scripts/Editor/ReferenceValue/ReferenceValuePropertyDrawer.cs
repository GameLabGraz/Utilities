using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GEAR.Gadgets.ReferenceValue.Editor
{
    public class ReferenceValuePropertyDrawer<T> : PropertyDrawer
    {
        private const int LineHeight = 16;
        private const int IndentWidth = 190;
        private static int MarginBetweenFields => 2 * (int)EditorGUIUtility.standardVerticalSpacing;

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | 
                                                  System.Reflection.BindingFlags.Public | 
                                                  System.Reflection.BindingFlags.NonPublic;

        private Rect _position;
        private int _totalPropertyHeight;
        
        private SerializedProperty _property;
        private FieldInfo _propertyFieldInfo;
        
        private Object Target => _property.serializedObject.targetObject;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyName = property.propertyPath.Split('.');
            _property = property.serializedObject.FindProperty(propertyName[0]);

            if (_propertyFieldInfo == null)
                _propertyFieldInfo = Target.GetType().GetField(_property.propertyPath, BindingFlags);

            if (_propertyFieldInfo == null) return;

            EditorGUI.BeginChangeCheck();

            var referenceValue = GetReferenceValue(property);
            if (referenceValue == null) return;

            _position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            SetPosition();
            IncrementPosition();

            referenceValue.objectInfo.Object = EditorGUI.ObjectField(_position, "Object",
                referenceValue.objectInfo.Object, typeof(Object), true);

            var components = new List<Component>();
            var fieldNames = new List<string>();
            var fullNames = new List<string>();

            if (referenceValue.objectInfo.Object != null)
            {
                if (referenceValue.objectInfo.Object is GameObject gameObject)
                {
                    var allGameObjComponents = gameObject.GetComponents<Component>();

                    var duplicateList =
                        allGameObjComponents.GroupBy(x => x.GetType())
                            .Where(g => g.Count() > 1)
                            .Select(y => y.Key)
                            .ToList();
                    
                    foreach (var component in gameObject.GetComponents<Component>())
                    {
                        var currentComponentFields = component.GetType().GetFields(BindingFlags)
                            .Where(field => typeof(T).IsAssignableFrom(field.FieldType));
                        
                        foreach (var f in currentComponentFields)
                        {
                            if(duplicateList.Contains(component.GetType()))
                                fullNames.Add($"{component.GetType().FullName} ({component.GetInstanceID()})/{f.Name}");
                            else
                                fullNames.Add($"{component.GetType().FullName}/{f.Name}");
                            fieldNames.Add($"{f.Name}");
                            components.Add(component);
                        }
                    }
                }
                else
                {
                    var currentComponentFields = referenceValue.objectInfo.Object.GetType()
                        .GetFields(BindingFlags)
                        .Where(field => typeof(T).IsAssignableFrom(field.FieldType));
                
                    fieldNames.AddRange(currentComponentFields.Select(f => f.Name));
                    fullNames.AddRange(currentComponentFields.Select(f => f.Name));
                }
            }

            IncrementPosition();
            
            referenceValue.index = EditorGUI.Popup(_position, "Value", referenceValue.index, fullNames.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                referenceValue.objectInfo.FieldName = fieldNames[referenceValue.index];
                referenceValue.objectInfo.ComponentName = components.Count > 0 ? 
                    components[referenceValue.index].GetType().Name : null;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _totalPropertyHeight;
        }

        private void SetPosition()
        {
            _position.height = LineHeight;
            _totalPropertyHeight = LineHeight;
            _position.x -= IndentWidth;
            _position.width += IndentWidth;
        }

        private void IncrementPosition()
        {
            _position.y += LineHeight + MarginBetweenFields;
            _totalPropertyHeight += LineHeight + MarginBetweenFields;
        }

        private ReferenceValue<T> GetReferenceValue(SerializedProperty property)
        {
            ReferenceValue<T> referenceValue;

            var value = _propertyFieldInfo.GetValue(Target);
            switch (value)
            {
                case null:
                case IList referenceValues when
                    referenceValues.Count == 0 || GetArrayIndex(property.propertyPath) >= referenceValues.Count:
                    return null;
                case IList referenceValues:
                {
                    var index = GetArrayIndex(property.propertyPath);
                    referenceValue = (ReferenceValue<T>)referenceValues[index];
                    break;
                }
                default:
                    referenceValue = (ReferenceValue<T>)value;
                    break;
            }

            return referenceValue;
        }

        private static int GetArrayIndex(string propertyPath)
        {
            return int.TryParse(
                Regex.Match(propertyPath, @".+\[(\d+)]").Groups[1].Value, out var index) 
                ? index : -1;
        }
    }

    [CustomPropertyDrawer(typeof(BooleanReferenceValue))]
    public class BooleanReferenceValuePropertyDrawer : ReferenceValuePropertyDrawer<bool> { }

    [CustomPropertyDrawer(typeof(StringReferenceValue))]
    public class StringReferenceValuePropertyDrawer : ReferenceValuePropertyDrawer<string> { }

    [CustomPropertyDrawer(typeof(IntegerReferenceValue))]
    public class IntegerReferenceValuePropertyDrawer : ReferenceValuePropertyDrawer<int> { }

    [CustomPropertyDrawer(typeof(FloatReferenceValue))]
    public class FloatReferenceValuePropertyDrawer : ReferenceValuePropertyDrawer<float> { }

    [CustomPropertyDrawer(typeof(Vector2ReferenceValue))]
    public class Vector2ReferenceValuePropertyDrawer : ReferenceValuePropertyDrawer<Vector2> { }

    [CustomPropertyDrawer(typeof(Vector3ReferenceValue))]
    public class Vector3ReferenceValuePropertyDrawer : ReferenceValuePropertyDrawer<Vector3> { }
}