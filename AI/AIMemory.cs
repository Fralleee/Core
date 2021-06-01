using UnityEngine;

namespace Fralle.Core
{
	public class AIMemory
	{
		public float Age => Time.time - lastSeen;
		public GameObject gameObject;
		public Vector3 position;
		public Vector3 direction;
		public float distance;
		public float angle;
		public float lastSeen;
		public float score;
	}
}
