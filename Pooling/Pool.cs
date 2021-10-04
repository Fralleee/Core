using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core.Pooling
{
  public class Pool : MonoBehaviour
  {
    public PoolBlock poolBlock;

    public Stack<PoolItem> PoolStack;
    [HideInInspector] public List<PoolItem> masterPool; // only used when using EmptyBehavior.ReuseOldest

    int addedObjects;
    int failedSpawns;
    int reusedObjects;
    int peakObjects;
    int origSize;
    int initSize;
    bool loaded;

    void OnValidate()
    {
      if (!loaded && poolBlock != null && poolBlock.maxSize <= poolBlock.size)
      {
        poolBlock.maxSize = poolBlock.size * 2;
      }
    }

    void Awake()
    {
      loaded = true;

      // required to allow creation or modification of pools at runtime. (Timing of script creation and initialization can get wonkey)
      poolBlock = poolBlock == null ? new PoolBlock(0, EmptyBehavior.Grow, 0, MaxEmptyBehavior.Fail, null, false) : new PoolBlock(poolBlock.size, poolBlock.emptyBehavior, poolBlock.maxSize, poolBlock.maxEmptyBehavior, poolBlock.prefab, poolBlock.printLogOnQuit);
      PoolStack = new Stack<PoolItem>();
      masterPool = new List<PoolItem>();

      origSize = Mathf.Max(0, poolBlock.size);
      poolBlock.size = 0;

      for (int i = 0; i < origSize; i++)
      {
        CreateObject(true);
      }
    }

    void Start()
    {
      //Invoke("StatInit", 0); // for logging after dynamic creation of pool objects from other scripts
      StatInit();
    }

    void StatInit()
    { // for logging after dynamic creation of pool objects from other scripts
      initSize = poolBlock.size - origSize;
    }

    public GameObject Spawn()
    { // use to call spawn directly from the pool, and also used by the "Spawn" button in the editor
      return Spawn(null, Vector3.zero, Quaternion.identity, false);
    }
    public GameObject Spawn(int? child)
    { // use to call spawn directly from the pool
      return Spawn(child, Vector3.zero, Quaternion.identity, false);
    }
    public GameObject Spawn(Vector3 pos, Quaternion rot)
    { // use to call spawn directly from the pool
      return Spawn(null, pos, rot, true);
    }
    public GameObject Spawn(int? child, Vector3 pos, Quaternion rot)
    { // use to call spawn directly from the pool
      return Spawn(child, pos, rot, true);
    }
    public GameObject Spawn(int? child, Vector3 pos, Quaternion rot, bool usePosRot, Transform parent = null)
    {
      GameObject obj = GetObject();
      if (!obj)
      { return null; } // early out

      obj.SetActive(false); // reset item in case object is being reused, has no effect if object is already disabled
      obj.transform.SetParent(parent, false);
      obj.transform.position = usePosRot ? pos : transform.position;
      obj.transform.rotation = usePosRot ? rot : transform.rotation;

      obj.SetActive(true);

      if (child != null && child < obj.transform.childCount)
      { // activate a specific child
        obj.transform.GetChild((int)child).gameObject.SetActive(true);
      }

      if (peakObjects < poolBlock.size - PoolStack.Count)
      { peakObjects = poolBlock.size - PoolStack.Count; } // for logging
      return obj;
    }

    public void Despawn(GameObject obj, PooledObject pooledObject)
    { // return an object back to this pool
      if (obj.transform.parent == transform)
      { return; } // already in pool
      obj.SetActive(false);
      obj.transform.SetParent(transform, false);
      obj.transform.localPosition = Vector3.zero;
      obj.transform.localRotation = Quaternion.identity;
      pooledObject.CancelInvoke();
      PoolStack.Push(new PoolItem(obj, pooledObject));
    }

    public GameObject GetObject()
    { // get object from pool, creating one if necessary and if settings allow
      GameObject result = null;
      if (PoolStack.Count == 0)
      {
        switch (poolBlock.emptyBehavior)
        {
          case EmptyBehavior.Fail:
            failedSpawns++;
            return null;
          case EmptyBehavior.ReuseOldest:
          {
            result = FindOldest();
            if (result)
            { reusedObjects++; }

            break;
          }
          case EmptyBehavior.Grow:
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }

        if (poolBlock.emptyBehavior != EmptyBehavior.Grow)
          return result;
        if (poolBlock.size >= poolBlock.maxSize)
        {
          switch (poolBlock.maxEmptyBehavior)
          {
            case MaxEmptyBehavior.Fail:
              failedSpawns++;
              return null;
            case MaxEmptyBehavior.ReuseOldest:
            {
              result = FindOldest();
              if (result)
              { reusedObjects++; }

              break;
            }
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
        else
        {
          addedObjects++;
          return CreateObject();
        }
      }
      else
      {
        PoolStack.Peek().refScript.timeSpawned = Time.time;
        return PoolStack.Pop().obj;
      }
      return result;
    }

    GameObject FindOldest()
    { // will also set timeSpawned for returned object
      GameObject result = null;
      int oldestIndex = 0;
      float oldestTime = Mathf.Infinity;
      if (masterPool.Count <= 0)
        return null;
      for (int i = 0; i < masterPool.Count; i++)
      {
        if (masterPool[i] == null || !masterPool[i].obj)
          continue;

        if (!(masterPool[i].refScript.timeSpawned < oldestTime))
          continue;
        oldestTime = masterPool[i].refScript.timeSpawned;
        result = masterPool[i].obj;
        oldestIndex = i;
      }
      masterPool[oldestIndex].refScript.timeSpawned = Time.time;
      return result;
    }

    public GameObject CreateObject()
    {
      return CreateObject(false);
    }

    public GameObject CreateObject(bool createInPool)
    { // true when creating an item in the pool without spawing it
      if (!poolBlock.prefab)
      {
        Debug.LogWarning($"SOMETHING WENT VERY WRONG: {name}");
        return null;
      }

      var obj = Instantiate(poolBlock.prefab, transform.position, transform.rotation);
      PooledObject oprScript = obj.GetComponent<PooledObject>();
      if (!oprScript)
      { oprScript = obj.AddComponent<PooledObject>(); }
      oprScript.poolScript = this;
      oprScript.timeSpawned = Time.time;
      if (!obj || !oprScript)
        Debug.LogWarning($"Found nulls in {poolBlock.prefab}");
      masterPool.Add(new PoolItem(obj, oprScript));

      if (createInPool)
      {
        PoolStack.Push(new PoolItem(obj, oprScript));
        obj.SetActive(false);
        obj.transform.SetParent(transform, false);
      }
      poolBlock.size++;
      return obj;
    }

    public int GetActiveCount => poolBlock.size - PoolStack.Count;

    public int GetAvailableCount => PoolStack.Count;

    void OnApplicationQuit()
    {
      if (poolBlock.printLogOnQuit)
      {
        PrintLog();
      }
    }

    public void PrintLog()
    {
      Debug.Log(transform.name + ":       Start Size: " + origSize + "    Init Added: " + initSize + "    Grow Objects: " + addedObjects + "    End Size: " + poolBlock.size + "\n" +
        "    Failed Spawns: " + failedSpawns + "    Reused Objects: " + reusedObjects + "     Most objects active at once: " + peakObjects);
    }

  }
}
