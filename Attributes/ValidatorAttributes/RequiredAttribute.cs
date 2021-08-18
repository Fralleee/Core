﻿using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public class RequiredAttribute : ValidatorAttribute
  {
    public string Message { get; private set; }

    public RequiredAttribute(string message = null)
    {
      Message = message;
    }
  }
}
