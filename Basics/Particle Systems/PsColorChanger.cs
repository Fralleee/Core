using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.Core.Basics
{
  [DisallowMultipleComponent]
  public class PsColorChanger : MonoBehaviour
  {
    #region Fields
    [FormerlySerializedAs("CurrentColor")] [Tooltip("Current \"Main\" color of all particle systems")]
    public Color currentColor;
    [FormerlySerializedAs("NewColor")] [Tooltip("New \"Main\" color of all particle systems")]
    public Color newColor;

    Color currentHsv; // r -> H; g -> S; b -> V (not a really correct way to do it :D)
    Color newHsv; // r -> H; g -> S; b -> V (not a really correct way to do it :D)
    #endregion

    #region Methods
    /**
				Update colors of all child systems
		*/
    public void ChangeColor()
    {
      ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();

      Color.RGBToHSV(currentColor, out currentHsv.r, out currentHsv.g, out currentHsv.b);
      Color.RGBToHSV(newColor, out newHsv.r, out newHsv.g, out newHsv.b);

      foreach (ParticleSystem system in systems)
      {
        ParticleSystem.MainModule main = system.main;
        main.startColor = main.startColor.mode switch
        {
          ParticleSystemGradientMode.Color => new ParticleSystem.MinMaxGradient(
            ConvertCurrentToNew(main.startColor.color)),
          ParticleSystemGradientMode.TwoColors => new ParticleSystem.MinMaxGradient(
            ConvertCurrentToNew(main.startColor.colorMin), ConvertCurrentToNew(main.startColor.colorMax)),
          ParticleSystemGradientMode.Gradient => new ParticleSystem.MinMaxGradient(
            ConvertCurrentToNew(main.startColor.gradient)),
          ParticleSystemGradientMode.TwoGradients => new ParticleSystem.MinMaxGradient(
            ConvertCurrentToNew(main.startColor.gradientMin),
            ConvertCurrentToNew(main.startColor.gradientMax)),
          _ => main.startColor
        };
      }
    }

    public void SwapCurrentWithNewColors()
    {
      Color temp = currentColor;
      currentColor = newColor;
      newColor = temp;
    }

    public Gradient ConvertCurrentToNew(Gradient gradient)
    {
      Gradient g = new Gradient { mode = gradient.mode };

      GradientAlphaKey[] alphaKeys = new GradientAlphaKey[gradient.alphaKeys.Length];
      GradientColorKey[] colorKeys = new GradientColorKey[gradient.colorKeys.Length];

      for (int i = 0; i < g.colorKeys.Length; ++i)
        colorKeys[i] = new GradientColorKey(
                ConvertCurrentToNew(gradient.colorKeys[i].color),
                gradient.colorKeys[i].time
            );

      System.Array.Copy(gradient.alphaKeys, alphaKeys, alphaKeys.Length);

      g.SetKeys(colorKeys, alphaKeys);
      return g;
    }

    public Color ConvertCurrentToNew(Color color)
    {
      Color hsv;
      Color.RGBToHSV(color, out hsv.r, out hsv.g, out hsv.b);
      Color endRes = Color.HSVToRGB(
              Mathf.Clamp01(Mathf.Abs(newHsv.r + (currentHsv.r - hsv.r))),
              hsv.g,
              hsv.b
          );
      endRes.a = color.a;
      return endRes;
    }
    #endregion
  }
}
