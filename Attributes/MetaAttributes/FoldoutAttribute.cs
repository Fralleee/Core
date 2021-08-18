using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public class FoldoutAttribute : MetaAttribute, IGroupAttribute
  {
    public string Name { get; private set; }

    public FoldoutAttribute(string name)
    {
      Name = name;
    }
  }
}
