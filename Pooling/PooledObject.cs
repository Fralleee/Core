using UnityEngine;

namespace Fralle.Core.Pooling
{
  public class PooledObject : MonoBehaviour
  {
    [HideInInspector] public Pool poolScript; // stores the location of the object pool script for this object
    [HideInInspector] public float timeSpawned;

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
      if (!poolScript)
        return false;

      poolScript.Despawn(gameObject, this);
      return true;
    }

    void OnDestroy()
    {
      if (!isQuitting)
        Debug.LogWarning($"{name} was Destroyed instead of despawned");
    }
  }
}
