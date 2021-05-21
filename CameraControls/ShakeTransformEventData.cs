using UnityEngine;

namespace Fralle.Core.CameraControls
{
	[CreateAssetMenu(fileName = "Shake Transform Event", menuName = "Camera/Shake Transform Event", order = 1)]
	public class ShakeTransformEventData : ScriptableObject
	{
		public enum TargetTransform
		{
			Position,
			Rotation
		}

		public TargetTransform Target = TargetTransform.Position;

		public float Amplitude = 1.0f;
		public float Frequency = 1.0f;

		public float Duration = 1.0f;

		public AnimationCurve BlendOverLifetime = new AnimationCurve(

			new Keyframe(0.0f, 0.0f, Mathf.Deg2Rad * 0.0f, Mathf.Deg2Rad * 720.0f),
			new Keyframe(0.2f, 1.0f),
			new Keyframe(1.0f, 0.0f));

		public void Init(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime, TargetTransform target)
		{
			this.Target = target;

			this.Amplitude = amplitude;
			this.Frequency = frequency;

			this.Duration = duration;

			this.BlendOverLifetime = blendOverLifetime;
		}
	}
}
