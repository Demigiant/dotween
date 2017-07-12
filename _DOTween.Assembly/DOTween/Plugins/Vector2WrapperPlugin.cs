#if COMPATIBLE
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/20 18:06

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Core.Surrogates;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    public class Vector2WrapperPlugin : ABSTweenPlugin<Vector2Wrapper, Vector2Wrapper, VectorOptions>
    {
        public override void Reset(TweenerCore<Vector2Wrapper, Vector2Wrapper, VectorOptions> t) { }

        public override void SetFrom(TweenerCore<Vector2Wrapper, Vector2Wrapper, VectorOptions> t, bool isRelative)
        {
            Vector2 prevEndVal = t.endValue;
            t.endValue = t.getter().value;
            t.startValue.value = isRelative ? t.endValue.value + prevEndVal : prevEndVal;
            Vector2 to = t.endValue;
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                to.x = t.startValue.value.x;
                break;
            case AxisConstraint.Y:
                to.y = t.startValue.value.y;
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

        public override Vector2Wrapper ConvertToStartValue(TweenerCore<Vector2Wrapper, Vector2Wrapper, VectorOptions> t, Vector2Wrapper value)
        {
            return value.value;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector2Wrapper, Vector2Wrapper, VectorOptions> t)
        {
            t.endValue.value += t.startValue.value;
        }

        public override void SetChangeValue(TweenerCore<Vector2Wrapper, Vector2Wrapper, VectorOptions> t)
        {
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                t.changeValue.value = new Vector2(t.endValue.value.x - t.startValue.value.x, 0);
                break;
            case AxisConstraint.Y:
                t.changeValue.value = new Vector2(0, t.endValue.value.y - t.startValue.value.y);
                break;
            default:
                t.changeValue.value = t.endValue.value - t.startValue.value;
                break;
            }
        }

        public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector2Wrapper changeValue)
        {
            return changeValue.value.magnitude / unitsXSecond;
        }

        public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector2Wrapper> getter, DOSetter<Vector2Wrapper> setter, float elapsed, Vector2Wrapper startValue, Vector2Wrapper changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            if (t.loopType == LoopType.Incremental) startValue.value += changeValue.value * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue.value += changeValue.value * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            switch (options.axisConstraint) {
            case AxisConstraint.X:
                Vector2 resX = getter().value;
                resX.x = startValue.value.x + changeValue.value.x * easeVal;
                if (options.snapping) resX.x = (float)Math.Round(resX.x);
                setter(resX);
                break;
            case AxisConstraint.Y:
                Vector2 resY = getter().value;
                resY.y = startValue.value.y + changeValue.value.y * easeVal;
                if (options.snapping) resY.y = (float)Math.Round(resY.y);
                setter(resY);
                break;
            default:
                startValue.value.x += changeValue.value.x * easeVal;
                startValue.value.y += changeValue.value.y * easeVal;
                if (options.snapping) {
                    startValue.value.x = (float)Math.Round(startValue.value.x);
                    startValue.value.y = (float)Math.Round(startValue.value.y);
                }
                setter(startValue.value);
                break;
            }
        }
    }
}
#endif