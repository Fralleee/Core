using UnityEngine;

namespace Fralle.Core.Pooling
{
	[System.Serializable]
	public class PoolItem
	{
		public GameObject Obj;
		public PooledObject RefScript;

		public PoolItem(GameObject obj, PooledObject refScript)
		{
			this.Obj = obj;
			this.RefScript = refScript;
		}
	}
}
