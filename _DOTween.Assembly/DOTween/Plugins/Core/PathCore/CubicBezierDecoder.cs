// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/11/30 11:58
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
    internal class CubicBezierDecoder : ABSPathDecoder
    {
        internal override int minInputWaypoints { get { return 3; } }

        // Used for temporary operations
        static readonly ControlPoint[] _PartialControlPs = new ControlPoint[1];
        static readonly Vector3[] _PartialWps = new Vector3[2];

        #region Methods
        
        // Finalize path and separate wps into stripped wps (no control points) and control points
        // wps must be in multiple of 3 (each waypoint has 2 control points) plus one starting waypoint without control points, in this order:
        // - waypoint
        // - IN control point
        // - OUT control point
        // NOTE: Control points have length of wps - 1 (first wp has no control point)
        internal override void FinalizePath(Path p, Vector3[] wps, bool isClosedPath)
        {
            if (isClosedPath && !p.addedExtraEndWp) isClosedPath = false;
            // Normally there's an extra wp without control points for the starting wp added by DOTween,
            // but if isClosedPath consider an extra wp without control points at the end
            int wpsLen = wps.Length;
            int diff = p.addedExtraStartWp ? 1 : 0;
            if (p.addedExtraEndWp) diff++;
            if (wpsLen < 3 + diff || (wpsLen - diff) % 3 != 0) {
                // Report multiple of 3s error even if we're checking for multiple of 3 + starting point,
                // because starting point is assigned by DOTween and not by user
                Debug.LogError(
                    "CubicBezier paths must contain waypoints in multiple of 3 excluding the starting point added automatically by DOTween" +
                    " (1: waypoint, 2: IN control point, 3: OUT control point — the minimum amount of waypoints for a single curve is 3)"
                );
                return;
            }

//            // DEBUG
//            for (int i = 0; i < wps.Length; ++i) {
//                Debug.Log("WP " + i + " ► " + wps[i]);
//            }
//            Debug.Log("--------------------------------------");
//            // DEBUG END

            int wpsOnlyLen = diff + (wpsLen - diff) / 3;
            // Store control points and stripped version of wps
            Vector3[] strippedWps = new Vector3[wpsOnlyLen];
            p.controlPoints = new ControlPoint[wpsOnlyLen - 1]; // Exclude control points for first wp
            strippedWps[0] = wps[0];
            int strippedWpIndex = 1;
            int cpIndex = 0;
            for (int i = 3 + (p.addedExtraStartWp ? 0 : 2); i < wpsLen; i+=3) {
                strippedWps[strippedWpIndex] = wps[i-2];
                strippedWpIndex++;
                p.controlPoints[cpIndex] = new ControlPoint(wps[i-1], wps[i]);
                cpIndex++;
            }
            p.wps = strippedWps; // Reassign stripped wps to path's wps
//            // DEBUG
//            for (int i = 0; i < strippedWps.Length; ++i) {
//                Debug.Log("WP " + i + " ► " + strippedWps[i]);
//            }
//            for (int i = 0; i < p.controlPoints.Length; ++i) {
//                Debug.Log("CP " + i + " ► " + p.controlPoints[i]);
//            }
//            // DEBUG END
            // Manage closed path
            if (isClosedPath) {
                // Add control points for closed path
                Vector3 wpEnd = p.wps[p.wps.Length - 2];
                Vector3 wpStart = p.wps[0];
                Vector3 cEnd = p.controlPoints[p.controlPoints.Length - 2].b;
                Vector3 cStart = p.controlPoints[0].a;
                float maxMagnitude = (wpStart - wpEnd).magnitude;
                p.controlPoints[p.controlPoints.Length - 1] = new ControlPoint(
                    wpEnd + Vector3.ClampMagnitude(wpEnd - cEnd, maxMagnitude),
                    wpStart + Vector3.ClampMagnitude(wpStart - cStart, maxMagnitude)
                );
            }
            // Store total subdivisions
            p.subdivisions = wpsOnlyLen * p.subdivisionsXSegment;
            // Store time to len tables
            SetTimeToLengthTables(p, p.subdivisions);
            // Store waypoints lengths
            SetWaypointsLengths(p, p.subdivisionsXSegment);
        }

        // controlPoints as a separate parameter so we can pass custom ones from SetWaypointsLengths
        // Immense thanks to Vivek Tank's Gamasutra post about Bezier curves whose code I used for this:
        // https://www.gamasutra.com/blogs/VivekTank/20180806/323709/How_to_work_with_Bezier_Curve_in_Games_with_Unity.php
        internal override Vector3 GetPoint(float perc, Vector3[] wps, Path p, ControlPoint[] controlPoints)
        {
            int numSections = wps.Length - 1;
            int tSec = (int)Math.Floor(perc * numSections);
            int currPt = numSections - 1;
            if (currPt > tSec) currPt = tSec;
            float t = perc * numSections - currPt;

            Vector3 p0 = wps[currPt];
            Vector3 p1 = controlPoints[currPt].a;
            Vector3 p2 = controlPoints[currPt].b;
            Vector3 p3 = wps[currPt + 1];

            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;
        
            Vector3 result = uuu * p0
                + 3 * uu * t * p1
                + 3 * u * tt * p2
                + ttt * p3;
        
            return result;
        }

        internal void SetTimeToLengthTables(Path p, int subdivisions)
        {
            float pathLen = 0;
            float incr = 1f / subdivisions;
            float[] timesTable = new float[subdivisions];
            float[] lengthsTable = new float[subdivisions];
            Vector3 prevP = GetPoint(0, p.wps, p, p.controlPoints);
            for (int i = 1; i < subdivisions + 1; ++i) {
                float perc = incr * i;
                Vector3 currP = GetPoint(perc, p.wps, p, p.controlPoints);
                pathLen += Vector3.Distance(currP, prevP);
                prevP = currP;
                timesTable[i - 1] = perc;
                lengthsTable[i - 1] = pathLen;
            }

            // Assign
            p.length = pathLen;
            p.timesTable = timesTable;
            p.lengthsTable = lengthsTable;
        }

        internal void SetWaypointsLengths(Path p, int subdivisions)
        {
            // Create a relative path between each waypoint,
            // with its start and end control lines coinciding with the next/prev waypoints.
            int count = p.wps.Length;
            float[] wpLengths = new float[count];
            wpLengths[0] = 0;
            for (int i = 1; i < count; ++i) {
                // Create partial path
//                _PartialControlPs[0].a = i == 1 ? p.controlPoints[0].a : p.wps[i - 2];
                _PartialControlPs[0] = p.controlPoints[i - 1];
                _PartialWps[0] = p.wps[i - 1];
                _PartialWps[1] = p.wps[i];
//                _PartialControlPs[1].a = i == count - 1 ? p.controlPoints[1].a : p.wps[i + 1];
                // Calculate length of partial path
                float partialLen = 0;
                float incr = 1f / subdivisions;
                Vector3 prevP = GetPoint(0, _PartialWps, p, _PartialControlPs);
                for (int c = 1; c < subdivisions + 1; ++c) {
                    float perc = incr * c;
                    Vector3 currP = GetPoint(perc, _PartialWps, p, _PartialControlPs);
                    partialLen += Vector3.Distance(currP, prevP);
                    prevP = currP;
                }
                wpLengths[i] = partialLen;
            }

            // Assign
            p.wpLengths = wpLengths;
        }

        #endregion
    }
}