﻿#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Fralle.Core
{
  [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
  public class EnumFlagsPropertyDrawer : PropertyDrawerBase
  {
    protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
    {
      return (PropertyUtility.GetTargetObjectOfProperty(property) is Enum)
        ? GetPropertyHeight(property)
        : GetPropertyHeight(property) + GetHelpBoxHeight();
    }

    protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginProperty(rect, label, property);

      if (PropertyUtility.GetTargetObjectOfProperty(property) is Enum targetEnum)
      {
        Enum enumNew = EditorGUI.EnumFlagsField(rect, label.text, targetEnum);
        property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
      }
      else
      {
        string message = attribute.GetType().Name + " can be used only on enums";
        DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
      }

      EditorGUI.EndProperty();
    }
  }
}
#endif
