using UnityEngine;

namespace Fralle.Core
{
  public class BitMaskAttribute : PropertyAttribute
  {
    public System.Type PropType;
    public BitMaskAttribute(System.Type aType)
    {
      PropType = aType;
    }
  }
}
