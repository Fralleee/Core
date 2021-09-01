using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field)]
  public class RequiredAttribute : ValidatorAttribute
  {
    public string Message { get; }

    public RequiredAttribute(string message = null)
    {
      Message = message;
    }
  }
}
