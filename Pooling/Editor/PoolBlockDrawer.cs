#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Fralle.Core.Pooling
{
  [CustomPropertyDrawer(typeof(PoolBlock))]
  public class PoolBlockDrawer : PropertyDrawer
  {
    readonly int lineHeight = 18;

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
      float height = 4f;
      SerializedProperty emptyBehavior = prop.FindPropertyRelative("EmptyBehavior");
      if (emptyBehavior.enumValueIndex == (int)EmptyBehavior.Grow)
      {
        height += 1f;
      }

      return (height * lineHeight);
    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
      EditorGUI.BeginProperty(position, label, prop);

      int indent = EditorGUI.indentLevel;

      SerializedProperty size = prop.FindPropertyRelative("Size");
      SerializedProperty emptyBehavior = prop.FindPropertyRelative("EmptyBehavior");
      SerializedProperty maxSize = prop.FindPropertyRelative("MaxSize");
      SerializedProperty maxEmptyBehavior = prop.FindPropertyRelative("MaxEmptyBehavior");
      SerializedProperty prefab = prop.FindPropertyRelative("Prefab");
      SerializedProperty printLogOnQuit = prop.FindPropertyRelative("PrintLogOnQuit");

      float height = 0f;

      Rect labelR = new Rect(position.x, position.y + height, 45f, lineHeight - 2f);
      Rect propR = new Rect(position.x + 50f, position.y + height, position.width - 50f, lineHeight - 2f);
      EditorGUI.LabelField(labelR, new GUIContent("Prefab", "Tooltip"), EditorStyles.label);
      EditorGUI.PropertyField(propR, prefab, GUIContent.none);
      height += lineHeight;

      labelR = new Rect(position.x, position.y + height, 30f, lineHeight - 2f);
      propR = new Rect(position.x + 30f, position.y + height, 50f, lineHeight - 2f);
      EditorGUI.LabelField(labelR, new GUIContent("Size", "Tooltip"), EditorStyles.label);
      EditorGUI.PropertyField(propR, size, GUIContent.none);

      labelR = new Rect(position.x + 100, position.y + height, 100f, lineHeight - 2f);
      propR = new Rect(position.x + 200f, position.y + height, 80f, lineHeight - 2f);
      EditorGUI.LabelField(labelR, new GUIContent("Empty Behavior", "Tooltip"), EditorStyles.label);
      EditorGUI.PropertyField(propR, emptyBehavior, GUIContent.none);

      if (emptyBehavior.enumValueIndex == (int)EmptyBehavior.Grow)
      {
        height += lineHeight;
        labelR = new Rect(position.x, position.y + height, 55f, lineHeight - 2f);
        propR = new Rect(position.x + 60f, position.y + height, 50f, lineHeight - 2f);
        EditorGUI.LabelField(labelR, new GUIContent("Max Size", "Tooltip"), EditorStyles.label);
        EditorGUI.PropertyField(propR, maxSize, GUIContent.none);
        labelR = new Rect(position.x + 125f, position.y + height, 100f, lineHeight - 2f);
        propR = new Rect(position.x + 225f, position.y + height, 80f, lineHeight - 2f);
        EditorGUI.LabelField(labelR, new GUIContent("Empty Behavior", "Tooltip"), EditorStyles.label);
        EditorGUI.PropertyField(propR, maxEmptyBehavior, GUIContent.none);

      }
      height += lineHeight + 4f;

      labelR = new Rect(position.x, position.y + height, 100f, lineHeight - 2f);
      EditorGUI.LabelField(labelR, new GUIContent("Debug Options:"), EditorStyles.boldLabel);
      height += lineHeight - 3f;

      labelR = new Rect(position.x, position.y + height, 100f, lineHeight - 2f);
      propR = new Rect(position.x + 100f, position.y + height, 20f, lineHeight - 2f);
      EditorGUI.LabelField(labelR, new GUIContent("Print Log On Quit", "When the scene is stopped, creates a report showing pool usage:\n\n" +
        "Start Size - Size of pool when beginning the scene.\n\n" +
        "Init Added - Number of objects added by InitializeSpawn() at runtime.\n\n" +
        "Grow Objects - Number of objects added with EMptyBehavior.Grow.\n\n" +
        "End Size - Total objects of this pool, active and inactive, at the time of the log report.\n\n" +
        "Failed Spawns - Number of Spawn() requests that didn't return a spawn.\n\n" +
        "Reused Objects - Number of times an object was reused before despawning normally.\n\n" +
        "Most Objects Active - The most items for this pool active at once."), EditorStyles.label);
      EditorGUI.PropertyField(propR, printLogOnQuit, GUIContent.none);

      propR = new Rect(position.x + 150, position.y + height, 70f, lineHeight - 2f);
      if (GUI.Button(propR, new GUIContent("Print Log", "Prints a log showing current pool usage:\n\n" +
        "Start Size - Size of pool when beginning the scene.\n\n" +
        "Init Added - Number of objects added by InitializeSpawn() at runtime.\n\n" +
        "Grow Objects - Number of objects added with EMptyBehavior.Grow.\n\n" +
        "End Size - Total objects of this pool, active and inactive, at the time of the log report.\n\n" +
        "Failed Spawns - Number of Spawn() requests that didn't return a spawn.\n\n" +
        "Reused Objects - Number of times an object was reused before despawning normally.\n\n" +
        "Most Objects Active - The most items for this pool active at once."), EditorStyles.miniButton))
      {
        (prop.serializedObject.targetObject as Pool)?.PrintLog();
      }
      if (!EditorApplication.isPlaying)
      { GUI.enabled = false; }
      propR = new Rect(position.x + 230, position.y + height, 60f, lineHeight - 2f);
      if (GUI.Button(propR, new GUIContent("Spawn", "Spawn an object from the pool. (Only in play mode.)"), EditorStyles.miniButton))
      {
        if (EditorApplication.isPlaying)
        {
          (prop.serializedObject.targetObject as Pool)?.Spawn();
        }
      }
      if (!EditorApplication.isPlaying)
      { GUI.enabled = false; }


      EditorGUI.indentLevel = indent;
      EditorGUI.EndProperty();
    }
  }
}
#endif
