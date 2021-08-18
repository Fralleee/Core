using UnityEngine;

namespace Fralle.Core
{
  public class HandIKTarget : MonoBehaviour
  {
    public Hand hand;

    Transform transformToFollow;

    public void Target(Transform t)
    {
      transformToFollow = t;
    }

    void LateUpdate()
    {
      if (transformToFollow == null)
        return;

      transform.position = transformToFollow.position;
      transform.rotation = transformToFollow.rotation;
    }
  }
}
