// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2020/09/23 16:30
// License Copyright (c) Daniele Giardini
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
    public struct CircleOptions : IPlugOptions
    {
        public float endValueDegrees;
        public bool relativeCenter;
        public bool snapping;

        internal Vector2 center; // Simply replicates the Tweener's endValue
        internal float radius;
        internal float startValueDegrees;

        internal bool initialized;
        
        public void Reset()
        {
            initialized = false;
            startValueDegrees = endValueDegrees = 0;
            relativeCenter = false;
            snapping = false;
        }

        public void Initialize(Vector2 startValue, Vector2 endValue)
        {
            initialized = true;
            center = endValue;
            if (relativeCenter) center = startValue + center;
            radius = Vector2.Distance(center, startValue);
            Vector2 semiNormalizedP = startValue - center;
            startValueDegrees = Mathf.Atan2(semiNormalizedP.x, semiNormalizedP.y) * Mathf.Rad2Deg;
        }
    }

    /// <summary>
    /// Tweens a Vector2 along a circle.
    /// EndValue represents the center of the circle, start and end value degrees are inside options
    /// ChangeValue x is changeValue°, y is unused
    /// </summary>
    public class CirclePlugin : ABSTweenPlugin<Vector2, Vector2, CircleOptions>
    {
        public override void Reset(TweenerCore<Vector2, Vector2, CircleOptions> t) { }

        public override void SetFrom(TweenerCore<Vector2, Vector2, CircleOptions> t, bool isRelative)
        {
            if (!t.plugOptions.initialized) {
                t.startValue = t.getter();
                t.plugOptions.Initialize(t.startValue, t.endValue);
            }
            float prevEndVal = t.plugOptions.endValueDegrees;
            t.plugOptions.endValueDegrees = t.plugOptions.startValueDegrees;
            t.plugOptions.startValueDegrees = isRelative ? t.plugOptions.endValueDegrees + prevEndVal : prevEndVal;
            t.startValue = GetPositionOnCircle(t.plugOptions, t.plugOptions.startValueDegrees);
            t.setter(t.startValue);
        }

        public override void SetFrom(TweenerCore<Vector2, Vector2, CircleOptions> t, Vector2 fromValue, bool setImmediately, bool isRelative)
        {
            if (!t.plugOptions.initialized) {
                t.startValue = t.getter();
                t.plugOptions.Initialize(t.startValue, t.endValue);
            }
            float fromValueDegrees = fromValue.x;
            if (isRelative) {
                float currVal = t.plugOptions.startValueDegrees;
                t.plugOptions.endValueDegrees += currVal;
                fromValueDegrees += currVal;
            }
            t.plugOptions.startValueDegrees = fromValueDegrees;
            t.startValue = GetPositionOnCircle(t.plugOptions, fromValueDegrees);
            if (setImmediately) t.setter(t.startValue);
        }

        public static ABSTweenPlugin<Vector2, Vector2, CircleOptions> Get()
        {
            return PluginsManager.GetCustomPlugin<CirclePlugin, Vector2, Vector2, CircleOptions>();
        }

        public override Vector2 ConvertToStartValue(TweenerCore<Vector2, Vector2, CircleOptions> t, Vector2 value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Vector2, Vector2, CircleOptions> t)
        {
            if (!t.plugOptions.initialized) t.plugOptions.Initialize(t.startValue, t.endValue);
            t.plugOptions.endValueDegrees += t.plugOptions.startValueDegrees;
        }

        public override void SetChangeValue(TweenerCore<Vector2, Vector2, CircleOptions> t)
        {
            if (!t.plugOptions.initialized) t.plugOptions.Initialize(t.startValue, t.endValue);
            t.changeValue = new Vector2(t.plugOptions.endValueDegrees - t.plugOptions.startValueDegrees, 0);
        }

        public override float GetSpeedBasedDuration(CircleOptions options, float unitsXSecond, Vector2 changeValue)
        {
            return changeValue.x / unitsXSecond;
        }

        public override void EvaluateAndApply(
            CircleOptions options, Tween t, bool isRelative, DOGetter<Vector2> getter, DOSetter<Vector2> setter,
            float elapsed, Vector2 startValue, Vector2 changeValue, float duration, bool usingInversePosition, int newCompletedSteps,
            UpdateNotice updateNotice
        ){
            float startValueDegrees = options.startValueDegrees;
            if (t.loopType == LoopType.Incremental) startValueDegrees += changeValue.x * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValueDegrees += changeValue.x * (t.loopType == LoopType.Incremental ? t.loops : 1)
                                                 * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            setter(GetPositionOnCircle(options, startValueDegrees + changeValue.x * easeVal));
        }

        public Vector2 GetPositionOnCircle(CircleOptions options, float degrees)
        {
            Vector2 pos = DOTweenUtils.GetPointOnCircle(options.center, options.radius, degrees);
            if (options.snapping) {
                pos.x = Mathf.Round(pos.x);
                pos.y = Mathf.Round(pos.y);
            }
            return pos;
        }

        // // DEBUG
        // void DebugLogData(CircleOptions options, Vector2 startValue, Vector2 changeValue, string prefix = "")
        // {
        //     Debug.Log(prefix + "startValue: " + startValue + ", startValue°: " + options.startValueDegrees + ", endValue°: " + options.endValueDegrees
        //               + ", radius: " + options.radius + ", center: " + options.center + ", relativeCenter: " + options.relativeCenter
        //               + ", changeValue.x: " + changeValue.x);
        // }
        // // DEBUG END
    }
}