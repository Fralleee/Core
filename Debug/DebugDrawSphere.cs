using UnityEngine;

namespace Fralle.Core
{
  public class DebugDrawSphere : MonoBehaviour
  {
    [SerializeField] Color color = Color.red;
    [SerializeField] float radius = 0.05f;

    void OnDrawGizmos()
    {
      Gizmos.color = color;
      Gizmos.DrawSphere(transform.position, radius);
    }
  }
}
