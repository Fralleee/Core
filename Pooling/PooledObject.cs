using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.Core.Pooling
{
  public class PooledObject : MonoBehaviour
  {
    [FormerlySerializedAs("PoolScript")] [HideInInspector] public Pool poolScript; // stores the location of the object pool script for this object
    [FormerlySerializedAs("TimeSpawned")] [HideInInspector] public float timeSpawned;

    bool isQuitting;

    void Awake()
    {
      Application.quitting += Application_quitting;
    }

    void Application_quitting()
    {
      isQuitting = true;
    }

    public bool Despawn(float del)
    { // -1 will use delay specified in this script
      if (del >= 0)
      {
        if (poolScript)
        {
          Invoke("DoDespawn", del);
          gameObject.SetActive(false);
          return true;
        }
        else
        {
          return false;
        }
      }
      else
      {
        return DoDespawn();
      }
    }

    bool DoDespawn()
    {
      if (poolScript)
      {
        poolScript.Despawn(gameObject, this);
        return true;
      }
      return false;
    }

    void OnDestroy()
    {
      if (!isQuitting)
        Debug.LogWarning($"{name} was Destroyed instead of despawned");
    }
  }
}
