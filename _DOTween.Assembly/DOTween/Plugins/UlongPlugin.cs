// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/03/29 23:01

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
    public class UlongPlugin : ABSTweenPlugin<ulong, ulong, NoOptions>
    {
        public override void Reset(TweenerCore<ulong, ulong, NoOptions> t) { }

        public override void SetFrom(TweenerCore<ulong, ulong, NoOptions> t, bool isRelative)
        {
            ulong prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
            t.setter(t.startValue);
        }

        public override ulong ConvertToStartValue(TweenerCore<ulong, ulong, NoOptions> t, ulong value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<ulong, ulong, NoOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<ulong, ulong, NoOptions> t)
        {
            t.changeValue = t.endValue - t.startValue;
        }

        public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, ulong changeValue)
        {
            float res = changeValue / unitsXSecond;
            if (res < 0) res = -res;
            return res;
        }

        public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<ulong> getter, DOSetter<ulong> setter, float elapsed, ulong startValue, ulong changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (uint)(t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue += changeValue * (uint)(t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (uint)(t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            // Converts ease value to decimal, otherwise final ulong conversion will not be correct
            setter((ulong)(startValue + changeValue * (decimal)EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)));
        } 
    }
}