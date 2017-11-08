// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2017/11/08 12:28
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php
namespace DG.Tweening.Core.Enums
{
    /// <summary>
    /// OnRewind callback behaviour (can only be set via DOTween's Utility Panel)
    /// </summary>
    public enum RewindCallbackMode
    {
        /// <summary>
        /// When calling Rewind or PlayBackwards/SmoothRewind, OnRewind callbacks will be fired only if the tween isn't already rewinded
        /// </summary>
        FireIfPositionChanged,
        /// <summary>
        /// When calling Rewind, OnRewind callbacks will always be fired, even if the tween is already rewinded.
        /// When calling PlayBackwards/SmoothRewind instead, OnRewind callbacks will be fired only if the tween isn't already rewinded
        /// </summary>
        FireAlwaysWithRewind,
        /// <summary>
        /// When calling Rewind or PlayBackwards/SmoothRewind, OnRewind callbacks will always be fired, even if the tween is already rewinded
        /// </summary>
        FireAlways,
    }
}