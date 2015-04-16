#if WP81
// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/04/15 12:17

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Surrogates;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

#pragma warning disable 1591
namespace DG.Tweening.Plugins
{
    public class ColorSurrogatePlugin : ABSTweenPlugin<ColorSurrogate, ColorSurrogate, ColorOptions>
    {
        public override void Reset(TweenerCore<ColorSurrogate, ColorSurrogate, ColorOptions> t) { }

        public override void SetFrom(TweenerCore<ColorSurrogate, ColorSurrogate, ColorOptions> t, bool isRelative)
        {
            ColorSurrogate prevEndVal = t.endValue;
            t.endValue = t.getter();
            t.startValue = isRelative ? t.endValue + prevEndVal : prevEndVal;
            ColorSurrogate to = t.endValue;
            if (!t.plugOptions.alphaOnly) to = t.startValue;
            else to.a = t.startValue.a;
            t.setter(to);
        }

        public override ColorSurrogate ConvertToStartValue(TweenerCore<ColorSurrogate, ColorSurrogate, ColorOptions> t, ColorSurrogate value)
        {
            return value;
        }

        public override void SetRelativeEndValue(TweenerCore<ColorSurrogate, ColorSurrogate, ColorOptions> t)
        {
            t.endValue += t.startValue;
        }

        public override void SetChangeValue(TweenerCore<ColorSurrogate, ColorSurrogate, ColorOptions> t)
        {
            t.changeValue = t.endValue - t.startValue;
        }

        public override float GetSpeedBasedDuration(ColorOptions options, float unitsXSecond, ColorSurrogate changeValue)
        {
            return 1f / unitsXSecond;
        }

        public override void EvaluateAndApply(ColorOptions options, Tween t, bool isRelative, DOGetter<ColorSurrogate> getter, DOSetter<ColorSurrogate> setter, float elapsed, ColorSurrogate startValue, ColorSurrogate changeValue, float duration, bool usingInversePosition)
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
            ColorSurrogate res = getter();
            res.a = startValue.a + changeValue.a * easeVal;
            setter(res);
        }
    }
}
#endif