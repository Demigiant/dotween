#if !COMPATIBLE
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/10 14:33
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
    public class ColorPlugin : ABSTweenPlugin<Color, Color, ColorOptions>
    {
        public override void Reset(TweenerCore<Color, Color, ColorOptions> t) { }

        public override void SetFrom(TweenerCore<Color, Color, ColorOptions> t, bool isRelative)
        {
            Color prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
            Color to = t.endValue;
            if (!t.plugOptions.alphaOnly) to = t.startValue;
            else to.a = t.startValue.a;
            t.setter(to);
        }

        public override Color ConvertToStartValue(TweenerCore<Color, Color, ColorOptions> t, Color value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Color, Color, ColorOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Color, Color, ColorOptions> t)
        {
            t.changeValue = t.endValue - t.startValue;
        }

        public override float GetSpeedBasedDuration(ColorOptions options, float unitsXSecond, Color changeValue)
        {
            return 1f / unitsXSecond;
        }

        public override void EvaluateAndApply(ColorOptions options, Tween t, bool isRelative, DOGetter<Color> getter, DOSetter<Color> setter, float elapsed, Color startValue, Color changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue += changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            if (!options.alphaOnly) {
                startValue.r += changeValue.r * easeVal;
                startValue.g += changeValue.g * easeVal;
                startValue.b += changeValue.b * easeVal;
                startValue.a += changeValue.a * easeVal;
                setter(startValue);
                return;
            }

            // Alpha only
            Color res = getter();
            res.a = startValue.a + changeValue.a * easeVal;
            setter(res);
        }
    }
}
#endif