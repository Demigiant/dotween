// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/28 11:33
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;

#pragma warning disable 1591
namespace DG.Tweening
{
    /// <summary>
    /// What axis to constrain in case of Vector tweens
    /// </summary>
    [Flags]
    public enum AxisConstraint
    {
        None = 0,
        X = 2,
        Y = 4,
        Z = 8,
        W = 16
    }
}