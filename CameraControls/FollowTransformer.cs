using UnityEngine;

namespace Fralle.Core
{
  public class FollowTransformer : MasterPositioner
  {
    public Transform TransformToFollow;

    public override Vector3 GetPosition() => TransformToFollow.position;
  }
}
