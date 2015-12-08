// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/19 14:11
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php
// 
// =============================================================
// Contains Daniele Giardini's C# port of the easing equations created by Robert Penner
// (all easing equations except for Flash, InFlash, OutFlash, InOutFlash,
// which use some parts of Robert Penner's equations but were created by Daniele Giardini)
// http://robertpenner.com/easing, see license below:
// =============================================================
//
// TERMS OF USE - EASING EQUATIONS
//
// Open source under the BSD License.
//
// Copyright © 2001 Robert Penner
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must retain the above copyright notice,
// this list of conditions and the following disclaimer.
// - Redistributions in binary form must reproduce the above copyright notice,
// this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// - Neither the name of the author nor the names of contributors may be used to endorse
// or promote products derived from this software without specific prior written permission.
// - THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
// EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Easing
{
    public static class EaseManager
    {
        const float _PiOver2 = Mathf.PI * 0.5f;
        const float _TwoPi = Mathf.PI * 2;

        /// <summary>
        /// Returns a value between 0 and 1 (inclusive) based on the elapsed time and ease selected
        /// </summary>
        public static float Evaluate(Tween t, float time, float duration, float overshootOrAmplitude, float period)
        {
            // Overload used only to allow custom user plugins to avoid calling t.easeType and t.customEase since they're internal
            return Evaluate(t.easeType, t.customEase, time, duration, overshootOrAmplitude, period);
        }

        /// <summary>
        /// Returns a value between 0 and 1 (inclusive) based on the elapsed time and ease selected
        /// </summary>
        public static float Evaluate(Ease easeType, EaseFunction customEase, float time, float duration, float overshootOrAmplitude, float period)
        {
            switch (easeType) {
            case Ease.Linear:
                return time / duration;
            case Ease.InSine:
                return -(float)Math.Cos(time / duration * _PiOver2) + 1;
            case Ease.OutSine:
                return (float)Math.Sin(time / duration * _PiOver2);
            case Ease.InOutSine:
                return -0.5f * ((float)Math.Cos(Mathf.PI * time / duration) - 1);
            case Ease.InQuad:
                return (time /= duration) * time;
            case Ease.OutQuad:
                return -(time /= duration) * (time - 2);
            case Ease.InOutQuad:
                if ((time /= duration * 0.5f) < 1) return 0.5f * time * time;
                return -0.5f * ((--time) * (time - 2) - 1);
            case Ease.InCubic:
                return (time /= duration) * time * time;
            case Ease.OutCubic:
                return ((time = time / duration - 1) * time * time + 1);
            case Ease.InOutCubic:
                if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time;
                return 0.5f * ((time -= 2) * time * time + 2);
            case Ease.InQuart:
                return (time /= duration) * time * time * time;
            case Ease.OutQuart:
                return -((time = time / duration - 1) * time * time * time - 1);
            case Ease.InOutQuart:
                if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time * time;
                return -0.5f * ((time -= 2) * time * time * time - 2);
            case Ease.InQuint:
                return (time /= duration) * time * time * time * time;
            case Ease.OutQuint:
                return ((time = time / duration - 1) * time * time * time * time + 1);
            case Ease.InOutQuint:
                if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time * time * time;
                return 0.5f * ((time -= 2) * time * time * time * time + 2);
            case Ease.InExpo:
                return (time == 0) ? 0 : (float)Math.Pow(2, 10 * (time / duration - 1));
            case Ease.OutExpo:
                if (time == duration) return 1;
                return (-(float)Math.Pow(2, -10 * time / duration) + 1);
            case Ease.InOutExpo:
                if (time == 0) return 0;
                if (time == duration) return 1;
                if ((time /= duration * 0.5f) < 1) return 0.5f * (float)Math.Pow(2, 10 * (time - 1));
                return 0.5f * (-(float)Math.Pow(2, -10 * --time) + 2);
            case Ease.InCirc:
                return -((float)Math.Sqrt(1 - (time /= duration) * time) - 1);
            case Ease.OutCirc:
                return (float)Math.Sqrt(1 - (time = time / duration - 1) * time);
            case Ease.InOutCirc:
                if ((time /= duration * 0.5f) < 1) return -0.5f * ((float)Math.Sqrt(1 - time * time) - 1);
                return 0.5f * ((float)Math.Sqrt(1 - (time -= 2) * time) + 1);
            case Ease.InElastic:
                float s0;
                if (time == 0) return 0;
                if ((time /= duration) == 1) return 1;
                if (period == 0) period = duration * 0.3f;
                if (overshootOrAmplitude < 1) {
                    overshootOrAmplitude = 1;
                    s0 = period / 4;
                } else s0 = period / _TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                return -(overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s0) * _TwoPi / period));
            case Ease.OutElastic:
                float s1;
                if (time == 0) return 0;
                if ((time /= duration) == 1) return 1;
                if (period == 0) period = duration * 0.3f;
                if (overshootOrAmplitude < 1) {
                    overshootOrAmplitude = 1;
                    s1 = period / 4;
                } else s1 = period / _TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                return (overshootOrAmplitude * (float)Math.Pow(2, -10 * time) * (float)Math.Sin((time * duration - s1) * _TwoPi / period) + 1);
            case Ease.InOutElastic:
                float s;
                if (time == 0) return 0;
                if ((time /= duration * 0.5f) == 2) return 1;
                if (period == 0) period = duration * (0.3f * 1.5f);
                if (overshootOrAmplitude < 1) {
                    overshootOrAmplitude = 1;
                    s = period / 4;
                } else s = period / _TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                if (time < 1) return -0.5f * (overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s) * _TwoPi / period));
                return overshootOrAmplitude * (float)Math.Pow(2, -10 * (time -= 1)) * (float)Math.Sin((time * duration - s) * _TwoPi / period) * 0.5f + 1;
            case Ease.InBack:
                return (time /= duration) * time * ((overshootOrAmplitude + 1) * time - overshootOrAmplitude);
            case Ease.OutBack:
                return ((time = time / duration - 1) * time * ((overshootOrAmplitude + 1) * time + overshootOrAmplitude) + 1);
            case Ease.InOutBack:
                if ((time /= duration * 0.5f) < 1) return 0.5f * (time * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time - overshootOrAmplitude));
                return 0.5f * ((time -= 2) * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time + overshootOrAmplitude) + 2);
            case Ease.InBounce:
                return Bounce.EaseIn(time, duration, overshootOrAmplitude, period);
            case Ease.OutBounce:
                return Bounce.EaseOut(time, duration, overshootOrAmplitude, period);
            case Ease.InOutBounce:
                return Bounce.EaseInOut(time, duration, overshootOrAmplitude, period);
            case Ease.INTERNAL_Custom:
                return customEase(time, duration, overshootOrAmplitude, period);
            case Ease.INTERNAL_Zero:
                // 0 duration tween
                return 1;

            // Extra custom eases ////////////////////////////////////////////////////
            case Ease.Flash:
                return Flash.Ease(time, duration, overshootOrAmplitude, period);
            case Ease.InFlash:
                return Flash.EaseIn(time, duration, overshootOrAmplitude, period);
            case Ease.OutFlash:
                return Flash.EaseOut(time, duration, overshootOrAmplitude, period);
            case Ease.InOutFlash:
                return Flash.EaseInOut(time, duration, overshootOrAmplitude, period);

            // Default
            default:
                // OutQuad
                return -(time /= duration) * (time - 2);
            }
        }

        public static EaseFunction ToEaseFunction(Ease ease)
        {
            switch (ease) {
            case Ease.Linear:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    time / duration;
            case Ease.InSine:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    -(float)Math.Cos(time / duration * _PiOver2) + 1;
            case Ease.OutSine:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    (float)Math.Sin(time / duration * _PiOver2);
            case Ease.InOutSine:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    -0.5f * ((float)Math.Cos(Mathf.PI * time / duration) - 1);
            case Ease.InQuad:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    (time /= duration) * time;
            case Ease.OutQuad:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    -(time /= duration) * (time - 2);
            case Ease.InOutQuad:
                return (float time, float duration, float overshootOrAmplitude, float period) => {
                    if ((time /= duration * 0.5f) < 1) return 0.5f * time * time;
                    return -0.5f * ((--time) * (time - 2) - 1);
                };
            case Ease.InCubic:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    (time /= duration) * time * time;
            case Ease.OutCubic:
                return (float time, float duration, float overshootOrAmplitude, float period) => 
                    ((time = time / duration - 1) * time * time + 1);
            case Ease.InOutCubic:
                return (float time, float duration, float overshootOrAmplitude, float period) => {
                    if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time;
                    return 0.5f * ((time -= 2) * time * time + 2);
                };
            case Ease.InQuart:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    (time /= duration) * time * time * time;
            case Ease.OutQuart:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    -((time = time / duration - 1) * time * time * time - 1);
            case Ease.InOutQuart:
                return (float time, float duration, float overshootOrAmplitude, float period) => {
                    if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time * time;
                    return -0.5f * ((time -= 2) * time * time * time - 2);
                };
            case Ease.InQuint:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    (time /= duration) * time * time * time * time;
            case Ease.OutQuint:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    ((time = time / duration - 1) * time * time * time * time + 1);
            case Ease.InOutQuint:
                return (float time, float duration, float overshootOrAmplitude, float period) => {
                    if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time * time * time;
                    return 0.5f * ((time -= 2) * time * time * time * time + 2);
                };
            case Ease.InExpo:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    (time == 0) ? 0 : (float)Math.Pow(2, 10 * (time / duration - 1));
            case Ease.OutExpo:
                return (float time, float duration, float overshootOrAmplitude, float period) => {
                    if (time == duration) return 1;
                    return (-(float)Math.Pow(2, -10 * time / duration) + 1);
                };
            case Ease.InOutExpo:
                return (float time, float duration, float overshootOrAmplitude, float period) => {
                    if (time == 0) return 0;
                    if (time == duration) return 1;
                    if ((time /= duration * 0.5f) < 1) return 0.5f * (float)Math.Pow(2, 10 * (time - 1));
                    return 0.5f * (-(float)Math.Pow(2, -10 * --time) + 2);
                };
            case Ease.InCirc:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    -((float)Math.Sqrt(1 - (time /= duration) * time) - 1);
            case Ease.OutCirc:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    (float)Math.Sqrt(1 - (time = time / duration - 1) * time);
            case Ease.InOutCirc:
                return (float time, float duration, float overshootOrAmplitude, float period) => {
                    if ((time /= duration * 0.5f) < 1) return -0.5f * ((float)Math.Sqrt(1 - time * time) - 1);
                    return 0.5f * ((float)Math.Sqrt(1 - (time -= 2) * time) + 1);
                };
            case Ease.InElastic:
                return (float time, float duration, float overshootOrAmplitude, float period) => {
                    float s0;
                    if (time == 0) return 0;
                    if ((time /= duration) == 1) return 1;
                    if (period == 0) period = duration * 0.3f;
                    if (overshootOrAmplitude < 1) {
                        overshootOrAmplitude = 1;
                        s0 = period / 4;
                    } else s0 = period / _TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                    return -(overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s0) * _TwoPi / period));
                };
            case Ease.OutElastic:
                return (float time, float duration, float overshootOrAmplitude, float period) => {
                    float s1;
                    if (time == 0) return 0;
                    if ((time /= duration) == 1) return 1;
                    if (period == 0) period = duration * 0.3f;
                    if (overshootOrAmplitude < 1) {
                        overshootOrAmplitude = 1;
                        s1 = period / 4;
                    } else s1 = period / _TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                    return (overshootOrAmplitude * (float)Math.Pow(2, -10 * time) * (float)Math.Sin((time * duration - s1) * _TwoPi / period) + 1);
                };
            case Ease.InOutElastic:
                return (float time, float duration, float overshootOrAmplitude, float period) => {
                    float s;
                    if (time == 0) return 0;
                    if ((time /= duration * 0.5f) == 2) return 1;
                    if (period == 0) period = duration * (0.3f * 1.5f);
                    if (overshootOrAmplitude < 1) {
                        overshootOrAmplitude = 1;
                        s = period / 4;
                    } else s = period / _TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                    if (time < 1) return -0.5f * (overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s) * _TwoPi / period));
                    return overshootOrAmplitude * (float)Math.Pow(2, -10 * (time -= 1)) * (float)Math.Sin((time * duration - s) * _TwoPi / period) * 0.5f + 1;
                };
            case Ease.InBack:
                return (float time, float duration, float overshootOrAmplitude, float period) => 
                    (time /= duration) * time * ((overshootOrAmplitude + 1) * time - overshootOrAmplitude);
            case Ease.OutBack:
                return (float time, float duration, float overshootOrAmplitude, float period) => 
                    ((time = time / duration - 1) * time * ((overshootOrAmplitude + 1) * time + overshootOrAmplitude) + 1);
            case Ease.InOutBack:
                return (float time, float duration, float overshootOrAmplitude, float period) => {
                    if ((time /= duration * 0.5f) < 1) return 0.5f * (time * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time - overshootOrAmplitude));
                    return 0.5f * ((time -= 2) * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time + overshootOrAmplitude) + 2);
                };
            case Ease.InBounce:
                return (float time, float duration, float overshootOrAmplitude, float period) => 
                    Bounce.EaseIn(time, duration, overshootOrAmplitude, period);
            case Ease.OutBounce:
                return (float time, float duration, float overshootOrAmplitude, float period) => 
                    Bounce.EaseOut(time, duration, overshootOrAmplitude, period);
            case Ease.InOutBounce:
                return (float time, float duration, float overshootOrAmplitude, float period) => 
                    Bounce.EaseInOut(time, duration, overshootOrAmplitude, period);

            // Extra custom eases ////////////////////////////////////////////////////
            case Ease.Flash:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    Flash.Ease(time, duration, overshootOrAmplitude, period);
            case Ease.InFlash:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    Flash.EaseIn(time, duration, overshootOrAmplitude, period);
            case Ease.OutFlash:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    Flash.EaseOut(time, duration, overshootOrAmplitude, period);
            case Ease.InOutFlash:
                return (float time, float duration, float overshootOrAmplitude, float period) =>
                    Flash.EaseInOut(time, duration, overshootOrAmplitude, period);

            // Default
            default:
                // OutQuad
                return (float time, float duration, float overshootOrAmplitude, float period) => -(time /= duration) * (time - 2);
            }
        }

        internal static bool IsFlashEase(Ease ease)
        {
            switch (ease) {
            case Ease.Flash:
            case Ease.InFlash:
            case Ease.OutFlash:
            case Ease.InOutFlash:
                return true;
            }
            return false;
        }
    }
}