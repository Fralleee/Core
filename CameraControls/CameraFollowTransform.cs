using Fralle.PingTap;
using UnityEngine;

namespace Fralle.Core.CameraControls
{
	public class CameraFollowTransform : Transformer
	{
		public Transform TransformToFollow;

		public override void Calculate() { }
		public override Vector3 GetPosition() => TransformToFollow.position;
		public override Quaternion GetRotation() => Quaternion.identity;
	}
}
