// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2026/01/08

#if DOTWEEN_UITOOLKIT && UNITY_2021_3_OR_NEWER // MODULE_MARKER

using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;

#pragma warning disable 1591
namespace DG.Tweening
{
	public static class DOTweenModuleUIToolkit
    {
        #region Shortcuts

        #region VisualElement

        /// <summary>Tweens a VisualElement's position (via style.translate) to the given value.
        /// Also stores the VisualElement as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOMove(this VisualElement target, Vector3 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t
                = DOTween.To(() => target.resolvedStyle.translate, x => target.style.translate = new Translate(x.x, x.y, x.z), endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        /// <summary>Tweens a VisualElement's position (via style.translate) to the given value.
        /// Also stores the VisualElement as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOMove(this VisualElement target, Vector2 endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t
                = DOTween.To(() => (Vector2)target.resolvedStyle.translate, x => target.style.translate = new Translate(x.x, x.y, 0), endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        /// <summary>Tweens a VisualElement's X position (via style.translate) to the given value.
        /// Also stores the VisualElement as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveX(this VisualElement target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t
                = DOTween.To(() => target.resolvedStyle.translate, x => target.style.translate = new Translate(x.x, x.y, x.z), new Vector3(endValue, 0, 0), duration);
            t.SetOptions(AxisConstraint.X, snapping).SetTarget(target);
            return t;
        }
        /// <summary>Tweens a VisualElement's Y position (via style.translate) to the given value.
        /// Also stores the VisualElement as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveY(this VisualElement target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t
                = DOTween.To(() => target.resolvedStyle.translate, x => target.style.translate = new Translate(x.x, x.y, x.z), new Vector3(0, endValue, 0), duration);
            t.SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
            return t;
        }
        /// <summary>Tweens a VisualElement's Z position (via style.translate) to the given value.
        /// Also stores the VisualElement as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveZ(this VisualElement target, float endValue, float duration, bool snapping = false)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t
                = DOTween.To(() => target.resolvedStyle.translate, x => target.style.translate = new Translate(x.x, x.y, x.z), new Vector3(0, 0, endValue), duration);
            t.SetOptions(AxisConstraint.Z, snapping).SetTarget(target);
            return t;
        }
        
        /// <summary>Tweens a VisualElement's scale (via style.scale) to the given value.
        /// Also stores the VisualElement as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOScale(this VisualElement target, Vector2 endValue, float duration)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t
                = DOTween.To(() => (Vector2)target.resolvedStyle.scale.value, x => target.style.scale = new Scale(x), endValue, duration);
            t.SetTarget(target);
            return t;
        }
        /// <summary>Tweens a VisualElement's scale (via style.scale) to the given value.
        /// Also stores the VisualElement as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOScale(this VisualElement target, float endValue, float duration)
        {
            TweenerCore<Vector2, Vector2, VectorOptions> t
                = DOTween.To(() => (Vector2)target.resolvedStyle.scale.value, x => target.style.scale = new Scale(x), new Vector2(endValue, endValue), duration);
            t.SetTarget(target);
            return t;
        }
        
        /// <summary>Tweens a VisualElement's rotation (via style.rotate) to the given value.
        /// Also stores the VisualElement as the tween's target so it can be used for filtered operations</summary>
        /// <param name="endValue">The end value to reach</param><param name="duration">The duration of the tween</param>
        public static TweenerCore<float, float, FloatOptions> DORotate(this VisualElement target, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t
                = DOTween.To(() => target.resolvedStyle.rotate.angle.value, x => target.style.rotate = new Rotate(x), endValue, duration);
            t.SetTarget(target);
            return t;
        }
        
        /// <summary>Punches a VisualElement's position towards the given direction and then back to the starting one
        /// as if it was connected to the starting position via an elastic.
        /// Also stores the VisualElement as the tween's target so it can be used for filtered operations</summary>
        /// <param name="punch">The direction and strength of the punch (added to the VisualElement's current position)</param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="vibrato">Indicates how much will the punch vibrate</param>
        /// <param name="elasticity">Represents how much (0 to 1) the vector will go beyond the starting position when bouncing backwards.
        /// 1 creates a full oscillation between the punch direction and the opposite direction,
        /// while 0 oscillates only between the punch and the start position</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener DOPunch(this VisualElement target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1, bool snapping = false)
        {
            return DOTween.Punch(() => target.resolvedStyle.translate, x => target.style.translate = new Translate(x.x, x.y, x.z), punch, duration, vibrato, elasticity)
                .SetTarget(target).SetOptions(snapping);
        }
        
        /// <summary>Shakes a VisualElement's position with the given values.
        /// Also stores the VisualElement as the tween's target so it can be used for filtered operations</summary>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="strength">The shake strength</param>
        /// <param name="vibrato">Indicates how much will the shake vibrate</param>
        /// <param name="randomness">Indicates how much the shake will be random (0 to 180 - values higher than 90 kind of suck, so beware). 
        /// Setting it to 0 will shake along a single direction.</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        /// <param name="fadeOut">If TRUE the shake will automatically fadeOut smoothly within the tween's duration, otherwise it will not</param>
        /// <param name="randomnessMode">Randomness mode</param>
        public static Tweener DOShake(this VisualElement target, float duration, float strength = 100, int vibrato = 10, float randomness = 90, bool snapping = false, bool fadeOut = true, ShakeRandomnessMode randomnessMode = ShakeRandomnessMode.Full)
        {
            return DOTween.Shake(() => target.resolvedStyle.translate, x => target.style.translate = new Translate(x.x, x.y, x.z), duration, strength, vibrato, randomness, true, fadeOut, randomnessMode)
                .SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake).SetOptions(snapping);
        }
        /// <summary>Shakes a VisualElement's position with the given values.
        /// Also stores the VisualElement as the tween's target so it can be used for filtered operations</summary>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="strength">The shake strength on each axis</param>
        /// <param name="vibrato">Indicates how much will the shake vibrate</param>
        /// <param name="randomness">Indicates how much the shake will be random (0 to 180 - values higher than 90 kind of suck, so beware). 
        /// Setting it to 0 will shake along a single direction.</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        /// <param name="fadeOut">If TRUE the shake will automatically fadeOut smoothly within the tween's duration, otherwise it will not</param>
        /// <param name="randomnessMode">Randomness mode</param>
        public static Tweener DOShake(this VisualElement target, float duration, Vector2 strength, int vibrato = 10, float randomness = 90, bool snapping = false, bool fadeOut = true, ShakeRandomnessMode randomnessMode = ShakeRandomnessMode.Full)
        {
            return DOTween.Shake(() => target.resolvedStyle.translate, x => target.style.translate = new Translate(x.x, x.y, x.z), duration, strength, vibrato, randomness, fadeOut, randomnessMode)
                .SetTarget(target).SetSpecialStartupMode(SpecialStartupMode.SetShake).SetOptions(snapping);
        }

        #endregion

        #endregion
	}
}
#endif
