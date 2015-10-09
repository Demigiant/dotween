using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

/// <summary>
/// Custom DOTween plugin example.
/// This one tweens a CustomRange value, but you can also create plugins just to do weird stuff, other than to tween custom objects
/// </summary>
public class CustomRangePlugin : ABSTweenPlugin<CustomRange, CustomRange, NoOptions>
{
    // Leave this empty
	public override void Reset(TweenerCore<CustomRange, CustomRange, NoOptions> t) {}

    // Sets the values in case of a From tween
    public override void SetFrom(TweenerCore<CustomRange, CustomRange, NoOptions> t, bool isRelative)
    {
        CustomRange prevEndVal = t.endValue;
        t.endValue = t.getter();
        t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
        t.setter(t.startValue);
    }

    // Used by special plugins, just let it return the given value
    public override CustomRange ConvertToStartValue(TweenerCore<CustomRange, CustomRange, NoOptions> t, CustomRange value)
    {
        return value;
    }

    // Determines the correct endValue in case this is a relative tween
    public override void SetRelativeEndValue(TweenerCore<CustomRange, CustomRange, NoOptions> t)
    {
        t.endValue = t.startValue + t.changeValue;
    }

    // Sets the overall change value of the tween
    public override void SetChangeValue(TweenerCore<CustomRange, CustomRange, NoOptions> t)
    {
        t.changeValue = t.endValue - t.startValue;
    }

    // Converts a regular duration to a speed-based duration
    public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, CustomRange changeValue)
    {
        // Not implemented in this case (but you could implement your own logic to convert duration to units x second)
        return unitsXSecond;
    }

    // Calculates the value based on the given time and ease
    public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<CustomRange> getter, DOSetter<CustomRange> setter, float elapsed, CustomRange startValue, CustomRange changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
    {
        CustomRange res = getter();
        float easeVal = EaseManager.Evaluate(t, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);

        // Here I use startValue directly because CustomRange a struct, so it won't reference the original.
        // If CustomRange was a class, I should create a new one to pass to the setter
        startValue.min += changeValue.min * easeVal;
        startValue.max += changeValue.max * easeVal;
        setter(startValue);
    }
}