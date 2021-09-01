using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Fralle.Core
{
  [CustomPropertyDrawer(typeof(Layer))]
  public class SingleUnityLayerPropertyDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginProperty(position, GUIContent.none, property);
      SerializedProperty layerIndex = property.FindPropertyRelative("layerIndex");
      position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
      if (layerIndex != null)
      {
        layerIndex.intValue = EditorGUI.LayerField(position, layerIndex.intValue);
      }
      EditorGUI.EndProperty();
    }
  }
}
#endif
