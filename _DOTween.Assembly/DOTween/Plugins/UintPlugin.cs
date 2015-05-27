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

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    public class UintPlugin : ABSTweenPlugin<uint, uint, NoOptions>
    {
        public override void Reset(TweenerCore<uint, uint, NoOptions> t) { }

        public override void SetFrom(TweenerCore<uint, uint, NoOptions> t, bool isRelative)
        {
            uint prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
            t.setter(t.startValue);
        }

        public override uint ConvertToStartValue(TweenerCore<uint, uint, NoOptions> t, uint value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<uint, uint, NoOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<uint, uint, NoOptions> t)
        {
            t.changeValue = t.endValue - t.startValue;
        }

        public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, uint changeValue)
        {
            float res = changeValue / unitsXSecond;
            if (res < 0) res = -res;
            return res;
        }

        public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<uint> getter, DOSetter<uint> setter, float elapsed, uint startValue, uint changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            if (t.loopType == LoopType.Incremental) startValue += (uint)(changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops));
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue += (uint)(changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops));
            }

            setter((uint)Math.Round(startValue + changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)));
        }
    }
}