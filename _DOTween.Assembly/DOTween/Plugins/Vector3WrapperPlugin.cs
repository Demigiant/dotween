#if COMPATIBLE
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/20 16:59

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
    public class Vector3WrapperPlugin : ABSTweenPlugin<Vector3Wrapper, Vector3Wrapper, VectorOptions>
    {
        public override void Reset(TweenerCore<Vector3Wrapper, Vector3Wrapper, VectorOptions> t) { }

        public override void SetFrom(TweenerCore<Vector3Wrapper, Vector3Wrapper, VectorOptions> t, bool isRelative)
        {
            Vector3 prevEndVal = t.endValue;
            t.endValue = t.getter().value;
            t.startValue.value = isRelative ? t.endValue.value + prevEndVal : prevEndVal;
            Vector3 to = t.endValue;
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
            default:
                to = t.startValue;
                break;
            }
            if (t.plugOptions.snapping) {
                to.x = (float)Math.Round(to.x);
                to.y = (float)Math.Round(to.y);
                to.z = (float)Math.Round(to.z);
            }
            t.setter(to);
        }

        public override Vector3Wrapper ConvertToStartValue(TweenerCore<Vector3Wrapper, Vector3Wrapper, VectorOptions> t, Vector3Wrapper value)
        {
            return value.value;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector3Wrapper, Vector3Wrapper, VectorOptions> t)
        {
            t.endValue.value += t.startValue.value;
        }

        public override void SetChangeValue(TweenerCore<Vector3Wrapper, Vector3Wrapper, VectorOptions> t)
        {
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                t.changeValue.value = new Vector3(t.endValue.value.x - t.startValue.value.x, 0, 0);
                break;
            case AxisConstraint.Y:
                t.changeValue.value = new Vector3(0, t.endValue.value.y - t.startValue.value.y, 0);
                break;
            case AxisConstraint.Z:
                t.changeValue.value = new Vector3(0, 0, t.endValue.value.z - t.startValue.value.z);
                break;
            default:
                t.changeValue.value = t.endValue.value - t.startValue.value;
                break;
            }
        }

        public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector3Wrapper changeValue)
        {
            return changeValue.value.magnitude / unitsXSecond;
        }

        public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector3Wrapper> getter, DOSetter<Vector3Wrapper> setter, float elapsed, Vector3Wrapper startValue, Vector3Wrapper changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            if (t.loopType == LoopType.Incremental) startValue.value += changeValue.value * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue.value += changeValue.value * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            switch (options.axisConstraint) {
            case AxisConstraint.X:
                Vector3 resX = getter().value;
                resX.x = startValue.value.x + changeValue.value.x * easeVal;
                if (options.snapping) resX.x = (float)Math.Round(resX.x);
                setter(resX);
                break;
            case AxisConstraint.Y:
                Vector3 resY = getter().value;
                resY.y = startValue.value.y + changeValue.value.y * easeVal;
                if (options.snapping) resY.y = (float)Math.Round(resY.y);
                setter(resY);
                break;
            case AxisConstraint.Z:
                Vector3 resZ = getter().value;
                resZ.z = startValue.value.z + changeValue.value.z * easeVal;
                if (options.snapping) resZ.z = (float)Math.Round(resZ.z);
                setter(resZ);
                break;
            default:
                startValue.value.x += changeValue.value.x * easeVal;
                startValue.value.y += changeValue.value.y * easeVal;
                startValue.value.z += changeValue.value.z * easeVal;
                if (options.snapping) {
                    startValue.value.x = (float)Math.Round(startValue.value.x);
                    startValue.value.y = (float)Math.Round(startValue.value.y);
                    startValue.value.z = (float)Math.Round(startValue.value.z);
                }
                setter(startValue.value);
                break;
            }
        }
    }
}
#endif