using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field)]
  public class LabelAttribute : MetaAttribute
  {
    public string Label { get; }

    public LabelAttribute(string label)
    {
      Label = label;
    }
  }
}
