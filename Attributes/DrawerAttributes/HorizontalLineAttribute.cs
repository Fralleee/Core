using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
  public class HorizontalLineAttribute : DrawerAttribute
  {
    public const float DefaultHeight = 2.0f;
    public const EColor DefaultColor = EColor.Gray;

    public float Height { get; }
    public EColor Color { get; }

    public HorizontalLineAttribute(float height = DefaultHeight, EColor color = DefaultColor)
    {
      Height = height;
      Color = color;
    }
  }
}
