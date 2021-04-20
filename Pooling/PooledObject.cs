using UnityEngine;

namespace Fralle.Core.Pooling
{
	public class PooledObject : MonoBehaviour
	{
		[HideInInspector] public Pool PoolScript; // stores the location of the object pool script for this object
		[HideInInspector] public float TimeSpawned;

		public bool Despawn(float del)
		{ // -1 will use delay specified in this script
			if (del >= 0)
			{
				if (PoolScript)
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
			if (PoolScript)
			{
				PoolScript.Despawn(gameObject, this);
				return true;
			}
			return false;
		}
	}
}
