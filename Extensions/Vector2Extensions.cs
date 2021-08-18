using UnityEngine;

namespace Fralle.Core
{
  public static class Vector2Extensions
  {
    public static Vector3 ToVector3(this Vector2 vector) => new Vector3(vector.x, 0, vector.y);

    public static float GetValueBetween(this Vector2 vector) => Random.Range(vector.x, vector.y);
  }
}
