using UnityEngine;

namespace Fralle.Core.Infrastructure
{
  public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
  {
    static T LocalInstance;
    public static bool Destroyed;

    public static T Instance
    {
      get
      {
        if (Destroyed)
          return null;

        if (LocalInstance != null)
          return LocalInstance;

        LocalInstance = (T)FindObjectOfType(typeof(T));

        if (LocalInstance != null)
          return LocalInstance;

        GameObject singletonObject = new GameObject();
        LocalInstance = singletonObject.AddComponent<T>();
        singletonObject.name = typeof(T).ToString() + " (Singleton)";

        DontDestroyOnLoad(singletonObject);

        return LocalInstance;
      }
    }

    protected virtual void Awake()
    {
      if (LocalInstance != null && LocalInstance != this as T)
      {
        Destroy(gameObject);
        return;
      }

      LocalInstance = this as T;
    }

    void OnApplicationQuit()
    {
      Destroyed = true;
    }

    protected virtual void OnDestroy()
    {
      Destroyed = true;
    }
  }
}
