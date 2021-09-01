using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field)]
  public class MinValueAttribute : ValidatorAttribute
  {
    public float MinValue { get; }

    public MinValueAttribute(float minValue)
    {
      MinValue = minValue;
    }

    public MinValueAttribute(int minValue)
    {
      MinValue = minValue;
    }
  }
}
