using System.Collections.Generic;
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
				if (pool.PoolBlock.Prefab == null)
					continue;
				else if (PoolRef.ContainsKey(pool.PoolBlock.Prefab))
					Debug.LogWarning($"Already found pool for {pool.PoolBlock.Prefab}.");
				else
					PoolRef.Add(pool.PoolBlock.Prefab, pool);
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
			bool result;
			if (PoolRef.ContainsKey(obj))
				result = true; // already have refrence
			else
				result = false;

			if (!result || (!(addPool > 0) && minPool <= 0))
				return result;
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
			if (!modBehavior)
				return true;
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

			return childScript.PoolBlock.Size - childScript.PoolStack.Count;
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
			bool result = true;
			GameObject[] tempObj = new GameObject[PoolRef.Count];
			int i = 0;
			foreach (GameObject obj in PoolRef.Keys)
			{
				if (PoolRef[obj] == null)
					continue;
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

			for (int i = 0; i < childScript.MasterPool.Count; i++)
			{
				childScript.Despawn(childScript.MasterPool[i].Obj, childScript.MasterPool[i].RefScript);
			}
			return true;
		}

	}
}
