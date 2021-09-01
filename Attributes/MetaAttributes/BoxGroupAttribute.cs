using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field)]
  public class BoxGroupAttribute : MetaAttribute, IGroupAttribute
  {
    public string Name { get; }

    public BoxGroupAttribute(string name = "")
    {
      Name = name;
    }
  }
}
