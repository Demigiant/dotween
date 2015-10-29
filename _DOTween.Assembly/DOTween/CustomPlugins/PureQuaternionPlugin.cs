// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/10/15 12:29
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.CustomPlugins
{
    /// <summary>
    /// Straight Quaternion plugin. Instead of using Vector3 values accepts Quaternion values directly.
    /// <para>Beware: doesn't work with LoopType.Incremental (neither directly nor if inside a LoopType.Incremental Sequence).</para>
    /// <para>To use it, call DOTween.To with the plugin parameter overload, passing it <c>PureQuaternionPlugin.Plug()</c> as first parameter
    /// (do not use any of the other public PureQuaternionPlugin methods):</para>
    /// <code>DOTween.To(PureQuaternionPlugin.Plug(), ()=> myQuaternionProperty, x=> myQuaternionProperty = x, myQuaternionEndValue, duration);</code>
    /// </summary>
    public class PureQuaternionPlugin : ABSTweenPlugin<Quaternion, Quaternion, NoOptions>
    {
        static PureQuaternionPlugin _plug;
        /// <summary>
        /// Plug this plugin inside a DOTween.To call.
        /// <para>Example:</para>
        /// <code>DOTween.To(PureQuaternionPlugin.Plug(), ()=> myQuaternionProperty, x=> myQuaternionProperty = x, myQuaternionEndValue, duration);</code>
        /// </summary>
        public static PureQuaternionPlugin Plug()
        {
            if (_plug == null) _plug = new PureQuaternionPlugin();
            return _plug;
        }

        /// <summary>INTERNAL: do not use</summary>
        public override void Reset(TweenerCore<Quaternion, Quaternion, NoOptions> t) { }

        /// <summary>INTERNAL: do not use</summary>
        public override void SetFrom(TweenerCore<Quaternion, Quaternion, NoOptions> t, bool isRelative)
        {
            Quaternion prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = isRelative ? t.endValue * prevEndVal : prevEndVal;
            t.setter(t.startValue);
        }

        /// <summary>INTERNAL: do not use</summary>
        public override Quaternion ConvertToStartValue(TweenerCore<Quaternion, Quaternion, NoOptions> t, Quaternion value)
        {
            return value;
        }

        /// <summary>INTERNAL: do not use</summary>
        public override void SetRelativeEndValue(TweenerCore<Quaternion, Quaternion, NoOptions> t)
        {
            t.endValue *= t.startValue;
        }

        /// <summary>INTERNAL: do not use</summary>
        public override void SetChangeValue(TweenerCore<Quaternion, Quaternion, NoOptions> t)
        {
            t.changeValue.x = t.endValue.x - t.startValue.x;
            t.changeValue.y = t.endValue.y - t.startValue.y;
            t.changeValue.z = t.endValue.z - t.startValue.z;
            t.changeValue.w = t.endValue.w - t.startValue.w;
        }

        /// <summary>INTERNAL: do not use</summary>
        public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, Quaternion changeValue)
        {
            return changeValue.eulerAngles.magnitude / unitsXSecond;
        }

        /// <summary>INTERNAL: do not use</summary>
        public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, float elapsed, Quaternion startValue, Quaternion changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
//            if (t.loopType == LoopType.Incremental) startValue *= changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
//            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
//                startValue += changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
//                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
//            }
            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            startValue.x += changeValue.x * easeVal;
            startValue.y += changeValue.y * easeVal;
            startValue.z += changeValue.z * easeVal;
            startValue.w += changeValue.w * easeVal;
            setter(startValue);
        }
    }
}