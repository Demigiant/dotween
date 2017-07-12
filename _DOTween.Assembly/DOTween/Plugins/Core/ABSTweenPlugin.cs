// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/07 00:41
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core
{
    // Public so it can be extended by custom plugins
    public abstract class ABSTweenPlugin<T1,T2,TPlugOptions> : ITweenPlugin where TPlugOptions : struct, IPlugOptions
    {
        public abstract void Reset(TweenerCore<T1, T2, TPlugOptions> t); // Resets specific TweenerCore stuff, not the plugin itself
        public abstract void SetFrom(TweenerCore<T1, T2, TPlugOptions> t, bool isRelative);
        public abstract T2 ConvertToStartValue(TweenerCore<T1, T2, TPlugOptions> t, T1 value);
        public abstract void SetRelativeEndValue(TweenerCore<T1, T2, TPlugOptions> t);
        public abstract void SetChangeValue(TweenerCore<T1, T2, TPlugOptions> t);
        public abstract float GetSpeedBasedDuration(TPlugOptions options, float unitsXSecond, T2 changeValue);
        // usingInversePosition is used by PathPlugin to calculate correctly the current waypoint reached
        public abstract void EvaluateAndApply(TPlugOptions options, Tween t, bool isRelative, DOGetter<T1> getter, DOSetter<T1> setter, float elapsed, T2 startValue, T2 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice);
    }
}