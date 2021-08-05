#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Fralle.Core
{
  [CustomPropertyDrawer(typeof(AllowNestingAttribute))]
  public class AllowNestingPropertyDrawer : PropertyDrawerBase
  {
    protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginProperty(rect, label, property);
      EditorGUI.PropertyField(rect, property, label, true);
      EditorGUI.EndProperty();
    }
  }
}
#endif
