#if WP81
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/15 12:32

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Surrogates;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    public class QuaternionSurrogatePlugin : ABSTweenPlugin<QuaternionSurrogate, Vector3Surrogate, QuaternionOptions>
    {
        public override void Reset(TweenerCore<QuaternionSurrogate, Vector3Surrogate, QuaternionOptions> t) { }

        public override void SetFrom(TweenerCore<QuaternionSurrogate, Vector3Surrogate, QuaternionOptions> t, bool isRelative)
        {
            Vector3Surrogate prevEndVal = t.endValue;
            t.endValue = ((Quaternion)t.getter()).eulerAngles;
            if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative) {
                t.startValue = prevEndVal;
            } else if (t.plugOptions.rotateMode == RotateMode.FastBeyond360) {
                t.startValue = t.endValue + prevEndVal;
            } else {
                QuaternionSurrogate rot = t.getter();
                if (t.plugOptions.rotateMode == RotateMode.WorldAxisAdd) {
                    t.startValue = ((Quaternion)rot * Quaternion.Inverse(rot) * Quaternion.Euler(prevEndVal) * (Quaternion)rot).eulerAngles;
                } else {
                    t.startValue = ((Quaternion)rot * Quaternion.Euler(prevEndVal)).eulerAngles;
                }
                t.endValue = -prevEndVal;
            }
            t.setter(Quaternion.Euler(t.startValue));
        }

        public override Vector3Surrogate ConvertToStartValue(TweenerCore<QuaternionSurrogate, Vector3Surrogate, QuaternionOptions> t, QuaternionSurrogate value)
        {
            return ((Quaternion)value).eulerAngles;
        }

        public override void SetRelativeEndValue(TweenerCore<QuaternionSurrogate, Vector3Surrogate, QuaternionOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<QuaternionSurrogate, Vector3Surrogate, QuaternionOptions> t)
        {
            if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative) {
                // Rotation will be adapted to 360° and will take the shortest route
                // - Adapt to 360°
                Vector3Surrogate ev = t.endValue;
                if (ev.x > 360) ev.x = ev.x % 360;
                if (ev.y > 360) ev.y = ev.y % 360;
                if (ev.z > 360) ev.z = ev.z % 360;
                Vector3Surrogate changeVal = ev - t.startValue;
                // - Find shortest rotation
                float abs = (changeVal.x > 0 ? changeVal.x : -changeVal.x);
                if (abs > 180) changeVal.x = changeVal.x > 0 ? -(360 - abs) : 360 - abs;
                abs = (changeVal.y > 0 ? changeVal.y : -changeVal.y);
                if (abs > 180) changeVal.y = changeVal.y > 0 ? -(360 - abs) : 360 - abs;
                abs = (changeVal.z > 0 ? changeVal.z : -changeVal.z);
                if (abs > 180) changeVal.z = changeVal.z > 0 ? -(360 - abs) : 360 - abs;
                // - Assign
                t.changeValue = changeVal;
            } else if (t.plugOptions.rotateMode == RotateMode.FastBeyond360 || t.isRelative) {
                t.changeValue = t.endValue - t.startValue;
            } else {
                t.changeValue = t.endValue;
            }
        }

        public override float GetSpeedBasedDuration(QuaternionOptions options, float unitsXSecond, Vector3Surrogate changeValue)
        {
            return changeValue.magnitude / unitsXSecond;
        }

        public override void EvaluateAndApply(QuaternionOptions options, Tween t, bool isRelative, DOGetter<QuaternionSurrogate> getter, DOSetter<QuaternionSurrogate> setter, float elapsed, Vector3Surrogate startValue, Vector3Surrogate changeValue, float duration, bool usingInversePosition)
        {
            Vector3Surrogate endValue = startValue;

            if (t.loopType == LoopType.Incremental) endValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                endValue += changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            switch (options.rotateMode) {
            case RotateMode.WorldAxisAdd:
            case RotateMode.LocalAxisAdd:
                QuaternionSurrogate startRot = Quaternion.Euler(startValue); // Reset rotation
                endValue.x = changeValue.x * easeVal;
                endValue.y = changeValue.y * easeVal;
                endValue.z = changeValue.z * easeVal;
                if (options.rotateMode == RotateMode.WorldAxisAdd) setter((Quaternion)startRot * Quaternion.Inverse(startRot) * Quaternion.Euler(endValue) * (Quaternion)startRot);
                else setter((Quaternion)startRot * Quaternion.Euler(endValue));
                break;
            default:
                endValue.x += changeValue.x * easeVal;
                endValue.y += changeValue.y * easeVal;
                endValue.z += changeValue.z * easeVal;
                setter(Quaternion.Euler(endValue));
                break;
            }
        }
    }
}
#endif