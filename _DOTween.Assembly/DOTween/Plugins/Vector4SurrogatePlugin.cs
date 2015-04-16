#if WP81
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/14 12:19

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
    public class Vector4SurrogatePlugin : ABSTweenPlugin<Vector4Surrogate, Vector4Surrogate, VectorOptions>
    {
        public override void Reset(TweenerCore<Vector4Surrogate, Vector4Surrogate, VectorOptions> t) { }

        public override void SetFrom(TweenerCore<Vector4Surrogate, Vector4Surrogate, VectorOptions> t, bool isRelative)
        {
            Vector4Surrogate prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
            Vector4Surrogate to = t.endValue;
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                to.x = t.startValue.x;
                break;
            case AxisConstraint.Y:
                to.y = t.startValue.y;
                break;
            case AxisConstraint.Z:
                to.z = t.startValue.z;
                break;
            case AxisConstraint.W:
                to.w = t.startValue.w;
                break;
            default:
                to = t.startValue;
                break;
            }
            if (t.plugOptions.snapping) {
                to.x = (float)Math.Round(to.x);
                to.y = (float)Math.Round(to.y);
                to.z = (float)Math.Round(to.z);
                to.w = (float)Math.Round(to.w);
            }
            t.setter(to);
        }

        public override Vector4Surrogate ConvertToStartValue(TweenerCore<Vector4Surrogate, Vector4Surrogate, VectorOptions> t, Vector4Surrogate value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector4Surrogate, Vector4Surrogate, VectorOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Vector4Surrogate, Vector4Surrogate, VectorOptions> t)
        {
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                t.changeValue = new Vector4Surrogate(t.endValue.x - t.startValue.x, 0, 0, 0);
                break;
            case AxisConstraint.Y:
                t.changeValue = new Vector4Surrogate(0, t.endValue.y - t.startValue.y, 0, 0);
                break;
            case AxisConstraint.Z:
                t.changeValue = new Vector4Surrogate(0, 0, t.endValue.z - t.startValue.z, 0);
                break;
            case AxisConstraint.W:
                t.changeValue = new Vector4Surrogate(0, 0, 0, t.endValue.w - t.startValue.w);
                break;
            default:
                t.changeValue = t.endValue - t.startValue;
                break;
            }
        }

        public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector4Surrogate changeValue)
        {
            return changeValue.magnitude / unitsXSecond;
        }

        public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector4Surrogate> getter, DOSetter<Vector4Surrogate> setter, float elapsed, Vector4Surrogate startValue, Vector4Surrogate changeValue, float duration, bool usingInversePosition)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue += changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            switch (options.axisConstraint) {
            case AxisConstraint.X:
                Vector4Surrogate resX = getter();
                resX.x = startValue.x + changeValue.x * easeVal;
                if (options.snapping) resX.x = (float)Math.Round(resX.x);
                setter(resX);
                break;
            case AxisConstraint.Y:
                Vector4Surrogate resY = getter();
                resY.y = startValue.y + changeValue.y * easeVal;
                if (options.snapping) resY.y = (float)Math.Round(resY.y);
                setter(resY);
                break;
            case AxisConstraint.Z:
                Vector4Surrogate resZ = getter();
                resZ.z = startValue.z + changeValue.z * easeVal;
                if (options.snapping) resZ.z = (float)Math.Round(resZ.z);
                setter(resZ);
                break;
            case AxisConstraint.W:
                Vector4Surrogate resW = getter();
                resW.w = startValue.w + changeValue.w * easeVal;
                if (options.snapping) resW.w = (float)Math.Round(resW.w);
                setter(resW);
                break;
            default:
                startValue.x += changeValue.x * easeVal;
                startValue.y += changeValue.y * easeVal;
                startValue.z += changeValue.z * easeVal;
                startValue.w += changeValue.w * easeVal;
                if (options.snapping) {
                    startValue.x = (float)Math.Round(startValue.x);
                    startValue.y = (float)Math.Round(startValue.y);
                    startValue.z = (float)Math.Round(startValue.z);
                    startValue.w = (float)Math.Round(startValue.w);
                }
                setter(startValue);
                break;
            }
        }
    }
}
#endif