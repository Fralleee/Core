using UnityEngine;

namespace Fralle.Core.CameraControls
{
  public class FollowTransformOffset : MonoBehaviour
  {
    public Transform transformToFollow;

    [SerializeField] float offset;

    void LateUpdate()
    {
      transform.position = transformToFollow.position + transformToFollow.forward * offset;
    }
  }
}
