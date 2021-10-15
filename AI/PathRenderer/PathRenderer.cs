using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Fralle.Core
{
  [RequireComponent(typeof(LineRenderer))]
  public class PathRenderer : MonoBehaviour
  {
    public List<Vector3> points;
    [SerializeField] float heightOffset;

    LineRenderer lineRenderer;
    int index;

    public void CalculatePath()
    {
      if (points.Count < 2)
        return;

      lineRenderer = GetComponent<LineRenderer>();
      lineRenderer.positionCount = 0;

      index = 0;
      for (int i = 0; i < points.Count - 1; i++)
        CalculatePath(points[i], points[i + 1]);
    }

    void CalculatePath(Vector3 start, Vector3 end)
    {
      NavMeshPath path = new NavMeshPath();
      if (NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path))
      {
        lineRenderer.positionCount += path.corners.Length;
        for (int i = 0; i < path.corners.Length; i++)
        {
          lineRenderer.SetPosition(index, path.corners[i] + Vector3.up * heightOffset);
          index++;
        }
      }
    }

    void OnValidate()
    {
      CalculatePath();
    }
  }
}
