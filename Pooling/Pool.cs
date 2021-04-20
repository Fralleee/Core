using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core.Pooling
{
	public class Pool : MonoBehaviour
	{
		public PoolBlock PoolBlock;

		[HideInInspector] public Stack<PoolItem> PoolStack;
		[HideInInspector] public List<PoolItem> MasterPool; // only used when using EmptyBehavior.ReuseOldest

		int addedObjects;
		int failedSpawns;
		int reusedObjects;
		int peakObjects;
		int origSize;
		int initSize;
		bool loaded;

#if UNITY_EDITOR
		void OnValidate()
		{
			if (!loaded && PoolBlock != null && PoolBlock.MaxSize <= PoolBlock.Size)
			{
				PoolBlock.MaxSize = PoolBlock.Size * 2;
			}
		}
#endif

		void Awake()
		{
			loaded = true;

			// required to allow creation or modification of pools at runtime. (Timing of script creation and initialization can get wonkey)
			if (PoolBlock == null)
			{
				PoolBlock = new PoolBlock(0, EmptyBehavior.Grow, 0, MaxEmptyBehavior.Fail, null, false);
			}
			else
			{
				PoolBlock = new PoolBlock(PoolBlock.Size, PoolBlock.EmptyBehavior, PoolBlock.MaxSize, PoolBlock.MaxEmptyBehavior, PoolBlock.Prefab, PoolBlock.PrintLogOnQuit);
			}
			PoolStack = new Stack<PoolItem>();
			MasterPool = new List<PoolItem>();

			origSize = Mathf.Max(0, PoolBlock.Size);
			PoolBlock.Size = 0;

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
			initSize = PoolBlock.Size - origSize;
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
			if (obj == null)
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

			if (peakObjects < PoolBlock.Size - PoolStack.Count)
			{ peakObjects = PoolBlock.Size - PoolStack.Count; } // for logging
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
				switch (PoolBlock.EmptyBehavior)
				{
					case EmptyBehavior.Fail:
						failedSpawns++; return null;
					case EmptyBehavior.ReuseOldest:
					{
						result = FindOldest();
						if (result != null)
						{ reusedObjects++; }

						break;
					}
				}

				if (PoolBlock.EmptyBehavior != EmptyBehavior.Grow) return result;
				if (PoolBlock.Size >= PoolBlock.MaxSize)
				{
					switch (PoolBlock.MaxEmptyBehavior)
					{
						case MaxEmptyBehavior.Fail:
							failedSpawns++; return null;
						case MaxEmptyBehavior.ReuseOldest:
						{
							result = FindOldest();
							if (result != null)
							{ reusedObjects++; }

							break;
						}
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
				PoolStack.Peek().RefScript.TimeSpawned = Time.time;
				return PoolStack.Pop().Obj;
			}
			return result;
		}

		GameObject FindOldest()
		{ // will also set timeSpawned for returned object
			GameObject result = null;
			int oldestIndex = 0;
			float oldestTime = Mathf.Infinity;
			if (MasterPool.Count <= 0) return null;
			for (int i = 0; i < MasterPool.Count; i++)
			{
				if (MasterPool[i] == null || MasterPool[i].Obj == null)
				{ continue; } // make sure object still exsists

				if (!(MasterPool[i].RefScript.TimeSpawned < oldestTime)) continue;
				oldestTime = MasterPool[i].RefScript.TimeSpawned;
				result = MasterPool[i].Obj;
				oldestIndex = i;
			}
			MasterPool[oldestIndex].RefScript.TimeSpawned = Time.time;
			return result;
		}

		public GameObject CreateObject()
		{
			return CreateObject(false);
		}
		public GameObject CreateObject(bool createInPool)
		{ // true when creating an item in the pool without spawing it
			GameObject obj = null;
			if (!PoolBlock.Prefab) return obj;
			obj = Instantiate(PoolBlock.Prefab, transform.position, transform.rotation);
			PooledObject oprScript = obj.GetComponent<PooledObject>();
			if (oprScript == null)
			{ oprScript = obj.AddComponent<PooledObject>(); }
			oprScript.PoolScript = this;
			oprScript.TimeSpawned = Time.time;
			MasterPool.Add(new PoolItem(obj, oprScript));

			if (createInPool)
			{
				PoolStack.Push(new PoolItem(obj, oprScript));
				obj.SetActive(false);
				obj.transform.SetParent(transform, false);
			}
			PoolBlock.Size++;
			return obj;
		}

		public int GetActiveCount()
		{
			return PoolBlock.Size - PoolStack.Count;
		}

		public int GetAvailableCount()
		{
			return PoolStack.Count;
		}

		void OnApplicationQuit()
		{
			if (PoolBlock.PrintLogOnQuit)
			{
				PrintLog();
			}
		}

		public void PrintLog()
		{
			Debug.Log(transform.name + ":       Start Size: " + origSize + "    Init Added: " + initSize + "    Grow Objects: " + addedObjects + "    End Size: " + PoolBlock.Size + "\n" +
				"    Failed Spawns: " + failedSpawns + "    Reused Objects: " + reusedObjects + "     Most objects active at once: " + peakObjects);
		}

	}
}
