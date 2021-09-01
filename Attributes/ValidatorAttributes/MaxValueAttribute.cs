using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field)]
  public class MaxValueAttribute : ValidatorAttribute
  {
    public float MaxValue { get; }

    public MaxValueAttribute(float maxValue)
    {
      MaxValue = maxValue;
    }

    public MaxValueAttribute(int maxValue)
    {
      MaxValue = maxValue;
    }
  }
}
