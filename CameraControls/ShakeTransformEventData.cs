using UnityEngine;
using UnityEngine.Serialization;

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

    [FormerlySerializedAs("Target")] public TargetTransform target = TargetTransform.Position;

    [FormerlySerializedAs("Amplitude")] public float amplitude = 1.0f;
    [FormerlySerializedAs("Frequency")] public float frequency = 1.0f;

    [FormerlySerializedAs("Duration")] public float duration = 1.0f;

    [FormerlySerializedAs("BlendOverLifetime")] public AnimationCurve blendOverLifetime = new AnimationCurve(

      new Keyframe(0.0f, 0.0f, Mathf.Deg2Rad * 0.0f, Mathf.Deg2Rad * 720.0f),
      new Keyframe(0.2f, 1.0f),
      new Keyframe(1.0f, 0.0f));

    public void Init(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime, TargetTransform target)
    {
      this.target = target;

      this.amplitude = amplitude;
      this.frequency = frequency;

      this.duration = duration;

      this.blendOverLifetime = blendOverLifetime;
    }
  }
}
