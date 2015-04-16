#if WP81
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/14 12:46

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Surrogates;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    public class Vector2SurrogatePlugin : ABSTweenPlugin<Vector2Surrogate, Vector2Surrogate, VectorOptions>
    {
        public override void Reset(TweenerCore<Vector2Surrogate, Vector2Surrogate, VectorOptions> t) { }

        public override void SetFrom(TweenerCore<Vector2Surrogate, Vector2Surrogate, VectorOptions> t, bool isRelative)
        {
            Vector2Surrogate prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
            Vector2Surrogate to = t.endValue;
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

        public override Vector2Surrogate ConvertToStartValue(TweenerCore<Vector2Surrogate, Vector2Surrogate, VectorOptions> t, Vector2Surrogate value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector2Surrogate, Vector2Surrogate, VectorOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Vector2Surrogate, Vector2Surrogate, VectorOptions> t)
        {
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                t.changeValue = new Vector2Surrogate(t.endValue.x - t.startValue.x, 0);
                break;
            case AxisConstraint.Y:
                t.changeValue = new Vector2Surrogate(0, t.endValue.y - t.startValue.y);
                break;
            default:
                t.changeValue = t.endValue - t.startValue;
                break;
            }
        }

        public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector2Surrogate changeValue)
        {
            return changeValue.magnitude / unitsXSecond;
        }

        public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector2Surrogate> getter, DOSetter<Vector2Surrogate> setter, float elapsed, Vector2Surrogate startValue, Vector2Surrogate changeValue, float duration, bool usingInversePosition)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue += changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            switch (options.axisConstraint) {
            case AxisConstraint.X:
                Vector2Surrogate resX = getter();
                resX.x = startValue.x + changeValue.x * easeVal;
                if (options.snapping) resX.x = (float)Math.Round(resX.x);
                setter(resX);
                break;
            case AxisConstraint.Y:
                Vector2Surrogate resY = getter();
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