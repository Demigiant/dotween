// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/08/17 19:40
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

namespace DG.Tweening.Core
{
    internal static class Utils
    {
        /// <summary>
        /// Returns a Vector3 with z = 0
        /// </summary>
        internal static Vector3 Vector3FromAngle(float degrees, float magnitude)
        {
            float radians = degrees * Mathf.Deg2Rad;
            return new Vector3(magnitude * Mathf.Cos(radians), magnitude * Mathf.Sin(radians), 0);
        }

        /// <summary>
        /// Returns the 2D angle between two vectors
        /// </summary>
        public static float Angle2D(Vector3 from, Vector3 to)
        {
            Vector2 baseDir = Vector2.right;
            to -= from;
            float ang = Vector2.Angle(baseDir, to);
            Vector3 cross = Vector3.Cross(baseDir, to);
            if (cross.z > 0) ang = 360 - ang;
            ang *= -1f;
            return ang;
        }
    }
}