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

    void Awake()
    {
      initPosition = transform.localPosition;
      initRotation = transform.localRotation;

      masterPositioner = GetComponent<MasterPositioner>();
      masterRotator = GetComponent<MasterRotator>();
      localTransformers = GetComponents<LocalTransformer>();

      rotators = localTransformers.Where(x => x is IRotator).Select(x => (IRotator)x).ToArray();
      positioners = localTransformers.Where(x => x is IPositioner).Select(x => (IPositioner)x).ToArray();
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
        transform.position = masterPositioner.GetPosition();
        transform.localPosition += combinedPosition;
      }
      else
        transform.localPosition = initPosition + combinedPosition;

      if (masterRotator)
      {
        transform.rotation = masterRotator.GetRotation();
        transform.localRotation *= combinedRotation;
      }
      else
        transform.localRotation = initRotation * combinedRotation;
    }
  }
}
