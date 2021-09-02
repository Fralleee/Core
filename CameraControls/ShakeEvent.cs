using Fralle.Core.CameraControls;
using UnityEngine;

namespace Fralle.Core
{
  [System.Serializable]
  public class ShakeEvent
  {
    readonly float duration;
    float timeRemaining;
    readonly ShakeTransformEventData data;
    Vector3 noiseOffset;
    public Vector3 noise;

    public ShakeTransformEventData.TargetTransform Target => data.target;

    public bool IsAlive()
    {
      return timeRemaining > 0.0f;
    }

    public ShakeEvent(ShakeTransformEventData data)
    {
      this.data = data;

      duration = data.duration;
      timeRemaining = duration;

      const float rand = 32.0f;

      noiseOffset.x = Random.Range(0.0f, rand);
      noiseOffset.y = Random.Range(0.0f, rand);
      noiseOffset.z = Random.Range(0.0f, rand);
    }

    public void Update()
    {
      float deltaTime = Time.deltaTime;

      timeRemaining -= deltaTime;

      float noiseOffsetDelta = deltaTime * data.frequency;

      noiseOffset.x += noiseOffsetDelta;
      noiseOffset.y += noiseOffsetDelta;
      noiseOffset.z += noiseOffsetDelta;

      noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
      noise.y = Mathf.PerlinNoise(noiseOffset.y, 1.0f);
      noise.z = Mathf.PerlinNoise(noiseOffset.z, 2.0f);

      noise -= Vector3.one * 0.5f;

      noise *= data.amplitude;

      float agePercent = 1.0f - (timeRemaining / duration);
      noise *= data.blendOverLifetime.Evaluate(agePercent);
    }
  }
}
