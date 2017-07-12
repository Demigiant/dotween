#if COMPATIBLE
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/20 18:21

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
    public class QuaternionWrapperPlugin : ABSTweenPlugin<QuaternionWrapper, Vector3Wrapper, QuaternionOptions>
    {
        public override void Reset(TweenerCore<QuaternionWrapper, Vector3Wrapper, QuaternionOptions> t) { }

        public override void SetFrom(TweenerCore<QuaternionWrapper, Vector3Wrapper, QuaternionOptions> t, bool isRelative)
        {
            Vector3 prevEndVal = t.endValue.value;
            t.endValue.value = t.getter().value.eulerAngles;
            if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative) {
                t.startValue.value = prevEndVal;
            } else if (t.plugOptions.rotateMode == RotateMode.FastBeyond360) {
                t.startValue.value = t.endValue.value + prevEndVal;
            } else {
                Quaternion rot = t.getter().value;
                if (t.plugOptions.rotateMode == RotateMode.WorldAxisAdd) {
                    t.startValue.value = (rot * Quaternion.Inverse(rot) * Quaternion.Euler(prevEndVal) * rot).eulerAngles;
                } else {
                    t.startValue.value = (rot * Quaternion.Euler(prevEndVal)).eulerAngles;
                }
                t.endValue.value = -prevEndVal;
            }
            t.setter(Quaternion.Euler(t.startValue.value));
        }

        public override Vector3Wrapper ConvertToStartValue(TweenerCore<QuaternionWrapper, Vector3Wrapper, QuaternionOptions> t, QuaternionWrapper value)
        {
            return value.value.eulerAngles;
        }

        public override void SetRelativeEndValue(TweenerCore<QuaternionWrapper, Vector3Wrapper, QuaternionOptions> t)
        {
            t.endValue.value += t.startValue.value;
        }

        public override void SetChangeValue(TweenerCore<QuaternionWrapper, Vector3Wrapper, QuaternionOptions> t)
        {
            if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative) {
                // Rotation will be adapted to 360° and will take the shortest route
                // - Adapt to 360°
                Vector3 ev = t.endValue.value;
                if (ev.x > 360) ev.x = ev.x % 360;
                if (ev.y > 360) ev.y = ev.y % 360;
                if (ev.z > 360) ev.z = ev.z % 360;
                Vector3 changeVal = ev - t.startValue.value;
                // - Find shortest rotation
                float abs = (changeVal.x > 0 ? changeVal.x : -changeVal.x);
                if (abs > 180) changeVal.x = changeVal.x > 0 ? -(360 - abs) : 360 - abs;
                abs = (changeVal.y > 0 ? changeVal.y : -changeVal.y);
                if (abs > 180) changeVal.y = changeVal.y > 0 ? -(360 - abs) : 360 - abs;
                abs = (changeVal.z > 0 ? changeVal.z : -changeVal.z);
                if (abs > 180) changeVal.z = changeVal.z > 0 ? -(360 - abs) : 360 - abs;
                // - Assign
                t.changeValue.value = changeVal;
            } else if (t.plugOptions.rotateMode == RotateMode.FastBeyond360 || t.isRelative) {
                t.changeValue.value = t.endValue.value - t.startValue.value;
            } else {
                t.changeValue.value = t.endValue.value;
            }
        }

        public override float GetSpeedBasedDuration(QuaternionOptions options, float unitsXSecond, Vector3Wrapper changeValue)
        {
            return changeValue.value.magnitude / unitsXSecond;
        }

        public override void EvaluateAndApply(QuaternionOptions options, Tween t, bool isRelative, DOGetter<QuaternionWrapper> getter, DOSetter<QuaternionWrapper> setter, float elapsed, Vector3Wrapper startValue, Vector3Wrapper changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            Vector3 endValue = startValue.value;

            if (t.loopType == LoopType.Incremental) endValue += changeValue.value * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                endValue += changeValue.value * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            switch (options.rotateMode) {
            case RotateMode.WorldAxisAdd:
            case RotateMode.LocalAxisAdd:
                Quaternion startRot = Quaternion.Euler(startValue.value); // Reset rotation
                endValue.x = changeValue.value.x * easeVal;
                endValue.y = changeValue.value.y * easeVal;
                endValue.z = changeValue.value.z * easeVal;
                if (options.rotateMode == RotateMode.WorldAxisAdd) setter(startRot * Quaternion.Inverse(startRot) * Quaternion.Euler(endValue) * startRot);
                else setter(startRot * Quaternion.Euler(endValue));
                break;
            default:
                endValue.x += changeValue.value.x * easeVal;
                endValue.y += changeValue.value.y * easeVal;
                endValue.z += changeValue.value.z * easeVal;
                setter(Quaternion.Euler(endValue));
                break;
            }
        }
    }
}
#endif