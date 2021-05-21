using UnityEngine;

namespace Fralle.PingTap
{
	public abstract class Transformer : MonoBehaviour
	{
		public bool master;
		public abstract void Calculate();
		public abstract Vector3 GetPosition();
		public abstract Quaternion GetRotation();
	}
}
