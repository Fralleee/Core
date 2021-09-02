using UnityEngine;

namespace Fralle.Core
{
  public class FollowTransformer : MasterPositioner
  {
    public Transform transformToFollow;

    public override Vector3 GetPosition() => transformToFollow.position;
  }
}
