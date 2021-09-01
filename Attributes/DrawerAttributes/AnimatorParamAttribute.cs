using System;
using UnityEngine;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field)]
  public class AnimatorParamAttribute : DrawerAttribute
  {
    public string AnimatorName { get; }
    public AnimatorControllerParameterType? AnimatorParamType { get; }

    public AnimatorParamAttribute(string animatorName)
    {
      AnimatorName = animatorName;
      AnimatorParamType = null;
    }

    public AnimatorParamAttribute(string animatorName, AnimatorControllerParameterType animatorParamType)
    {
      AnimatorName = animatorName;
      AnimatorParamType = animatorParamType;
    }
  }
}
