// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2026/01/08

#if DOTWEEN_UITOOLKIT && UNITY_2021_3_OR_NEWER // MODULE_MARKER

using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening.Core;
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
                = DOTween.To(() => (Vector2)target.resolvedStyle.translate, x => target.style.translate = x, endValue, duration);
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

        #endregion

        #endregion
	}
}
#endif
