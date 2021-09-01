using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
  public class ValueChangedAttribute : MetaAttribute
  {
    public string CallbackName { get; }

    public ValueChangedAttribute(string callbackName)
    {
      CallbackName = callbackName;
    }
  }
}
