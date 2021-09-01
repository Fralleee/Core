using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.Core
{
  public class FollowTransformer : MasterPositioner
  {
    [FormerlySerializedAs("TransformToFollow")] public Transform transformToFollow;

    public override Vector3 GetPosition() => transformToFollow.position;
  }
}
