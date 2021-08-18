using UnityEngine;

namespace Fralle.Core
{
  public static class LayerExtensions
  {
    public static bool IsInLayerMask(this LayerMask layermask, int layer)
    {
      return layermask == (layermask | (1 << layer));
    }
  }
}
