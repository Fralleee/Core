using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class ShowNativePropertyAttribute : SpecialCaseDrawerAttribute
  {
  }
}
