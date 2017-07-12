#if COMPATIBLE
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/20 18:13
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
    public class Vector4WrapperPlugin : ABSTweenPlugin<Vector4Wrapper, Vector4Wrapper, VectorOptions>
    {
        public override void Reset(TweenerCore<Vector4Wrapper, Vector4Wrapper, VectorOptions> t) { }

        public override void SetFrom(TweenerCore<Vector4Wrapper, Vector4Wrapper, VectorOptions> t, bool isRelative)
        {
            Vector4 prevEndVal = t.endValue;
            t.endValue = t.getter().value;
            t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
            Vector4 to = t.endValue;
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                to.x = t.startValue.value.x;
                break;
            case AxisConstraint.Y:
                to.y = t.startValue.value.y;
                break;
            case AxisConstraint.Z:
                to.z = t.startValue.value.z;
                break;
            case AxisConstraint.W:
                to.w = t.startValue.value.w;
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

        public override Vector4Wrapper ConvertToStartValue(TweenerCore<Vector4Wrapper, Vector4Wrapper, VectorOptions> t, Vector4Wrapper value)
        {
            return value.value;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector4Wrapper, Vector4Wrapper, VectorOptions> t)
        {
            t.endValue.value += t.startValue.value;
        }

        public override void SetChangeValue(TweenerCore<Vector4Wrapper, Vector4Wrapper, VectorOptions> t)
        {
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                t.changeValue.value = new Vector4(t.endValue.value.x - t.startValue.value.x, 0, 0, 0);
                break;
            case AxisConstraint.Y:
                t.changeValue.value = new Vector4(0, t.endValue.value.y - t.startValue.value.y, 0, 0);
                break;
            case AxisConstraint.Z:
                t.changeValue.value = new Vector4(0, 0, t.endValue.value.z - t.startValue.value.z, 0);
                break;
            case AxisConstraint.W:
                t.changeValue.value = new Vector4(0, 0, 0, t.endValue.value.w - t.startValue.value.w);
                break;
            default:
                t.changeValue.value = t.endValue.value - t.startValue.value;
                break;
            }
        }

        public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector4Wrapper changeValue)
        {
            return changeValue.value.magnitude / unitsXSecond;
        }

        public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector4Wrapper> getter, DOSetter<Vector4Wrapper> setter, float elapsed, Vector4Wrapper startValue, Vector4Wrapper changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            if (t.loopType == LoopType.Incremental) startValue.value += changeValue.value * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue.value += changeValue.value * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            switch (options.axisConstraint) {
            case AxisConstraint.X:
                Vector4 resX = getter().value;
                resX.x = startValue.value.x + changeValue.value.x * easeVal;
                if (options.snapping) resX.x = (float)Math.Round(resX.x);
                setter(resX);
                break;
            case AxisConstraint.Y:
                Vector4 resY = getter().value;
                resY.y = startValue.value.y + changeValue.value.y * easeVal;
                if (options.snapping) resY.y = (float)Math.Round(resY.y);
                setter(resY);
                break;
            case AxisConstraint.Z:
                Vector4 resZ = getter().value;
                resZ.z = startValue.value.z + changeValue.value.z * easeVal;
                if (options.snapping) resZ.z = (float)Math.Round(resZ.z);
                setter(resZ);
                break;
            case AxisConstraint.W:
                Vector4 resW = getter().value;
                resW.w = startValue.value.w + changeValue.value.w * easeVal;
                if (options.snapping) resW.w = (float)Math.Round(resW.w);
                setter(resW);
                break;
            default:
                startValue.value.x += changeValue.value.x * easeVal;
                startValue.value.y += changeValue.value.y * easeVal;
                startValue.value.z += changeValue.value.z * easeVal;
                startValue.value.w += changeValue.value.w * easeVal;
                if (options.snapping) {
                    startValue.value.x = (float)Math.Round(startValue.value.x);
                    startValue.value.y = (float)Math.Round(startValue.value.y);
                    startValue.value.z = (float)Math.Round(startValue.value.z);
                    startValue.value.w = (float)Math.Round(startValue.value.w);
                }
                setter(startValue.value);
                break;
            }
        }
    }
}
#endif