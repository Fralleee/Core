using UnityEngine;
using static Fralle.Core.EasingUtility;

namespace Fralle.Core
{
  public static class Vector3Easings
  {
    public static Vector3 GetComputedVector3(Function easingFunction, Vector3 start, Vector3 end, float alpha)
        => new Vector3(
            easingFunction(start.x, end.x, alpha),
            easingFunction(start.y, end.y, alpha),
            easingFunction(start.z, end.z, alpha));

    public static Vector3 Linear(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.Linear), start, end, alpha);
    public static Vector3 Spring(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.Spring), start, end, alpha);
    public static Vector3 EaseInQuad(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInQuad), start, end, alpha);
    public static Vector3 EaseOutQuad(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseOutQuad), start, end, alpha);
    public static Vector3 EaseInOutQuad(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInOutQuad), start, end, alpha);
    public static Vector3 EaseInCubic(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInCubic), start, end, alpha);
    public static Vector3 EaseOutCubic(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseOutCubic), start, end, alpha);
    public static Vector3 EaseInOutCubic(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInOutCubic), start, end, alpha);
    public static Vector3 EaseInQuart(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInQuart), start, end, alpha);
    public static Vector3 EaseOutQuart(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseOutQuart), start, end, alpha);
    public static Vector3 EaseInOutQuart(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInOutQuart), start, end, alpha);
    public static Vector3 EaseInQuint(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInQuint), start, end, alpha);
    public static Vector3 EaseOutQuint(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseOutQuint), start, end, alpha);
    public static Vector3 EaseInOutQuint(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInOutQuint), start, end, alpha);
    public static Vector3 EaseInSine(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInSine), start, end, alpha);
    public static Vector3 EaseOutSine(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseOutSine), start, end, alpha);
    public static Vector3 EaseInOutSine(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInOutSine), start, end, alpha);
    public static Vector3 EaseInExpo(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInExpo), start, end, alpha);
    public static Vector3 EaseOutExpo(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseOutExpo), start, end, alpha);
    public static Vector3 EaseInOutExpo(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInOutExpo), start, end, alpha);
    public static Vector3 EaseInCirc(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInCirc), start, end, alpha);
    public static Vector3 EaseOutCirc(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseOutCirc), start, end, alpha);
    public static Vector3 EaseInOutCirc(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInOutCirc), start, end, alpha);
    public static Vector3 EaseInBounce(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInBounce), start, end, alpha);
    public static Vector3 EaseOutBounce(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseOutBounce), start, end, alpha);
    public static Vector3 EaseInOutBounce(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInOutBounce), start, end, alpha);
    public static Vector3 EaseInBack(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInBack), start, end, alpha);
    public static Vector3 EaseOutBack(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseOutBack), start, end, alpha);
    public static Vector3 EaseInOutBack(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInOutBack), start, end, alpha);
    public static Vector3 EaseInElastic(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInElastic), start, end, alpha);
    public static Vector3 EaseOutElastic(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseOutElastic), start, end, alpha);
    public static Vector3 EaseInOutElastic(Vector3 start, Vector3 end, float alpha) => GetComputedVector3(GetEasingFunction(Ease.EaseInOutElastic), start, end, alpha);
  }
}
