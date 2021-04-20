using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core.CameraControls
{
	public class ShakeTransform : MonoBehaviour
	{
		[System.Serializable]
		public class ShakeEvent
		{
			readonly float duration;
			float timeRemaining;
			readonly ShakeTransformEventData data;
			Vector3 noiseOffset;
			public Vector3 Noise;

			public ShakeTransformEventData.TargetTransform Target => data.Target;

			public bool IsAlive()
			{
				return timeRemaining > 0.0f;
			}


			public ShakeEvent(ShakeTransformEventData data)
			{
				this.data = data;

				duration = data.Duration;
				timeRemaining = duration;

				const float rand = 32.0f;

				noiseOffset.x = Random.Range(0.0f, rand);
				noiseOffset.y = Random.Range(0.0f, rand);
				noiseOffset.z = Random.Range(0.0f, rand);
			}

			public void Update()
			{
				var deltaTime = Time.deltaTime;

				timeRemaining -= deltaTime;

				var noiseOffsetDelta = deltaTime * data.Frequency;

				noiseOffset.x += noiseOffsetDelta;
				noiseOffset.y += noiseOffsetDelta;
				noiseOffset.z += noiseOffsetDelta;

				Noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
				Noise.y = Mathf.PerlinNoise(noiseOffset.y, 1.0f);
				Noise.z = Mathf.PerlinNoise(noiseOffset.z, 2.0f);

				Noise -= Vector3.one * 0.5f;

				Noise *= data.Amplitude;

				var agePercent = 1.0f - (timeRemaining / duration);
				Noise *= data.BlendOverLifetime.Evaluate(agePercent);
			}
		}

		readonly List<ShakeEvent> shakeEvents = new List<ShakeEvent>();

		public void AddShakeEvent(ShakeTransformEventData data)
		{
			shakeEvents.Add(new ShakeEvent(data));
		}

		public void AddShakeEvent(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime,
			ShakeTransformEventData.TargetTransform target)
		{
			var data = ScriptableObject.CreateInstance<ShakeTransformEventData>();
			data.Init(amplitude, frequency, duration, blendOverLifetime, target);

			AddShakeEvent(data);
		}

		void LateUpdate()
		{
			var positionOffset = Vector3.zero;
			var rotationOffset = Vector3.zero;

			for (var i = shakeEvents.Count - 1; i != -1; i--)
			{
				var se = shakeEvents[i];
				se.Update();

				if (se.Target == ShakeTransformEventData.TargetTransform.Position)
				{
					positionOffset += se.Noise;
				}
				else
				{
					rotationOffset += se.Noise;
				}

				if (!se.IsAlive())
				{
					shakeEvents.RemoveAt(i);
				}
			}

			transform.localPosition = positionOffset;
			transform.localEulerAngles = rotationOffset;
		}
	}
}
