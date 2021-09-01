using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field)]
  public class FoldoutAttribute : MetaAttribute, IGroupAttribute
  {
    public string Name { get; }

    public FoldoutAttribute(string name)
    {
      Name = name;
    }
  }
}
