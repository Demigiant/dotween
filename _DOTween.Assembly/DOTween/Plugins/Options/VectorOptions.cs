// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/28 11:23
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Options
{
    public struct VectorOptions : IPlugOptions
    {
        public AxisConstraint axisConstraint;
        public bool snapping;

        public void Reset()
        {
            axisConstraint = AxisConstraint.None;
            snapping = false;
        }
    }
}