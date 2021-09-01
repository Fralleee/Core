using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.Core.Pooling
{
  [System.Serializable]
  public class PoolItem
  {
    [FormerlySerializedAs("Obj")] public GameObject obj;
    [FormerlySerializedAs("RefScript")] public PooledObject refScript;

    public PoolItem(GameObject obj, PooledObject refScript)
    {
      this.obj = obj;
      this.refScript = refScript;
    }
  }
}
