using UnityEngine;

namespace Fralle.Core.CameraControls
{
	public class PositionInFront : MonoBehaviour
	{
		[SerializeField] Transform transformToFollow;
		[SerializeField] float offset;

		void LateUpdate()
		{
			transform.position = transformToFollow.position + transformToFollow.forward * offset;
		}
	}
}
