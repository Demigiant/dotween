// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/01 18:50
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Options
{
    public struct QuaternionOptions : IPlugOptions
    {
        public RotateMode rotateMode; // Accessed by shortcuts and Modules
        public AxisConstraint axisConstraint; // Used by SpecialStartupMode SetLookAt, accessed by shortcuts and Modules
        public Vector3 up; // Used by SpecialStartupMode SetLookAt, accessed by shortcuts and Modules
        public bool dynamicLookAt; // Used by DODynamicLookAt shortcut, updates the lookAt every frame
        public Vector3 dynamicLookAtWorldPosition;

        public void Reset()
        {
            rotateMode = RotateMode.Fast;
            axisConstraint = AxisConstraint.None;
            up = Vector3.zero;
            dynamicLookAt = false;
            dynamicLookAtWorldPosition = Vector3.zero;
        }
    }
}