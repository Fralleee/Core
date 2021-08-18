using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Fralle.Core
{
  public class HandIK : MonoBehaviour
  {
    public Hand hand;
    ChainIKConstraint chainIKConstraint;

    void Awake()
    {
      chainIKConstraint = GetComponentInParent<ChainIKConstraint>();
    }

    public void Toggle(bool enabled = true)
    {
      if (chainIKConstraint)
        chainIKConstraint.weight = enabled ? 1 : 0;
    }
  }
}
