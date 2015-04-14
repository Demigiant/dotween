#if WP81
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/14 12:44

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Surrogates;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    public class Vector3SurrogatePlugin : ABSTweenPlugin<Vector3Surrogate, Vector3Surrogate, VectorOptions>
    {
        public override void Reset(TweenerCore<Vector3Surrogate, Vector3Surrogate, VectorOptions> t) { }

        public override void SetFrom(TweenerCore<Vector3Surrogate, Vector3Surrogate, VectorOptions> t, bool isRelative)
        {
            Vector3Surrogate prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
            Vector3Surrogate to = t.endValue;
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

        public override Vector3Surrogate ConvertToStartValue(TweenerCore<Vector3Surrogate, Vector3Surrogate, VectorOptions> t, Vector3Surrogate value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector3Surrogate, Vector3Surrogate, VectorOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Vector3Surrogate, Vector3Surrogate, VectorOptions> t)
        {
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                t.changeValue = new Vector3Surrogate(t.endValue.x - t.startValue.x, 0, 0);
                break;
            case AxisConstraint.Y:
                t.changeValue = new Vector3Surrogate(0, t.endValue.y - t.startValue.y, 0);
                break;
            case AxisConstraint.Z:
                t.changeValue = new Vector3Surrogate(0, 0, t.endValue.z - t.startValue.z);
                break;
            default:
                t.changeValue = t.endValue - t.startValue;
                break;
            }
        }

        public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector3Surrogate changeValue)
        {
            return changeValue.magnitude / unitsXSecond;
        }

        public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector3Surrogate> getter, DOSetter<Vector3Surrogate> setter, float elapsed, Vector3Surrogate startValue, Vector3Surrogate changeValue, float duration, bool usingInversePosition)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue += changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            switch (options.axisConstraint) {
            case AxisConstraint.X:
                Vector3Surrogate resX = getter();
                resX.x = startValue.x + changeValue.x * easeVal;
                if (options.snapping) resX.x = (float)Math.Round(resX.x);
                setter(resX);
                break;
            case AxisConstraint.Y:
                Vector3Surrogate resY = getter();
                resY.y = startValue.y + changeValue.y * easeVal;
                if (options.snapping) resY.y = (float)Math.Round(resY.y);
                setter(resY);
                break;
            case AxisConstraint.Z:
                Vector3Surrogate resZ = getter();
                resZ.z = startValue.z + changeValue.z * easeVal;
                if (options.snapping) resZ.z = (float)Math.Round(resZ.z);
                setter(resZ);
                break;
            default:
                startValue.x += changeValue.x * easeVal;
                startValue.y += changeValue.y * easeVal;
                startValue.z += changeValue.z * easeVal;
                if (options.snapping) {
                    startValue.x = (float)Math.Round(startValue.x);
                    startValue.y = (float)Math.Round(startValue.y);
                    startValue.z = (float)Math.Round(startValue.z);
                }
                setter(startValue);
                break;
            }
        }
    }
}
#endif