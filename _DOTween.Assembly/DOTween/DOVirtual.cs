// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/01/29 12:57

using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Creates virtual tweens that can be used to change other elements via their OnUpdate calls
    /// </summary>
    public static class DOVirtual
    {
        #region Virtual Tweens

        /// <summary>
        /// Tweens a virtual float.
        /// You can add regular settings to the generated tween,
        /// but do not use <code>SetUpdate</code> or you will overwrite the onVirtualUpdate parameter
        /// </summary>
        /// <param name="from">The value to start from</param>
        /// <param name="to">The value to tween to</param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="onVirtualUpdate">A callback which must accept a parameter of type float, called at each update</param>
        /// <returns></returns>
        public static Tweener Float(float from, float to, float duration, TweenCallback<float> onVirtualUpdate)
        {
            float val = from;
            return DOTween.To(() => val, x => val = x, to, duration).OnUpdate(() => onVirtualUpdate(val));
        }

        #endregion

        #region Virtual Functions

        /// <summary>Returns a value based on the given ease and lifetime percentage (0 to 1)</summary>
        /// <param name="from">The value to start from when lifetimePercentage is 0</param>
        /// <param name="to">The value to reach when lifetimePercentage is 1</param>
        /// <param name="lifetimePercentage">The time percentage (0 to 1) at which the value should be taken</param>
        /// <param name="easeType">The type of ease</param>
        public static float EasedValue(float from, float to, float lifetimePercentage, Ease easeType)
        {
            return from + (to - from) * EaseManager.Evaluate(easeType, null, lifetimePercentage, 1, DOTween.defaultEaseOvershootOrAmplitude, DOTween.defaultEasePeriod);
        }
        /// <summary>Returns a value based on the given ease and lifetime percentage (0 to 1)</summary>
        /// <param name="from">The value to start from when lifetimePercentage is 0</param>
        /// <param name="to">The value to reach when lifetimePercentage is 1</param>
        /// <param name="lifetimePercentage">The time percentage (0 to 1) at which the value should be taken</param>
        /// <param name="easeType">The type of ease</param>
        /// <param name="overshoot">Eventual overshoot to use with Back ease</param>
        public static float EasedValue(float from, float to, float lifetimePercentage, Ease easeType, float overshoot)
        {
            return from + (to - from) * EaseManager.Evaluate(easeType, null, lifetimePercentage, 1, overshoot, DOTween.defaultEasePeriod);
        }
        /// <summary>Returns a value based on the given ease and lifetime percentage (0 to 1)</summary>
        /// <param name="from">The value to start from when lifetimePercentage is 0</param>
        /// <param name="to">The value to reach when lifetimePercentage is 1</param>
        /// <param name="lifetimePercentage">The time percentage (0 to 1) at which the value should be taken</param>
        /// <param name="easeType">The type of ease</param>
        /// <param name="amplitude">Eventual amplitude to use with Elastic easeType</param>
        /// <param name="period">Eventual period to use with Elastic easeType</param>
        public static float EasedValue(float from, float to, float lifetimePercentage, Ease easeType, float amplitude, float period)
        {
            return from + (to - from) * EaseManager.Evaluate(easeType, null, lifetimePercentage, 1, amplitude, period);
        }
        /// <summary>Returns a value based on the given ease and lifetime percentage (0 to 1)</summary>
        /// <param name="from">The value to start from when lifetimePercentage is 0</param>
        /// <param name="to">The value to reach when lifetimePercentage is 1</param>
        /// <param name="lifetimePercentage">The time percentage (0 to 1) at which the value should be taken</param>
        /// <param name="easeCurve">The AnimationCurve to use for ease</param>
        public static float EasedValue(float from, float to, float lifetimePercentage, AnimationCurve easeCurve)
        {
            return from + (to - from) * EaseManager.Evaluate(Ease.INTERNAL_Custom, new EaseCurve(easeCurve).Evaluate, lifetimePercentage, 1, DOTween.defaultEaseOvershootOrAmplitude, DOTween.defaultEasePeriod);
        }

        /// <summary>Fires the given callback after the given time.</summary>
        /// <param name="delay">Callback delay</param>
        /// <param name="callback">Callback to fire when the delay has expired</param>
        /// <param name="ignoreTimeScale">If TRUE (default) ignores Unity's timeScale</param>
        public static Tween DelayedCall(float delay, TweenCallback callback, bool ignoreTimeScale = true)
        {
            return DOTween.Sequence().AppendInterval(delay).OnStepComplete(callback).SetUpdate(UpdateType.Normal, ignoreTimeScale).SetAutoKill(true);
        }

        #endregion
    }
}