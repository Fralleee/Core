#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

namespace Fralle.Core
{
  public abstract class PropertyValidatorBase
  {
    public abstract void ValidateProperty(SerializedProperty property);
  }

  public static class ValidatorAttributeExtensions
  {
    private static Dictionary<Type, PropertyValidatorBase> ValidatorsByAttributeType;

    static ValidatorAttributeExtensions()
    {
      ValidatorsByAttributeType = new Dictionary<Type, PropertyValidatorBase>
      {
        [typeof(MinValueAttribute)] = new MinValuePropertyValidator(),
        [typeof(MaxValueAttribute)] = new MaxValuePropertyValidator(),
        [typeof(RequiredAttribute)] = new RequiredPropertyValidator(),
        [typeof(ValidateInputAttribute)] = new ValidateInputPropertyValidator()
      };
    }

    public static PropertyValidatorBase GetValidator(this ValidatorAttribute attr)
    {
      if (ValidatorsByAttributeType.TryGetValue(attr.GetType(), out var validator))
      {
        return validator;
      }
      else
      {
        return null;
      }
    }
  }
}
#endif
