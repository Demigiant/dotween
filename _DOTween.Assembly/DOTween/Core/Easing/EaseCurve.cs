// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2012/11/07 13:46
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Easing
{
    /// <summary>
    /// Used to interpret AnimationCurves as eases.
    /// Public so it can be used by external ease factories
    /// </summary>
    public class EaseCurve
    {
        readonly AnimationCurve _animCurve;

        // ***********************************************************************************
        // CONSTRUCTOR
        // ***********************************************************************************

        public EaseCurve(AnimationCurve animCurve)
        {
            _animCurve = animCurve;
        }

        // ===================================================================================
        // PUBLIC METHODS --------------------------------------------------------------------

        public float Evaluate(float time, float duration, float unusedOvershoot, float unusedPeriod)
        {
            float curveLen = _animCurve[_animCurve.length - 1].time;
            float timePerc = time / duration;
            float eval = _animCurve.Evaluate(timePerc * curveLen);
            return eval;
        }
    }
}