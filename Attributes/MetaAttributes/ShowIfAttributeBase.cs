using System;

namespace Fralle.Core
{
  public class ShowIfAttributeBase : MetaAttribute
  {
    public string[] Conditions { get; }
    public EConditionOperator ConditionOperator { get; }
    public bool Inverted { get; protected set; }

    /// <summary>
    ///		If this not null, <see cref="Conditions"/>[0] is name of an enum variable.
    /// </summary>
    public Enum EnumValue { get; }

    public ShowIfAttributeBase(string condition)
    {
      ConditionOperator = EConditionOperator.And;
      Conditions = new[] { condition };
    }

    public ShowIfAttributeBase(EConditionOperator conditionOperator, params string[] conditions)
    {
      ConditionOperator = conditionOperator;
      Conditions = conditions;
    }

    public ShowIfAttributeBase(string enumName, Enum enumValue)
      : this(enumName)
    {
      EnumValue = enumValue ?? throw new ArgumentNullException(nameof(enumValue), "This parameter must be an enum value.");
    }
  }
}
