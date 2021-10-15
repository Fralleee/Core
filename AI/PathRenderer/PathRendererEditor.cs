#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Fralle.Core
{
  [CustomEditor(typeof(PathRenderer)), CanEditMultipleObjects]
  public class PathRendererEditor : Editor
  {
    protected virtual void OnSceneGUI()
    {
      Tools.current = Tool.None;
      PathRenderer pathRenderer = (PathRenderer)target;

      Event e = Event.current;
      switch (e.type)
      {
        case EventType.KeyDown:
        {
          if (Event.current.keyCode == KeyCode.D && Event.current.modifiers == EventModifiers.Control)
          {
            pathRenderer.points.Add(pathRenderer.points[pathRenderer.points.Count - 1]);
            Event.current.Use();
          }
          else if (Event.current.keyCode == KeyCode.Delete)
          {
            pathRenderer.points.RemoveAt(pathRenderer.points.Count - 1);
            Event.current.Use();
            pathRenderer.CalculatePath();
          }
          break;
        }
      }

      EditorGUI.BeginChangeCheck();
      Vector3[] newPositions = new Vector3[pathRenderer.points.Count];
      for (int i = 0; i < pathRenderer.points.Count; i++)
        newPositions[i] = Handles.PositionHandle(pathRenderer.points[i], Quaternion.identity);


      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(pathRenderer, "Change Look At Target Position");
        for (int i = 0; i < pathRenderer.points.Count; i++)
          pathRenderer.points[i] = newPositions[i];

        pathRenderer.CalculatePath();
      }
    }
  }
}
#endif
