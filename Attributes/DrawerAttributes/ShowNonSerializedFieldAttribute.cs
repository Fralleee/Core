using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field)]
  public class ShowNonSerializedFieldAttribute : SpecialCaseDrawerAttribute
  {
  }
}
