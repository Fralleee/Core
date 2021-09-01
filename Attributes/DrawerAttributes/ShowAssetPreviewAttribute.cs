using System;

namespace Fralle.Core
{
  [AttributeUsage(AttributeTargets.Field)]
  public class ShowAssetPreviewAttribute : DrawerAttribute
  {
    public const int DefaultWidth = 64;
    public const int DefaultHeight = 64;

    public int Width { get; }
    public int Height { get; }

    public ShowAssetPreviewAttribute(int width = DefaultWidth, int height = DefaultHeight)
    {
      Width = width;
      Height = height;
    }
  }
}
