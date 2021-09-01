using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field)]
  public class MinMaxSliderAttribute : DrawerAttribute
  {
    public float MinValue { get; }
    public float MaxValue { get; }

    public MinMaxSliderAttribute(float minValue, float maxValue)
    {
      MinValue = minValue;
      MaxValue = maxValue;
    }
  }
}
