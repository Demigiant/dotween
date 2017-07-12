#if !COMPATIBLE
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/10 16:51
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
    public class Vector2Plugin : ABSTweenPlugin<Vector2, Vector2, VectorOptions>
    {
        public override void Reset(TweenerCore<Vector2, Vector2, VectorOptions> t) { }

        public override void SetFrom(TweenerCore<Vector2, Vector2, VectorOptions> t, bool isRelative)
        {
            Vector2 prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
            Vector2 to = t.endValue;
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                to.x = t.startValue.x;
                break;
            case AxisConstraint.Y:
                to.y = t.startValue.y;
                break;
            default:
                to = t.startValue;
                break;
            }
            if (t.plugOptions.snapping) {
                to.x = (float)Math.Round(to.x);
                to.y = (float)Math.Round(to.y);
            }
            t.setter(to);
        }

        public override Vector2 ConvertToStartValue(TweenerCore<Vector2, Vector2, VectorOptions> t, Vector2 value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector2, Vector2, VectorOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Vector2, Vector2, VectorOptions> t)
        {
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                t.changeValue = new Vector2(t.endValue.x - t.startValue.x, 0);
                break;
            case AxisConstraint.Y:
                t.changeValue = new Vector2(0, t.endValue.y - t.startValue.y);
                break;
            default:
                t.changeValue = t.endValue - t.startValue;
                break;
            }
        }

        public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector2 changeValue)
        {
            return changeValue.magnitude / unitsXSecond;
        }

        public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector2> getter, DOSetter<Vector2> setter, float elapsed, Vector2 startValue, Vector2 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue += changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            switch (options.axisConstraint) {
            case AxisConstraint.X:
                Vector2 resX = getter();
                resX.x = startValue.x + changeValue.x * easeVal;
                if (options.snapping) resX.x = (float)Math.Round(resX.x);
                setter(resX);
                break;
            case AxisConstraint.Y:
                Vector2 resY = getter();
                resY.y = startValue.y + changeValue.y * easeVal;
                if (options.snapping) resY.y = (float)Math.Round(resY.y);
                setter(resY);
                break;
            default:
                startValue.x += changeValue.x * easeVal;
                startValue.y += changeValue.y * easeVal;
                if (options.snapping) {
                    startValue.x = (float)Math.Round(startValue.x);
                    startValue.y = (float)Math.Round(startValue.y);
                }
                setter(startValue);
                break;
            }
        }
    }
}
#endif