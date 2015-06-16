// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/10/07 11:28
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core.PathCore
{
    /// <summary>
    /// Path control point
    /// </summary>
    [System.Serializable]
    public struct ControlPoint
    {
        public Vector3 a, b;

        public ControlPoint(Vector3 a, Vector3 b)
        {
            this.a = a;
            this.b = b;
        }

        public static ControlPoint operator +(ControlPoint cp, Vector3 v)
        {
            return new ControlPoint(cp.a + v, cp.b + v);
        }
    }
}