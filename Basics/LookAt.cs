using UnityEngine;

namespace Fralle.Core.Basics
{
	public class LookAt : MonoBehaviour
	{
		[SerializeField] private Transform transformToLookAt;

		void Update()
		{
			transform.LookAt(transformToLookAt.position);
		}
	}
}
