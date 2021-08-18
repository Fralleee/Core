using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Fralle.Core
{
  [CustomPropertyDrawer(typeof(Layer))]
  public class SingleUnityLayerPropertyDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
      EditorGUI.BeginProperty(_position, GUIContent.none, _property);
      SerializedProperty layerIndex = _property.FindPropertyRelative("layerIndex");
      _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
      if (layerIndex != null)
      {
        layerIndex.intValue = EditorGUI.LayerField(_position, layerIndex.intValue);
      }
      EditorGUI.EndProperty();
    }
  }
}
#endif
