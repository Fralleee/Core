using UnityEngine;

namespace Fralle.Core.Basics
{
  public class FollowTransform : MonoBehaviour
  {
    public Transform transformToFollow;

    void LateUpdate()
    {
      transform.SetPositionAndRotation(transformToFollow.position, transformToFollow.rotation);
    }
  }
}
