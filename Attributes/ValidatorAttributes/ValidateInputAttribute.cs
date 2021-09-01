using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field)]
  public class ValidateInputAttribute : ValidatorAttribute
  {
    public string CallbackName { get; }
    public string Message { get; }

    public ValidateInputAttribute(string callbackName, string message = null)
    {
      CallbackName = callbackName;
      Message = message;
    }
  }
}
