using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
  public class ValueChangedAttribute : MetaAttribute
  {
    public string CallbackName { get; private set; }

    public ValueChangedAttribute(string callbackName)
    {
      CallbackName = callbackName;
    }
  }
}
