// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2020/10/02 11:20
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System.Collections.Generic;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Extra non-tweening-related curve methods
    /// </summary>
    public static class DOCurve
    {
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        /// <summary>
        /// Cubic bezier curve methods
        /// </summary>
        public static class CubicBezier
        {
            /// <summary>
            /// Calculates a point along the given Cubic Bezier segment-curve.<para/>
            /// </summary>
            /// <param name="startPoint">Segment start point</param>
            /// <param name="startControlPoint">Start point's control point/handle</param>
            /// <param name="endPoint">Segment end point</param>
            /// <param name="endControlPoint">End point's control point/handle</param>
            /// <param name="factor">0-1 percentage along which to retrieve point</param>
            public static Vector3 GetPointOnSegment(
                Vector3 startPoint, Vector3 startControlPoint, Vector3 endPoint, Vector3 endControlPoint, float factor
            ){
                float u = 1 - factor;
                float tt = factor * factor;
                float uu = u * u;
                float uuu = uu * u;
                float ttt = tt * factor;
                Vector3 p = uuu * startPoint
                    + 3 * uu * factor * startControlPoint
                    + 3 * u * tt * endControlPoint
                    + ttt * endPoint;
                return p;
            }

            /// <summary>
            /// Returns an array containing a series of points along the given Cubic Bezier segment-curve.<para/>
            /// </summary>
            /// <param name="startPoint">Start point</param>
            /// <param name="startControlPoint">Start point's control point/handle</param>
            /// <param name="endPoint">End point</param>
            /// <param name="endControlPoint">End point's control point/handle</param>
            /// <param name="resolution">Cloud resolution (min: 2)</param>
            public static Vector3[] GetSegmentPointCloud(
                Vector3 startPoint, Vector3 startControlPoint, Vector3 endPoint, Vector3 endControlPoint, int resolution = 10
            ){
                if (resolution < 2) resolution = 2;
                Vector3[] pointCloud = new Vector3[resolution];
                float step = 1f / (resolution - 1);
                for (int i = 0; i < resolution; ++i) {
                    pointCloud[i] = GetPointOnSegment(startPoint, startControlPoint, endPoint, endControlPoint, step * i);
                }
                return pointCloud;
            }
            /// <summary>
            /// Calculates a series of points along the given Cubic Bezier segment-curve and adds them to the given list.<para/>
            /// </summary>
            /// <param name="startPoint">Start point</param>
            /// <param name="startControlPoint">Start point's control point/handle</param>
            /// <param name="endPoint">End point</param>
            /// <param name="endControlPoint">End point's control point/handle</param>
            /// <param name="resolution">Cloud resolution (min: 2)</param>
            public static void GetSegmentPointCloud(
                List<Vector3> addToList, Vector3 startPoint, Vector3 startControlPoint, Vector3 endPoint, Vector3 endControlPoint, int resolution = 10
            ){
                if (resolution < 2) resolution = 2;
                float step = 1f / (resolution - 1);
                for (int i = 0; i < resolution; ++i) {
                    addToList.Add(GetPointOnSegment(startPoint, startControlPoint, endPoint, endControlPoint, step * i));
                }
            }
        }
    }
}