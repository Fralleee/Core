using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core.Pooling
{
	public class PoolManager : MonoBehaviour
	{
		public bool AllowCreate = true;
		public bool AllowModify = true;

		[Tooltip("When the scene is stopped, creates a report showing pool usage:\n\n" +
			"Start Size - Size of pool when beginning the scene.\n\n" +
			"Init Added - Number of objects added by InitializeSpawn() at runtime.\n\n" +
			"Grow Objects - Number of objects added with EmptyBehavior.Grow.\n\n" +
			"End Size - Total objects of this pool, active and inactive, at the time of the log report.\n\n" +
			"Failed Spawns - Number of Spawn() requests that didn't return a spawn.\n\n" +
			"Reused Objects - Number of times an object was reused before despawning normally.\n\n" +
			"Most Objects Active - The most items for this pool active at once.")]
		public bool PrintAllLogsOnQuit;

		[HideInInspector] public Dictionary<GameObject, Pool> PoolRef;

		void Awake()
		{
			CheckDict();
		}

		void CheckDict()
		{
			PoolRef ??= new Dictionary<GameObject, Pool>();
		}

		public bool InitializeSpawn(GameObject obj, float addPool, int minPool, EmptyBehavior emptyBehavior, MaxEmptyBehavior maxEmptyBehavior, bool modBehavior)
		{
			if (obj == null)
			{ return false; }
			CheckDict();
			bool tempModify = false;

			if (PoolRef.ContainsKey(obj) && PoolRef[obj] == null)
			{ // check for broken reference
				PoolRef.Remove(obj); // remove it
			}
			bool result;
			if (PoolRef.ContainsKey(obj))
			{
				result = true; // already have refrence
			}
			else
			{
				if (MakePoolRef(obj) == null)
				{ // ref not found
					if (AllowCreate)
					{
						CreatePool(obj, 0, 0, emptyBehavior, maxEmptyBehavior);
						tempModify = true; // may modify a newly created pool
						result = true;
					}
					else
					{
						result = false;
					}
				}
				else
				{
					result = true; // ref was created
				}
			}

			if (!result || (!AllowModify && !tempModify) || (!(addPool > 0) && minPool <= 0)) return result;
			int size = PoolRef[obj].PoolBlock.Size;
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
			PoolRef[obj].PoolBlock.MaxSize = PoolRef[obj].PoolBlock.Size * 2;
			if (!modBehavior) return true;
			PoolRef[obj].PoolBlock.EmptyBehavior = emptyBehavior;
			PoolRef[obj].PoolBlock.MaxEmptyBehavior = maxEmptyBehavior;

			return result;
		}

		public GameObject Spawn(GameObject obj, int? child, Vector3 pos, Quaternion rot, bool usePosRot, Transform parent)
		{
			if (obj == null)
			{ return null; } // object wasn't defined
			CheckDict();

			if (PoolRef.ContainsKey(obj))
			{
				// reference already created
				if (PoolRef[obj] != null)
				{ // make sure pool still exsists
					return PoolRef[obj].Spawn(child, pos, rot, usePosRot, parent); // create spawn
				}

				// pool no longer exsists
				PoolRef.Remove(obj); // remove reference
				return null;
			}

			// ref not yet created
			Pool childScript = MakePoolRef(obj); // create ref
			return childScript == null ? null : childScript.Spawn(child, pos, rot, usePosRot, parent);
		}

		Pool MakePoolRef(GameObject obj)
		{ // attempt to create and return script reference
			for (int i = 0; i < transform.childCount; i++)
			{
				Pool childScript = transform.GetChild(i).GetComponent<Pool>();
				if (!childScript || obj != childScript.PoolBlock.Prefab) continue;
				PoolRef.Add(obj, childScript);
				return childScript;
			}
			return null;
		}

		public int GetActiveCount(GameObject prefab)
		{
			if (prefab == null)
			{ return 0; } // object wasn't defined

			var childScript = PoolRef.ContainsKey(prefab) ? PoolRef[prefab] : MakePoolRef(prefab);
			if (childScript == null)
			{ // pool not found
				return 0;
			}

			return childScript.PoolBlock.Size - childScript.PoolStack.Count;
		}

		public int GetAvailableCount(GameObject prefab)
		{
			if (prefab == null)
			{ return 0; } // object wasn't defined

			var childScript = PoolRef.ContainsKey(prefab) ? PoolRef[prefab] : MakePoolRef(prefab);
			return childScript == null ? 0 : childScript.PoolStack.Count;
		}

		public bool RemoveAll()
		{
			bool result = true;
			GameObject[] tempObj = new GameObject[PoolRef.Count];
			int i = 0;
			foreach (GameObject obj in PoolRef.Keys)
			{
				if (PoolRef[obj] == null) continue;
				tempObj[i] = obj;
				i++;
			}
			for (int t = 0; t < tempObj.Length; t++)
			{
				if (tempObj[t] != null && !RemovePool(tempObj[t]))
					return false;
			}
			return result;
		}

		public bool DespawnAll()
		{
			bool result = true;
			foreach (GameObject obj in PoolRef.Keys)
			{
				if (!DespawnPool(obj))
				{ result = false; }
			}
			return result;
		}

		public bool RemovePool(GameObject prefab)
		{
			if (prefab == null)
			{ return false; } // object wasn't defined

			Pool childScript;
			if (PoolRef.ContainsKey(prefab))
			{ // reference already created
				childScript = PoolRef[prefab];
			}
			else
			{ // ref not yet created
				childScript = MakePoolRef(prefab); // create ref
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
			if (prefab == null)
			{ return false; } // object wasn't defined
			Pool childScript;
			if (PoolRef.ContainsKey(prefab))
			{ // reference already created
				childScript = PoolRef[prefab];
			}
			else
			{ // ref not yet created
				childScript = MakePoolRef(prefab); // create ref
			}
			if (childScript == null)
			{ // pool not found
				return false;
			}

			for (int i = 0; i < childScript.MasterPool.Count; i++)
			{
				childScript.Despawn(childScript.MasterPool[i].Obj, childScript.MasterPool[i].RefScript);
			}
			return true;
		}

		public void CreatePool()
		{
			CreatePool(null, 32, 64, EmptyBehavior.Grow, MaxEmptyBehavior.Fail);
		}
		public void CreatePool(GameObject prefab, int size, int maxSize, EmptyBehavior emptyBehavior, MaxEmptyBehavior maxEmptyBehavior)
		{
			GameObject obj = new GameObject("Object Pool");
			obj.transform.parent = transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			Pool script = obj.AddComponent<Pool>();
			if (Application.isPlaying)
			{
				obj.name = prefab.name;
				script.PoolBlock.Size = size;
				script.PoolBlock.MaxSize = maxSize;
				script.PoolBlock.EmptyBehavior = emptyBehavior;
				script.PoolBlock.MaxEmptyBehavior = maxEmptyBehavior;
				script.PoolBlock.Prefab = prefab;
				if (prefab)
				{ MakePoolRef(prefab); }
			}
		}

		void OnApplicationQuit()
		{
			if (PrintAllLogsOnQuit)
			{
				PrintAllLogs();
			}
		}

		public void PrintAllLogs()
		{
			foreach (Pool script in PoolRef.Values)
			{
				script.PrintLog();
			}
		}

	}
}
