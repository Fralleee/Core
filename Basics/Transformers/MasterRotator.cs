using UnityEngine;

namespace Fralle.Core
{
  public abstract class MasterRotator : Transformer, IRotator
  {
    public abstract Quaternion GetRotation();
  }
}
