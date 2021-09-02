using System;
using UnityEngine;
// ReSharper disable UnusedMember.Global

namespace Fralle.Core
{
  public static class FloatExtensions
  {
    public static float Floor(this float value) => Mathf.Floor(value);

    public static float Round(this float value) => Mathf.Round(value);

    public static float Ceil(this float value) => Mathf.Ceil(value);

    public static bool EqualsWithTolerance(this float value, float otherValue, float tolerance = 0.000001f) => Math.Abs(value - otherValue) < tolerance;
  }
}
