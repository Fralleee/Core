using UnityEngine;

namespace Fralle.Core
{
  public class LookRotationTransformer : MasterRotator
  {
    Quaternion lookRotation = Quaternion.identity;

    public override Quaternion GetRotation() => lookRotation;
    public void ApplyLookRotation(Quaternion quaternion) => lookRotation = quaternion;
  }
}
