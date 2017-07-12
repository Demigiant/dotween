#if !COMPATIBLE
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/07 20:02
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    public class QuaternionPlugin : ABSTweenPlugin<Quaternion,Vector3,QuaternionOptions>
    {
        public override void Reset(TweenerCore<Quaternion, Vector3, QuaternionOptions> t) { }

        public override void SetFrom(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, bool isRelative)
        {
            Vector3 prevEndVal = t.endValue;
            t.endValue = t.getter().eulerAngles;
            if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative) {
                t.startValue = prevEndVal;
            } else if (t.plugOptions.rotateMode == RotateMode.FastBeyond360) {
                t.startValue = t.endValue + prevEndVal;
            } else {
                Quaternion rot = t.getter();
                if (t.plugOptions.rotateMode == RotateMode.WorldAxisAdd) {
                    t.startValue = (rot * Quaternion.Inverse(rot) * Quaternion.Euler(prevEndVal) * rot).eulerAngles;
                } else {
                    t.startValue = (rot * Quaternion.Euler(prevEndVal)).eulerAngles;
                }
                t.endValue = -prevEndVal;
            }
            t.setter(Quaternion.Euler(t.startValue));
        }

        public override Vector3 ConvertToStartValue(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, Quaternion value)
        {
            return value.eulerAngles;
        }

        public override void SetRelativeEndValue(TweenerCore<Quaternion, Vector3, QuaternionOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Quaternion, Vector3, QuaternionOptions> t)
        {
            if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative) {
                // Rotation will be adapted to 360° and will take the shortest route
                // - Adapt to 360°
                Vector3 ev = t.endValue;
                if (ev.x > 360) ev.x = ev.x % 360;
                if (ev.y > 360) ev.y = ev.y % 360;
                if (ev.z > 360) ev.z = ev.z % 360;
                Vector3 changeVal = ev - t.startValue;
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

        public override float GetSpeedBasedDuration(QuaternionOptions options, float unitsXSecond, Vector3 changeValue)
        {
            return changeValue.magnitude / unitsXSecond;
        }

        public override void EvaluateAndApply(QuaternionOptions options, Tween t, bool isRelative, DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            Vector3 endValue = startValue;

            if (t.loopType == LoopType.Incremental) endValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                endValue += changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            switch (options.rotateMode) {
            case RotateMode.WorldAxisAdd:
            case RotateMode.LocalAxisAdd:
                Quaternion startRot = Quaternion.Euler(startValue); // Reset rotation
                endValue.x = changeValue.x * easeVal;
                endValue.y = changeValue.y * easeVal;
                endValue.z = changeValue.z * easeVal;
                if (options.rotateMode == RotateMode.WorldAxisAdd) setter(startRot * Quaternion.Inverse(startRot) * Quaternion.Euler(endValue) * startRot);
                else setter(startRot * Quaternion.Euler(endValue));
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