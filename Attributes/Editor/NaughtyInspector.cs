using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Fralle.Core
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof(Object), true)]
  public class NaughtyInspector : Editor
  {
    private List<SerializedProperty> serializedProperties = new List<SerializedProperty>();
    private IEnumerable<FieldInfo> nonSerializedFields;
    private IEnumerable<PropertyInfo> nativeProperties;
    private IEnumerable<MethodInfo> methods;
    private Dictionary<string, SavedBool> foldouts = new Dictionary<string, SavedBool>();

    protected virtual void OnEnable()
    {
      nonSerializedFields = ReflectionUtility.GetAllFields(
        target, f => f.GetCustomAttributes(typeof(ShowNonSerializedFieldAttribute), true).Length > 0);

      nativeProperties = ReflectionUtility.GetAllProperties(
        target, p => p.GetCustomAttributes(typeof(ShowNativePropertyAttribute), true).Length > 0);

      methods = ReflectionUtility.GetAllMethods(
        target, m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);
    }

    protected virtual void OnDisable()
    {
      ReorderableListPropertyDrawer.Instance.ClearCache();
    }

    public override void OnInspectorGUI()
    {
      GetSerializedProperties(ref serializedProperties);

      bool anyNaughtyAttribute = serializedProperties.Any(p => PropertyUtility.GetAttribute<ICustomAttribute>(p) != null);
      if (!anyNaughtyAttribute)
      {
        DrawDefaultInspector();
      }
      else
      {
        DrawSerializedProperties();
      }

      DrawNonSerializedFields();
      DrawNativeProperties();
      DrawButtons();
    }

    protected void GetSerializedProperties(ref List<SerializedProperty> outSerializedProperties)
    {
      outSerializedProperties.Clear();
      using SerializedProperty iterator = serializedObject.GetIterator();
      if (!iterator.NextVisible(true))
        return;

      do
      {
        outSerializedProperties.Add(serializedObject.FindProperty(iterator.name));
      }
      while (iterator.NextVisible(false));
    }

    protected void DrawSerializedProperties()
    {
      serializedObject.Update();

      // Draw non-grouped serialized properties
      foreach (SerializedProperty property in GetNonGroupedProperties(serializedProperties))
      {
        if (property.name.Equals("m_Script", System.StringComparison.Ordinal))
        {
          using (new EditorGUI.DisabledScope(disabled: true))
          {
            EditorGUILayout.PropertyField(property);
          }
        }
        else
        {
          NaughtyEditorGui.PropertyField_Layout(property, includeChildren: true);
        }
      }

      // Draw grouped serialized properties
      foreach (IGrouping<string, SerializedProperty> group in GetGroupedProperties(serializedProperties))
      {
        IEnumerable<SerializedProperty> visibleProperties = group.Where(PropertyUtility.IsVisible);
        IEnumerable<SerializedProperty> properties = visibleProperties as SerializedProperty[] ?? visibleProperties.ToArray();
        if (!properties.Any())
        {
          continue;
        }

        NaughtyEditorGui.BeginBoxGroup_Layout(group.Key);
        foreach (SerializedProperty property in properties)
        {
          NaughtyEditorGui.PropertyField_Layout(property, includeChildren: true);
        }

        NaughtyEditorGui.EndBoxGroup_Layout();
      }

      // Draw foldout serialized properties
      foreach (IGrouping<string, SerializedProperty> group in GetFoldoutProperties(serializedProperties))
      {
        IEnumerable<SerializedProperty> visibleProperties = group.Where(PropertyUtility.IsVisible);
        IEnumerable<SerializedProperty> properties = visibleProperties as SerializedProperty[] ?? visibleProperties.ToArray();
        if (!properties.Any())
        {
          continue;
        }

        if (!foldouts.ContainsKey(group.Key))
        {
          foldouts[group.Key] = new SavedBool($"{target.GetInstanceID()}.{group.Key}", false);
        }

        foldouts[group.Key].Value = EditorGUILayout.Foldout(foldouts[group.Key].Value, group.Key, true);
        if (foldouts[group.Key].Value)
        {
          foreach (SerializedProperty property in properties)
          {
            NaughtyEditorGui.PropertyField_Layout(property, true);
          }
        }
      }

      serializedObject.ApplyModifiedProperties();
    }

    protected void DrawNonSerializedFields(bool drawHeader = false)
    {
      if (nonSerializedFields.Any())
      {
        if (drawHeader)
        {
          EditorGUILayout.Space();
          EditorGUILayout.LabelField("Non-Serialized Fields", GetHeaderGuiStyle());
          NaughtyEditorGui.HorizontalLine(
            EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());
        }

        foreach (FieldInfo field in nonSerializedFields)
        {
          NaughtyEditorGui.NonSerializedField_Layout(serializedObject.targetObject, field);
        }
      }
    }

    protected void DrawNativeProperties(bool drawHeader = false)
    {
      if (!nativeProperties.Any())
        return;
      if (drawHeader)
      {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Native Properties", GetHeaderGuiStyle());
        NaughtyEditorGui.HorizontalLine(
          EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());
      }

      foreach (PropertyInfo property in nativeProperties)
      {
        NaughtyEditorGui.NativeProperty_Layout(serializedObject.targetObject, property);
      }
    }

    protected void DrawButtons(bool drawHeader = false)
    {
      if (methods.Any())
      {
        if (drawHeader)
        {
          EditorGUILayout.Space();
          EditorGUILayout.LabelField("Buttons", GetHeaderGuiStyle());
          NaughtyEditorGui.HorizontalLine(
            EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());
        }

        foreach (MethodInfo method in methods)
        {
          NaughtyEditorGui.Button(serializedObject.targetObject, method);
        }
      }
    }

    private static IEnumerable<SerializedProperty> GetNonGroupedProperties(IEnumerable<SerializedProperty> properties)
    {
      return properties.Where(p => PropertyUtility.GetAttribute<IGroupAttribute>(p) == null);
    }

    private static IEnumerable<IGrouping<string, SerializedProperty>> GetGroupedProperties(IEnumerable<SerializedProperty> properties)
    {
      return properties
        .Where(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p) != null)
        .GroupBy(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p).Name);
    }

    private static IEnumerable<IGrouping<string, SerializedProperty>> GetFoldoutProperties(IEnumerable<SerializedProperty> properties)
    {
      return properties
        .Where(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p) != null)
        .GroupBy(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p).Name);
    }

    private static GUIStyle GetHeaderGuiStyle()
    {
      GUIStyle style = new GUIStyle(EditorStyles.centeredGreyMiniLabel) { fontStyle = FontStyle.Bold, alignment = TextAnchor.UpperCenter };
      return style;
    }
  }
}
