using UnityEngine;

namespace Fralle.Core
{
  public class HandIKTarget : MonoBehaviour
  {
    public Hand hand;

    Transform transformToFollow;
    bool isEnabled;

    public void Target(Transform t)
    {
      transformToFollow = t;
      isEnabled = transformToFollow != null;
    }

    void LateUpdate()
    {
      if (!isEnabled)
        return;

      transform.position = transformToFollow.position;
      transform.rotation = transformToFollow.rotation;
    }
  }
}
