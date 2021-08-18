using UnityEngine;

namespace Fralle.Core
{
  public abstract class MasterPositioner : Transformer, IPositioner
  {
    public abstract Vector3 GetPosition();
  }
}
