#if COMPATIBLE
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/20 18:17

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
    public class ColorWrapperPlugin : ABSTweenPlugin<ColorWrapper, ColorWrapper, ColorOptions>
    {
        public override void Reset(TweenerCore<ColorWrapper, ColorWrapper, ColorOptions> t) { }

        public override void SetFrom(TweenerCore<ColorWrapper, ColorWrapper, ColorOptions> t, bool isRelative)
        {
            Color prevEndVal = t.endValue;
            t.endValue.value = t.getter().value;
            t.startValue.value = isRelative ? t.endValue.value + prevEndVal : prevEndVal;
            Color to = t.endValue.value;
            if (!t.plugOptions.alphaOnly) to = t.startValue.value;
            else to.a = t.startValue.value.a;
            t.setter(to);
        }

        public override ColorWrapper ConvertToStartValue(TweenerCore<ColorWrapper, ColorWrapper, ColorOptions> t, ColorWrapper value)
        {
            return value.value;
        }

        public override void SetRelativeEndValue(TweenerCore<ColorWrapper, ColorWrapper, ColorOptions> t)
        {
            t.endValue.value += t.startValue.value;
        }

        public override void SetChangeValue(TweenerCore<ColorWrapper, ColorWrapper, ColorOptions> t)
        {
            t.changeValue.value = t.endValue.value - t.startValue.value;
        }

        public override float GetSpeedBasedDuration(ColorOptions options, float unitsXSecond, ColorWrapper changeValue)
        {
            return 1f / unitsXSecond;
        }

        public override void EvaluateAndApply(ColorOptions options, Tween t, bool isRelative, DOGetter<ColorWrapper> getter, DOSetter<ColorWrapper> setter, float elapsed, ColorWrapper startValue, ColorWrapper changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
        {
            if (t.loopType == LoopType.Incremental) startValue.value += changeValue.value * (t.isComplete ? t.completedLoops - 1 : t.completedLoops);
            if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental) {
                startValue.value += changeValue.value * (t.loopType == LoopType.Incremental ? t.loops : 1)
                    * (t.sequenceParent.isComplete ? t.sequenceParent.completedLoops - 1 : t.sequenceParent.completedLoops);
            }

            float easeVal = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
            if (!options.alphaOnly) {
                startValue.value.r += changeValue.value.r * easeVal;
                startValue.value.g += changeValue.value.g * easeVal;
                startValue.value.b += changeValue.value.b * easeVal;
                startValue.value.a += changeValue.value.a * easeVal;
                setter(startValue);
                return;
            }

            // Alpha only
            Color res = getter().value;
            res.a = startValue.value.a + changeValue.value.a * easeVal;
            setter(res);
        }
    }
}
#endif