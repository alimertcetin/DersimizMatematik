using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace XIVEditor.Utils
{
    [CustomPropertyDrawer(typeof(DisplayWithoutEdit))]
    public class DisplayWithoutEditDrawer : PropertyDrawer
    {
        /// <summary>
        /// Display attribute and his value in inspector depending on the type
        /// Fill attribute needed
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.AnimationCurve:
                    break;
                case SerializedPropertyType.ArraySize:
                    break;
                case SerializedPropertyType.Boolean:
                    EditorGUI.LabelField(position, label, new GUIContent(property.boolValue.ToString()));
                    break;
                case SerializedPropertyType.Bounds:
                    break;
                case SerializedPropertyType.Character:
                    break;
                case SerializedPropertyType.Color:
                    break;
                case SerializedPropertyType.Enum:
                    EditorGUI.LabelField(position, label, new GUIContent(property.enumDisplayNames[property.enumValueIndex]));
                    break;
                case SerializedPropertyType.Float:
                    EditorGUI.LabelField(position, label, new GUIContent(property.floatValue.ToString()));
                    break;
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Gradient:
                    break;
                case SerializedPropertyType.Integer:
                    EditorGUI.LabelField(position, label, new GUIContent(property.intValue.ToString()));
                    break;
                case SerializedPropertyType.LayerMask:
                    break;
                case SerializedPropertyType.ObjectReference:
                    break;
                case SerializedPropertyType.Quaternion:
                    break;
                case SerializedPropertyType.Rect:
                    break;
                case SerializedPropertyType.String:
                    EditorGUI.LabelField(position, label, new GUIContent(property.stringValue));
                    break;
                case SerializedPropertyType.Vector2:
                    break;
                case SerializedPropertyType.Vector3:
                    break;
                case SerializedPropertyType.Vector4:
                    break;
            }
        }
    }
    
    public class ButtonAttribute : PropertyAttribute
    {
        public string label;
        
        public ButtonAttribute(string label)
        {
            this.label = label;
        }
    }
    
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw the default property field
            EditorGUI.PropertyField(position, property, label);

            var buttonAttribute = (ButtonAttribute)attribute;
            
            if (GUILayout.Button(buttonAttribute.label))
            {
                ReflectionUtils.GetMethods<ButtonAttribute>(fieldInfo.FieldType);
            }
        }
    }
}