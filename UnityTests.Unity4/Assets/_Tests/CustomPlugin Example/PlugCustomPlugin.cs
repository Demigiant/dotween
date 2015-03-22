using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

public struct PlugCustomPlugin : IPlugSetter<Vector3, Vector3, CustomPlugin, NoOptions>
{
    readonly Vector3 _endValue;
    readonly DOGetter<Vector3> _getter; 
    readonly DOSetter<Vector3> _setter;

    public PlugCustomPlugin(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue)
    {
        _getter = getter;
        _setter = setter;
        _endValue = new Vector3(endValue, 0, 0);
    }

    public DOGetter<Vector3> Getter() { return _getter; }
    public DOSetter<Vector3> Setter() { return _setter; }
    public Vector3 EndValue() { return _endValue; }
    public NoOptions GetOptions() { return new NoOptions(); }
}

// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// ||| CLASS |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

public class CustomPlugin : ABSTweenPlugin<Vector3,Vector3,NoOptions>
{
	public override void Reset(TweenerCore<Vector3, Vector3, NoOptions> t) {}

    public override void SetFrom(TweenerCore<Vector3, Vector3, NoOptions> t, bool isRelative)
    {
        Vector3 prevEndVal = t.endValue;
        t.endValue = t.getter();
        t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
        t.setter(t.startValue);
    }

    public override Vector3 ConvertToStartValue(TweenerCore<Vector3, Vector3, NoOptions> t, Vector3 value)
    {
        return value;
    }

    public override void SetRelativeEndValue(TweenerCore<Vector3, Vector3, NoOptions> t)
    {
        t.endValue = t.startValue + t.changeValue;
    }

    public override void SetChangeValue(TweenerCore<Vector3, Vector3, NoOptions> t)
    {
        t.changeValue = t.endValue - t.startValue;
    }

    public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, Vector3 changeValue)
    {
        float res = changeValue.magnitude / unitsXSecond;
        if (res < 0) res = -res;
        return res;
    }

    public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, DOSetter<Vector3> setter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration, bool usingInversePosition)
    {
        Vector3 res = getter();
        float easeVal = EaseManager.Evaluate(t, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
        res.x = startValue.x + changeValue.x * easeVal;
        setter(res);
    }
}