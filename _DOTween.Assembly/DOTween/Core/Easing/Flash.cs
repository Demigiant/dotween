// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/10/31 15:59
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core.Easing
{
    public static class Flash
    {
        public static float Ease(float time, float duration, float overshootOrAmplitude, float period)
        {
            int stepIndex = Mathf.CeilToInt((time / duration) * overshootOrAmplitude); // 1 to overshootOrAmplitude
            float stepDuration = duration / overshootOrAmplitude;
            time -= stepDuration * (stepIndex - 1);
            float dir = (stepIndex % 2 != 0) ? 1 : -1;
            if (dir < 0) time -= stepDuration;
            float res = (time * dir) / stepDuration;
            return WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
        }

        public static float EaseIn(float time, float duration, float overshootOrAmplitude, float period)
        {
            int stepIndex = Mathf.CeilToInt((time / duration) * overshootOrAmplitude); // 1 to overshootOrAmplitude
            float stepDuration = duration / overshootOrAmplitude;
            time -= stepDuration * (stepIndex - 1);
            float dir = (stepIndex % 2 != 0) ? 1 : -1;
            if (dir < 0) time -= stepDuration;
            time = time * dir;
            float res = (time /= stepDuration) * time;
            return WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
        }

        public static float EaseOut(float time, float duration, float overshootOrAmplitude, float period)
        {
            int stepIndex = Mathf.CeilToInt((time / duration) * overshootOrAmplitude); // 1 to overshootOrAmplitude
            float stepDuration = duration / overshootOrAmplitude;
            time -= stepDuration * (stepIndex - 1);
            float dir = (stepIndex % 2 != 0) ? 1 : -1;
            if (dir < 0) time -= stepDuration;
            time = time * dir;
            float res = -(time /= stepDuration) * (time - 2);
            return WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
        }

        public static float EaseInOut(float time, float duration, float overshootOrAmplitude, float period)
        {
            int stepIndex = Mathf.CeilToInt((time / duration) * overshootOrAmplitude); // 1 to overshootOrAmplitude
            float stepDuration = duration / overshootOrAmplitude;
            time -= stepDuration * (stepIndex - 1);
            float dir = (stepIndex % 2 != 0) ? 1 : -1;
            if (dir < 0) time -= stepDuration;
            time = time * dir;
            float res = (time /= stepDuration * 0.5f) < 1
                ? 0.5f * time * time
                : -0.5f * ((--time) * (time - 2) - 1);
            return WeightedEase(overshootOrAmplitude, period, stepIndex, stepDuration, dir, res);
        }

        static float WeightedEase(float overshootOrAmplitude, float period, int stepIndex, float stepDuration, float dir, float res)
        {
            float easedRes = 0;
            float finalDecimals = 0;
            // Use previous stepIndex in case of odd ones, so that back ease is not clamped
            if (dir > 0 && (int)overshootOrAmplitude % 2 == 0) stepIndex++;
            else if (dir < 0 && (int)overshootOrAmplitude % 2 != 0) stepIndex++;

            if (period > 0) {
                float finalTruncated = (float)Math.Truncate(overshootOrAmplitude);
                finalDecimals = overshootOrAmplitude - finalTruncated;
                if (finalTruncated % 2 > 0) finalDecimals = 1 - finalDecimals;
                finalDecimals = (finalDecimals * stepIndex) / overshootOrAmplitude;
                easedRes = (res * (overshootOrAmplitude - stepIndex)) / overshootOrAmplitude;
            } else if (period < 0) {
                period = -period;
                easedRes = (res * stepIndex) / overshootOrAmplitude;
            }
            float diff = easedRes - res;
            res += (diff * period) + finalDecimals;
            if (res > 1) res = 1;
            return res;
        }
    }
}