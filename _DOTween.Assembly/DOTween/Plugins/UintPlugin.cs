// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/10 19:24
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    public class UintPlugin : ABSTweenPlugin<uint, uint, UintOptions>
    {
        public override void Reset(TweenerCore<uint, uint, UintOptions> t) { }

        public override void SetFrom(TweenerCore<uint, uint, UintOptions> t, bool isRelative)
        {
            uint prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
            t.setter(t.startValue);
        }

        public override uint ConvertToStartValue(TweenerCore<uint, uint, UintOptions> t, uint value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<uint, uint, UintOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<uint, uint, UintOptions> t)
        {
            t.plugOptions.isNegativeChangeValue = t.endValue < t.startValue;
            t.changeValue = t.plugOptions.isNegativeChangeValue ? t.startValue - t.endValue : t.endValue - t.startValue;
//            t.changeValue = t.endValue - t.startValue;
        }

        public override float GetSpeedBasedDuration(UintOptions options, float unitsXSecond, uint changeValue)
        {
            float res = changeValue / unitsXSecond;
            if (res < 0) res = -res;
            return res;
        }

        public override void EvaluateAndApply(UintOptions options, Tween t, bool isRelative, DOGetter<uint> getter, DOSetter<uint> setter, float elapsed, uint startValue, uint changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            uint v;
            if (t.loopType == LoopType.Incremental) {
                v =  (uint)(changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops));
                if (options.isNegativeChangeValue) startValue -= v;
                else startValue += v;
            }
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                v = (uint)(changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops));
                if (options.isNegativeChangeValue) startValue -= v;
                else startValue += v;
            }

            v = (uint)Math.Round(changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod));
            if (options.isNegativeChangeValue) setter(startValue - v);
            else setter(startValue + v);
        }
    }
}