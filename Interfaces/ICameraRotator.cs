using UnityEngine;

namespace Fralle.Core
{
  public interface ICameraRotator
  {
    void ApplyLookRotation(Quaternion quaternion);
  }
}
