// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/11 13:04
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
    // BEWARE: RectOffset seems a struct but is a class
    // USING THIS PLUGIN WILL GENERATE GC ALLOCATIONS
    public class RectOffsetPlugin : ABSTweenPlugin<RectOffset, RectOffset, NoOptions>
    {
        static RectOffset _r = new RectOffset(); // Used to store incremental values without creating a new RectOffset each time

        public override void Reset(TweenerCore<RectOffset, RectOffset, NoOptions> t)
        {
            t.startValue = t.endValue = t.changeValue = null;
        }

        public override void SetFrom(TweenerCore<RectOffset, RectOffset, NoOptions> t, bool isRelative)
        {
            RectOffset prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = prevEndVal;
            if (isRelative) {
                t.startValue.left += t.endValue.left;
                t.startValue.right += t.endValue.right;
                t.startValue.top += t.endValue.top;
                t.startValue.bottom += t.endValue.bottom;
            }
            t.setter(t.startValue);
        }

        public override RectOffset ConvertToStartValue(TweenerCore<RectOffset, RectOffset, NoOptions> t, RectOffset value)
        {
            return new RectOffset(value.left, value.right, value.top, value.bottom);
        }

        public override void SetRelativeEndValue(TweenerCore<RectOffset, RectOffset, NoOptions> t)
        {
            t.endValue.left += t.startValue.left;
            t.endValue.right += t.startValue.right;
            t.endValue.top += t.startValue.top;
            t.endValue.bottom += t.startValue.bottom;
        }

        public override void SetChangeValue(TweenerCore<RectOffset, RectOffset, NoOptions> t)
        {
            t.changeValue = new RectOffset(
                t.endValue.left - t.startValue.left,
                t.endValue.right - t.startValue.right,
                t.endValue.top - t.startValue.top,
                t.endValue.bottom - t.startValue.bottom
            );
        }

        public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, RectOffset changeValue)
        {
            // Uses length of diagonal to calculate units.
            float diffW = changeValue.right;
            if (diffW < 0) diffW = -diffW;
            float diffH = changeValue.bottom;
            if (diffH < 0) diffH = -diffH;
            float diag = (float)Math.Sqrt(diffW * diffW + diffH * diffH);
            return diag / unitsXSecond;
        }

        public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<RectOffset> getter, DOSetter<RectOffset> setter, float elapsed, RectOffset startValue, RectOffset changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            _r.left = startValue.left;
            _r.right = startValue.right;
            _r.top = startValue.top;
            _r.bottom = startValue.bottom;

            if (t.loopType == LoopType.Incremental) {
                int iterations = t.isComplete ? t.completedLoops - 1 : t.completedLoops;
                _r.left += changeValue.left * iterations;
                _r.right += changeValue.right * iterations;
                _r.top += changeValue.top * iterations;
                _r.bottom += changeValue.bottom * iterations;
            }
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                int iterations = (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
                _r.left += changeValue.left * iterations;
                _r.right += changeValue.right * iterations;
                _r.top += changeValue.top * iterations;
                _r.bottom += changeValue.bottom * iterations;
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            setter(
                new RectOffset(
                    (int)Math.Round(_r.left + changeValue.left * easeVal),
                    (int)Math.Round(_r.right + changeValue.right * easeVal),
                    (int)Math.Round(_r.top + changeValue.top * easeVal),
                    (int)Math.Round(_r.bottom + changeValue.bottom * easeVal)
                )
            );
        }
    }
}