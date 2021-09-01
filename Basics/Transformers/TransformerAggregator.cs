using System.Linq;
using UnityEngine;

namespace Fralle.Core
{
  public class TransformerAggregator : MonoBehaviour
  {
    MasterPositioner masterPositioner;
    MasterRotator masterRotator;

    LocalTransformer[] localTransformers;

    IRotator[] rotators;
    IPositioner[] positioners;

    Vector3 initPosition;
    Quaternion initRotation;

    Transform aggTransform;

    void Awake()
    {
      aggTransform = transform;

      initPosition = aggTransform.localPosition;
      initRotation = aggTransform.localRotation;

      masterPositioner = GetComponent<MasterPositioner>();
      masterRotator = GetComponent<MasterRotator>();
      localTransformers = GetComponents<LocalTransformer>();

      rotators = localTransformers.OfType<IRotator>().ToArray();
      positioners = localTransformers.OfType<IPositioner>().ToArray();
    }

    void Update()
    {
      Calculate();
    }

    void Calculate()
    {
      Vector3 combinedPosition = Vector3.zero;
      Quaternion combinedRotation = Quaternion.identity;

      foreach (LocalTransformer localTransformer in localTransformers)
        localTransformer.Calculate();

      foreach (IPositioner rotator in positioners)
        combinedPosition += rotator.GetPosition();

      foreach (IRotator rotator in rotators)
        combinedRotation *= rotator.GetRotation();

      if (masterPositioner)
      {
        aggTransform.position = masterPositioner.GetPosition();
        aggTransform.localPosition += combinedPosition;
      }
      else
        aggTransform.localPosition = initPosition + combinedPosition;

      if (masterRotator)
      {
        aggTransform.rotation = masterRotator.GetRotation();
        aggTransform.localRotation *= combinedRotation;
      }
      else
        aggTransform.localRotation = initRotation * combinedRotation;
    }
  }
}
