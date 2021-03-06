using System.Linq;
using UnityEngine;
// ReSharper disable UnusedMember.Global

namespace Fralle.Core
{
  public static class TransformExtensions
  {

    public static void EnableChildren(this Transform transform)
    {
      foreach (Transform t in transform)
        t.gameObject.SetActive(true);
    }

    public static void DisableChildren(this Transform transform)
    {
      foreach (Transform t in transform)
        t.gameObject.SetActive(false);
    }

    public static Vector3 DirectionTo(this Transform source, Transform destination)
    {
      return source.position.DirectionTo(destination.position);
    }

    public static void LookAtFlat(this Transform source, Transform target)
    {
      Vector3 position = new Vector3(target.position.x, source.position.y, target.position.z);
      source.LookAt(position);
    }

    public static Transform FindRecursively(this Transform aParent, string aName)
    {
      foreach (Transform child in aParent)
      {
        if (child.name == aName)
          return child;
        Transform result = child.FindRecursively(aName);
        if (result)
          return result;
      }
      return null;
    }

    public static Transform FindChildWithTag(this Transform parent, string tag)
    {
      Transform t = parent.transform;
      return t.Cast<Transform>().FirstOrDefault(tr => tr.CompareTag(tag));
    }

  }
}
