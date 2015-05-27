// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/12/25 12:40

using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
    internal class Color2Plugin : ABSTweenPlugin<Color2, Color2, ColorOptions>
    {
        public override void Reset(TweenerCore<Color2, Color2, ColorOptions> t) { }

        public override void SetFrom(TweenerCore<Color2, Color2, ColorOptions> t, bool isRelative)
        {
            Color2 prevEndVal = t.endValue;
            t.endValue = t.getter();
            if (isRelative) t.startValue = new Color2(t.endValue.ca + prevEndVal.ca, t.endValue.cb + prevEndVal.cb);
            else t.startValue = new Color2(prevEndVal.ca, prevEndVal.cb);
            Color2 to = t.endValue;
            if (!t.plugOptions.alphaOnly) to = t.startValue;
            else {
                to.ca.a = t.startValue.ca.a;
                to.cb.a = t.startValue.cb.a;
            }
            t.setter(to);
        }

        public override Color2 ConvertToStartValue(TweenerCore<Color2, Color2, ColorOptions> t, Color2 value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<Color2, Color2, ColorOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<Color2, Color2, ColorOptions> t)
        {
            t.changeValue = t.endValue - t.startValue;
        }

        public override float GetSpeedBasedDuration(ColorOptions options, float unitsXSecond, Color2 changeValue)
        {
            return 1f / unitsXSecond;
        }

        public override void EvaluateAndApply(ColorOptions options, Tween t, bool isRelative, DOGetter<Color2> getter, DOSetter<Color2> setter, float elapsed, Color2 startValue, Color2 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            if (t.loopType == LoopType.Incremental) startValue += changeValue * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue += changeValue * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            if (!options.alphaOnly) {
                startValue.ca.r += changeValue.ca.r * easeVal;
                startValue.ca.g += changeValue.ca.g * easeVal;
                startValue.ca.b += changeValue.ca.b * easeVal;
                startValue.ca.a += changeValue.ca.a * easeVal;
                startValue.cb.r += changeValue.cb.r * easeVal;
                startValue.cb.g += changeValue.cb.g * easeVal;
                startValue.cb.b += changeValue.cb.b * easeVal;
                startValue.cb.a += changeValue.cb.a * easeVal;
                setter(startValue);
                return;
            }

            // Alpha only
            Color2 res = getter();
            res.ca.a = startValue.ca.a + changeValue.ca.a * easeVal;
            res.cb.a = startValue.cb.a + changeValue.cb.a * easeVal;
            setter(res);
        }
    }
}