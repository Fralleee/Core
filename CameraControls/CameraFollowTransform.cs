using UnityEngine;

namespace Fralle.Core.CameraControls
{
	public class CameraFollowTransform : MonoBehaviour
	{

		public Transform TransformToFollow;

		void LateUpdate()
		{
			transform.position = TransformToFollow.position;
		}
	}
}
