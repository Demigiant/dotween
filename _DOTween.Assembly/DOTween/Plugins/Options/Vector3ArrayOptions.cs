// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/08/21 12:11
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Options
{
    public struct Vector3ArrayOptions : IPlugOptions
    {
        public AxisConstraint axisConstraint;
        public bool snapping;

        internal float[] durations; // Duration of each segment

        public void Reset()
        {
            axisConstraint = AxisConstraint.None;
            snapping = false;
            durations = null;
        }
    }
}