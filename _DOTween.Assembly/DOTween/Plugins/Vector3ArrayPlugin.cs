// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/08/20 15:05
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    /// <summary>
    /// This plugin generates some GC allocations at startup
    /// </summary>
    public class Vector3ArrayPlugin : ABSTweenPlugin<Vector3, Vector3[], Vector3ArrayOptions>
    {
        public override void Reset(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
        {
            t.startValue = t.endValue = t.changeValue = null;
        }

        public override void SetFrom(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, bool isRelative) {}

        public override Vector3[] ConvertToStartValue(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, Vector3 value)
        {
            int len = t.endValue.Length;
            Vector3[] res = new Vector3[len];
            for (int i = 0; i < len; i++) {
                if (i == 0) res[i] = value;
                else res[i] = t.endValue[i - 1];
            }
            return res;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
        {
            int len = t.endValue.Length;
            for (int i = 0; i < len; ++i) {
                if (i > 0) t.startValue[i] = t.endValue[i - 1];
                t.endValue[i] = t.startValue[i] + t.endValue[i];
            }
        }

        public override void SetChangeValue(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
        {
            int len = t.endValue.Length;
            t.changeValue = new Vector3[len];
            for (int i = 0; i < len; ++i) t.changeValue[i] = t.endValue[i] - t.startValue[i];
        }

        public override float GetSpeedBasedDuration(Vector3ArrayOptions options, float unitsXSecond, Vector3[] changeValue)
        {
            float totDuration = 0;
            int len = changeValue.Length;
            for (int i = 0; i < len; ++i) {
                float duration = changeValue[i].magnitude / options.durations[i];
                options.durations[i] = duration;
                totDuration += duration;
            }
            return totDuration;
        }

        public override void EvaluateAndApply(Vector3ArrayOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, DOSetter<Vector3> setter, float elapsed, Vector3[] startValue, Vector3[] changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            Vector3 incrementValue = Vector3.zero;
            if (t.loopType == LoopType.Incremental) {
                int iterations = t.isComplete ? t.completedLoops - 1 : t.completedLoops;
                if (iterations > 0) {
                    int end = startValue.Length - 1;
                    incrementValue = (startValue[end] + changeValue[end] - startValue[0]) * iterations;
                }
            }
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                int iterations = (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
                if (iterations > 0) {
                    int end = startValue.Length - 1;
                    incrementValue += (startValue[end] + changeValue[end] - startValue[0]) * iterations;
                }
            }

            // Find correct index and segmentElapsed
            int index = 0;
            float segmentElapsed = 0;
            float segmentDuration = 0;
            int len = options.durations.Length;
            float count = 0;
            for (int i = 0; i < len; i++) {
                segmentDuration = options.durations[i];
                count += segmentDuration;
                if (elapsed > count) {
                    segmentElapsed += segmentDuration;
                    continue;
                }
                index = i;
                segmentElapsed = elapsed - segmentElapsed;
                break;
            }
            // Evaluate
            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, segmentElapsed, segmentDuration, t.easeOvershootOrAmplitude, t.easePeriod);
            Vector3 res;
            switch (options.axisConstraint) {
            case AxisConstraint.X:
                res = getter();
                res.x = startValue[index].x + incrementValue.x + changeValue[index].x * easeVal;
                if (options.snapping) res.x = (float)Math.Round(res.x);
                setter(res);
                break;
            case AxisConstraint.Y:
                res = getter();
                res.y = startValue[index].y + incrementValue.y + changeValue[index].y * easeVal;
                if (options.snapping) res.y = (float)Math.Round(res.y);
                setter(res);
                return;
            case AxisConstraint.Z:
                res = getter();
                res.z = startValue[index].z + incrementValue.z + changeValue[index].z * easeVal;
                if (options.snapping) res.z = (float)Math.Round(res.z);
                setter(res);
                break;
            default:
                res.x = startValue[index].x + incrementValue.x + changeValue[index].x * easeVal;
                res.y = startValue[index].y + incrementValue.y + changeValue[index].y * easeVal;
                res.z = startValue[index].z + incrementValue.z + changeValue[index].z * easeVal;
                if (options.snapping) {
                    res.x = (float)Math.Round(res.x);
                    res.y = (float)Math.Round(res.y);
                    res.z = (float)Math.Round(res.z);
                }
                setter(res);
                break;
            }
        }
    }
}