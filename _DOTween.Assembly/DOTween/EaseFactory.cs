// Author: Andrei Stanescu - https://github.com/reydanro
// Unity Forum Post: http://forum.unity3d.com/threads/dotween-hotween-v2-a-unity-tween-engine.260692/page-24#post-2052957
// Created: 2015/04/04 14:39
// Modified by: Daniele Giardini - http://www.demigiant.com

using DG.Tweening.Core.Easing;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Allows to wrap ease method in special ways, adding extra features
    /// </summary>
    public class EaseFactory
    {
        #region StopMotion

        /// <summary>
        /// Converts the given ease so that it also creates a stop-motion effect, by playing the tween at the given FPS
        /// </summary>
        /// <param name="motionFps">FPS at which the tween should be played</param>
        /// <param name="ease">Ease type</param>
        public static EaseFunction StopMotion(int motionFps, Ease? ease = null)
        {
            EaseFunction easeFunc = EaseManager.ToEaseFunction(ease == null ? DOTween.defaultEaseType : (Ease)ease);
            return StopMotion(motionFps, easeFunc);
        }
        /// <summary>
        /// Converts the given ease so that it also creates a stop-motion effect, by playing the tween at the given FPS
        /// </summary>
        /// <param name="motionFps">FPS at which the tween should be played</param>
        /// <param name="animCurve">AnimationCurve to use for the ease</param>
        public static EaseFunction StopMotion(int motionFps, AnimationCurve animCurve)
        {
            return StopMotion(motionFps, new EaseCurve(animCurve).Evaluate);
        }
        /// <summary>
        /// Converts the given ease so that it also creates a stop-motion effect, by playing the tween at the given FPS
        /// </summary>
        /// <param name="motionFps">FPS at which the tween should be played</param>
        /// <param name="customEase">Custom ease function to use</param>
        public static EaseFunction StopMotion(int motionFps, EaseFunction customEase)
        {
            // Compute the time interval in which we must re-evaluate the value
            float motionDelay = 1.0f / motionFps;

            return delegate(float time, float duration, float overshootOrAmplitude, float period) {
                // Adjust the time so it's in steps
                float steptime = time < duration ? time - (time % motionDelay) : time;
                // Evaluate the ease value based on the new step time
                return customEase(steptime, duration, overshootOrAmplitude, period);
            };
        }

        #endregion
    }
}