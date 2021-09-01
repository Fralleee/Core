using Fralle.Core.CameraControls;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core
{
  public class ShakeTransformer : LocalTransformer, IRotator, IPositioner
  {
    readonly List<ShakeEvent> shakeEvents = new List<ShakeEvent>();

    Vector3 currentPosition = Vector3.zero;
    Quaternion currentRotation = Quaternion.identity;

    public Vector3 GetPosition() => currentPosition;
    public Quaternion GetRotation() => currentRotation;
    public override void Calculate()
    {
      Vector3 positionOffset = Vector3.zero;
      Vector3 rotationOffset = Vector3.zero;

      for (int i = shakeEvents.Count - 1; i != -1; i--)
      {
        ShakeEvent shakeEvent = shakeEvents[i];
        shakeEvent.Update();

        if (shakeEvent.Target == ShakeTransformEventData.TargetTransform.Position)
          positionOffset += shakeEvent.noise;
        else
          rotationOffset += shakeEvent.noise;

        if (!shakeEvent.IsAlive())
          shakeEvents.RemoveAt(i);
      }

      currentPosition = positionOffset;
      currentRotation = Quaternion.Euler(rotationOffset);
    }

    public void AddShakeEvent(ShakeTransformEventData data)
    {
      shakeEvents.Add(new ShakeEvent(data));
    }

    public void AddShakeEvent(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime,
      ShakeTransformEventData.TargetTransform target)
    {
      ShakeTransformEventData data = ScriptableObject.CreateInstance<ShakeTransformEventData>();
      data.Init(amplitude, frequency, duration, blendOverLifetime, target);

      AddShakeEvent(data);
    }
  }
}
