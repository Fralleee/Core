using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fralle.Core.Pooling
{
  public class PoolManager : MonoBehaviour
  {
    [HideInInspector] public Dictionary<GameObject, Pool> PoolRef;

    void Awake()
    {
      CheckDict();
    }

    void Start()
    {
      PopulateDict();
    }

    void CheckDict()
    {
      PoolRef ??= new Dictionary<GameObject, Pool>();
    }

    void PopulateDict()
    {
      Pool[] pools = gameObject.GetComponentsInChildren<Pool>();
      foreach (Pool pool in pools)
      {
        if (pool.poolBlock.prefab == null)
          continue;
        if (PoolRef.ContainsKey(pool.poolBlock.prefab))
          Debug.LogWarning($"Already found pool for {pool.poolBlock.prefab}.");
        else
          PoolRef.Add(pool.poolBlock.prefab, pool);
      }
    }

    public bool InitializeSpawn(GameObject obj, float addPool, int minPool, EmptyBehavior emptyBehavior, MaxEmptyBehavior maxEmptyBehavior, bool modBehavior)
    {
      if (obj == null)
        return false;

      CheckDict();

      if (PoolRef.ContainsKey(obj) && PoolRef[obj] == null)
      { // check for broken reference
        PoolRef.Remove(obj); // remove it
      }

      bool result = PoolRef.ContainsKey(obj);

      if (!result || (!(addPool > 0) && minPool <= 0))
        return result;
      int size = PoolRef[obj].poolBlock.size;
      int l1 = 0;
      int l2 = 0;
      if (addPool >= 0)
      { // not negative
        if (addPool < 1)
        { // is a percentage
          l2 = Mathf.RoundToInt(size * addPool);
        }
        else
        { // not a percentage
          l1 = Mathf.RoundToInt(addPool);
        }
      }
      int loop = 0;
      int a = size == 0 ? 0 : Mathf.Max(l1, l2);
      if (size < minPool)
      { loop = minPool - size; }
      loop += a;
      for (int i = 0; i < loop; i++)
      {
        PoolRef[obj].CreateObject(true);
      }
      PoolRef[obj].poolBlock.maxSize = PoolRef[obj].poolBlock.size * 2;
      if (!modBehavior)
        return true;

      PoolRef[obj].poolBlock.emptyBehavior = emptyBehavior;
      PoolRef[obj].poolBlock.maxEmptyBehavior = maxEmptyBehavior;

      return true;
    }

    public GameObject Spawn(GameObject obj, int? child, Vector3 pos, Quaternion rot, bool usePosRot, Transform parent)
    {
      if (obj == null)
      {
        return null;
      } // object wasn't defined
      CheckDict();

      if (!PoolRef.ContainsKey(obj))
        return null;
      // reference already created
      if (PoolRef[obj] != null)
      { // make sure pool still exsists
        return PoolRef[obj].Spawn(child, pos, rot, usePosRot, parent); // create spawn
      }

      // pool no longer exsists
      PoolRef.Remove(obj); // remove reference
      return null;

    }

    public int GetActiveCount(GameObject prefab)
    {
      if (prefab == null)
      { return 0; } // object wasn't defined

      Pool childScript = PoolRef.ContainsKey(prefab) ? PoolRef[prefab] : null;
      if (childScript == null)
      { // pool not found
        return 0;
      }

      return childScript.poolBlock.size - childScript.PoolStack.Count;
    }

    public int GetAvailableCount(GameObject prefab)
    {
      if (prefab == null)
      { return 0; } // object wasn't defined

      Pool childScript = PoolRef.ContainsKey(prefab) ? PoolRef[prefab] : null;
      return childScript == null ? 0 : childScript.PoolStack.Count;
    }

    public bool RemoveAll()
    {
      GameObject[] tempObj = new GameObject[PoolRef.Count];
      int i = 0;
      foreach (var obj in PoolRef.Keys.Where(obj => PoolRef[obj] != null))
      {
        tempObj[i] = obj;
        i++;
      }

      return tempObj.All(t1 => t1 == null || RemovePool(t1));
    }

    public bool DespawnAll()
    {
      bool result = true;
      foreach (var obj in PoolRef.Keys.Where(obj => !DespawnPool(obj)))
      {
        result = false;
      }
      return result;
    }

    public bool RemovePool(GameObject prefab)
    {
      if (prefab == null)
      { return false; } // object wasn't defined

      Pool childScript = null;
      if (PoolRef.ContainsKey(prefab))
      { // reference already created
        childScript = PoolRef[prefab];
      }

      if (childScript == null)
      { // pool not found
        return false;
      }

      bool result = DespawnPool(prefab);
      Destroy(childScript.gameObject);
      PoolRef.Remove(prefab);
      return result;
    }

    public bool DespawnPool(GameObject prefab)
    {
      Debug.LogWarning($"Despawning Pool {prefab}");
      if (prefab == null)
      { return false; } // object wasn't defined
      Pool childScript = null;
      if (PoolRef.ContainsKey(prefab))
      { // reference already created
        childScript = PoolRef[prefab];
      }
      if (childScript == null)
      { // pool not found
        return false;
      }

      foreach (var t in childScript.masterPool)
      {
        childScript.Despawn(t.obj, t.refScript);
      }
      return true;
    }

  }
}
