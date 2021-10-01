using Fralle.Core.CameraControls;
using System.Collections.Generic;
using UnityEngine;

namespace Fralle.Core
{
  public static class ProceduralMotion
  {
    static Vector3 headbobRotationAxis = new Vector3(0, 1, 0);
    public static (Vector3, Quaternion) Recoil(Vector3 currentRecoil, Quaternion currentRotation, float speed, float recoverTime)
    {
      Quaternion toRotation = Quaternion.Euler(currentRecoil.y, currentRecoil.x, currentRecoil.z);
      currentRotation = Quaternion.RotateTowards(currentRotation, toRotation, speed * Time.deltaTime);
      currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, recoverTime * Time.deltaTime);
      return (currentRecoil, currentRotation);
    }

    public static (Vector3, Quaternion) CameraShake(List<ShakeEvent> shakeEvents)
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

      return (positionOffset, Quaternion.Euler(rotationOffset));
    }

    public static (Vector3, float) Headbob(float bobAmount, float bobSpeed, float timer)
    {
      timer += bobSpeed * Time.deltaTime;
      if (timer > Mathf.PI * 2)
        timer -= Mathf.PI * 2;

      return (new Vector3(0f, bobAmount, 0f), timer);
    }

    public static (Vector3, Quaternion) Headbob(float bobAmount, float angleChanges)
    {
      return (new Vector3(0f, bobAmount, 0f), Quaternion.AngleAxis(angleChanges, headbobRotationAxis));
    }

    public static Vector3 ResetHeadbob(Vector3 position, float smoothSpeed) => Vector3.Lerp(position, Vector3.zero, Time.deltaTime * smoothSpeed);

    public static (Vector3, Quaternion) ResetHeadbob(Vector3 position, Quaternion rotation, float smoothSpeed)
    {
      return (Vector3.Lerp(position, Vector3.zero, Time.deltaTime * smoothSpeed), Quaternion.Lerp(rotation, Quaternion.identity, Time.deltaTime * smoothSpeed));
    }
  }
}
