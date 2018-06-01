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
        /// <para>Example usage:</para><code>yield return myTween.WaitForCompletionCY();</code>
        /// </summary>
        public static CustomYieldInstruction WaitForCompletionCY(this Tween t)
        {
            if (!t.active) {
                if (Debugger.logPriority > 0) Debugger.LogInvalidTween(t);
                return null;
            }

            return new WaitForCompletion(t);
        }

        #endregion
    }
}