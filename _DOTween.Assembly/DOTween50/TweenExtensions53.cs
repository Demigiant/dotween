// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/06/01 10:49
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using DG.Tweening.CustomYieldInstructions;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Methods that extend Tween objects and allow to control or get data from them.
    /// These require at least Unity 5.3
    /// </summary>
    public static class TweenExtensions53
    {
        #region Custom Yield Instructions

        /// <summary>
        /// Returns a <see cref="CustomYieldInstruction"/> that waits until the tween is killed or complete.
        /// It can be used inside a coroutine as a yield.
        /// <para>Example usage:</para><code>yield return myTween.WaitForCompletion(true);</code>
        /// </summary>
        public static CustomYieldInstruction WaitForCompletion(this Tween t, bool returnCustomYieldInstruction)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            return new DOTweenCYInstruction.WaitForCompletion(t);
        }

        /// <summary>
        /// Returns a <see cref="CustomYieldInstruction"/> that waits until the tween is killed or rewinded.
        /// It can be used inside a coroutine as a yield.
        /// <para>Example usage:</para><code>yield return myTween.WaitForRewind();</code>
        /// </summary>
        public static CustomYieldInstruction WaitForRewind(this Tween t, bool returnCustomYieldInstruction)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            return new DOTweenCYInstruction.WaitForRewind(t);
        }

        /// <summary>
        /// Returns a <see cref="CustomYieldInstruction"/> that waits until the tween is killed.
        /// It can be used inside a coroutine as a yield.
        /// <para>Example usage:</para><code>yield return myTween.WaitForKill();</code>
        /// </summary>
        public static CustomYieldInstruction WaitForKill(this Tween t, bool returnCustomYieldInstruction)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            return new DOTweenCYInstruction.WaitForKill(t);
        }

        /// <summary>
        /// Returns a <see cref="CustomYieldInstruction"/> that waits until the tween is killed or has gone through the given amount of loops.
        /// It can be used inside a coroutine as a yield.
        /// <para>Example usage:</para><code>yield return myTween.WaitForElapsedLoops(2);</code>
        /// </summary>
        /// <param name="elapsedLoops">Elapsed loops to wait for</param>
        public static CustomYieldInstruction WaitForElapsedLoops(this Tween t, int elapsedLoops, bool returnCustomYieldInstruction)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            return new DOTweenCYInstruction.WaitForElapsedLoops(t, elapsedLoops);
        }

        /// <summary>
        /// Returns a <see cref="CustomYieldInstruction"/> that waits until the tween is killed or has reached the given position (loops included, delays excluded).
        /// It can be used inside a coroutine as a yield.
        /// <para>Example usage:</para><code>yield return myTween.WaitForPosition(2.5f);</code>
        /// </summary>
        /// <param name="position">Position (loops included, delays excluded) to wait for</param>
        public static CustomYieldInstruction WaitForPosition(this Tween t, float position, bool returnCustomYieldInstruction)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            return new DOTweenCYInstruction.WaitForPosition(t, position);
        }

        /// <summary>
        /// Returns a <see cref="CustomYieldInstruction"/> that waits until the tween is killed or started
        /// (meaning when the tween is set in a playing state the first time, after any eventual delay).
        /// It can be used inside a coroutine as a yield.
        /// <para>Example usage:</para><code>yield return myTween.WaitForStart();</code>
        /// </summary>
        public static CustomYieldInstruction WaitForStart(this Tween t, bool returnCustomYieldInstruction)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            return new DOTweenCYInstruction.WaitForStart(t);
        }

        #endregion
    }
}