using UnityEngine;

namespace Fralle.Core.Basics
{
  public class FollowTransform : MonoBehaviour
  {
    public Transform transformToFollow;
    [SerializeField] bool position;
    [SerializeField] bool rotation;

    void LateUpdate()
    {
      if (transformToFollow == null)
        return;

      if (position)
        transform.position = transformToFollow.position;
      if (rotation)
        transform.rotation = transformToFollow.rotation;
    }
  }
}
