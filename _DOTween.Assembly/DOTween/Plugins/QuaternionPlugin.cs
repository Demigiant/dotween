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
                t.startValue = GetEulerValForCalculations(t, prevEndVal, t.endValue);
            } else if (t.plugOptions.rotateMode == RotateMode.FastBeyond360) {
                // t.startValue = t.endValue + prevEndVal;
                t.startValue = GetEulerValForCalculations(t, t.endValue + prevEndVal, t.endValue);
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
        public override void SetFrom(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, Vector3 fromValue, bool setImmediately, bool isRelative)
        {
            if (isRelative) {
                Vector3 currVal = t.getter().eulerAngles;
                t.endValue += currVal;
                fromValue += currVal;
            }
            // t.startValue = fromValue;
            t.startValue = GetEulerValForCalculations(t, fromValue, t.endValue);
            if (setImmediately) t.setter(Quaternion.Euler(fromValue));
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
            // Vector3 endVal = GetEndValForCalculations(t);
            // If FROM don't use conversions method becuse it's already been used when assigning From values
            Vector3 endVal = t.isFrom ? t.endValue : GetEulerValForCalculations(t, t.endValue, t.startValue);
            Vector3 startVal = t.startValue;

            if (t.plugOptions.rotateMode == RotateMode.Fast && !t.isRelative) {
                // Rotation will be adapted to 360° and will take the shortest route
                // - Adapt to 360°
                // Vector3 ev = t.endValue;
                if (endVal.x > 360) endVal.x = endVal.x % 360;
                if (endVal.y > 360) endVal.y = endVal.y % 360;
                if (endVal.z > 360) endVal.z = endVal.z % 360;
                Vector3 changeVal = endVal - startVal;
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
                t.changeValue = endVal - startVal;
            } else {
                t.changeValue = endVal;
            }
            // Debug.Log("►►► " + t.startValue + " ► " + t.endValue);
            // Debug.Log(t.startValue + "/" + startVal + " - " + t.endValue + "/" + endVal + " ► " + t.changeValue);
            // Debug.Log("   ► " + Quaternion.Euler(t.startValue).eulerAngles + " - " + Quaternion.Euler(t.endValue).eulerAngles + " ► " + Quaternion.Euler(t.changeValue).eulerAngles);
        }

        public override float GetSpeedBasedDuration(QuaternionOptions options, float unitsXSecond, Vector3 changeValue)
        {
            return changeValue.magnitude / unitsXSecond;
        }

        public override void EvaluateAndApply(
            QuaternionOptions options, Tween t, bool isRelative, DOGetter<Quaternion> getter, DOSetter<Quaternion> setter,
            float elapsed, Vector3 startValue, Vector3 changeValue, float duration, bool usingInversePosition, int newCompletedSteps,
            UpdateNotice updateNotice
        ){
            if (options.dynamicLookAt) {
                TweenerCore<Quaternion, Vector3, QuaternionOptions> tweener = (TweenerCore<Quaternion, Vector3, QuaternionOptions>)t;
                tweener.endValue = options.dynamicLookAtWorldPosition;
                SpecialPluginsUtils.SetLookAt(tweener);
                SetChangeValue(tweener);
                changeValue = tweener.changeValue;
            }

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

        // This fixes wobbling when rotating on a single axis in some cases
        Vector3 GetEulerValForCalculations(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, Vector3 val, Vector3 counterVal)
        {
            // return val;
            if (t.isRelative) return val;
            // if (t.isFrom) return val; // Caller decides if this should be ignored or not
            // if (t.plugOptions.rotateMode != RotateMode.Fast) return val;

            Vector3 valFlipped = FlipEulerAngles(val);
            bool xAreTheSame = Mathf.Approximately(counterVal.x, val.x) || Mathf.Approximately(counterVal.x, valFlipped.x);
            bool yAreTheSame = Mathf.Approximately(counterVal.y, val.y) || Mathf.Approximately(counterVal.y, valFlipped.y);
            bool zAreTheSame = Mathf.Approximately(counterVal.z, val.z) || Mathf.Approximately(counterVal.z, valFlipped.z);
            bool isSingleAxisRotation = xAreTheSame && (yAreTheSame || zAreTheSame)
                                        || yAreTheSame && zAreTheSame;
            // Debug.Log(counterVal + " - " + val + " / " + valFlipped + " ► isSingleAxis: " + isSingleAxisRotation);
            if (!isSingleAxisRotation) return val;

            int axisToRotate = xAreTheSame
                ? yAreTheSame ? 2 : 1
                : 0;
            bool flip = false;
            switch (axisToRotate) {
            case 0: // X
                flip = !Mathf.Approximately(counterVal.y, val.y) || !Mathf.Approximately(counterVal.z, val.z);
                break;
            case 1: // Y
                flip = !Mathf.Approximately(counterVal.x, val.x) || !Mathf.Approximately(counterVal.z, val.z);
                break;
            case 2: // Z
                flip = !Mathf.Approximately(counterVal.x, val.x) || !Mathf.Approximately(counterVal.y, val.y);
                break;
            }
            // Debug.Log("   axis: " + axisToRotate + " - flip: " + flip);
            // if (flip) Debug.Log("    FLIPPED " + val + " to " + valFlipped);
            return flip ? valFlipped : val;
        }

        // Flips the euler angles from one representation to the other
        Vector3 FlipEulerAngles(Vector3 euler)
        {
            return new Vector3(180 - euler.x, euler.y + 180, euler.z + 180);
        }
    }
}
#endif