// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/04 11:02
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
    internal class LinearDecoder : ABSPathDecoder
    {
        internal override void FinalizePath(Path p, Vector3[] wps, bool isClosedPath)
        {
            p.controlPoints = null;
            // Store time to len tables
            p.subdivisions = (wps.Length) * p.subdivisionsXSegment; // Unused
            SetTimeToLengthTables(p, p.subdivisions);
        }

        internal override Vector3 GetPoint(float perc, Vector3[] wps, Path p, ControlPoint[] controlPoints)
        {
            if (perc <= 0) {
                p.linearWPIndex = 1;
                return wps[0];
            }

            int startPIndex = 0;
            int endPIndex = 0;
            int count = p.timesTable.Length;
            for (int i = 1; i < count; i++) {
                if (p.timesTable[i] >= perc) {
                    startPIndex = i - 1;
                    endPIndex = i;
                    break;
                }
            }

            float startPPerc = p.timesTable[startPIndex];
            float partialPerc = perc - startPPerc;
            float partialLen = p.length * partialPerc;
            Vector3 wp0 = wps[startPIndex];
            Vector3 wp1 = wps[endPIndex];
            p.linearWPIndex = endPIndex;
            return wp0 + Vector3.ClampMagnitude(wp1 - wp0, partialLen);
        }

        // Linear exception: also sets waypoints lengths and doesn't set lengthsTable since it's useless
        internal void SetTimeToLengthTables(Path p, int subdivisions)
        {
            float pathLen = 0;
            int wpsLen = p.wps.Length;
            float[] wpLengths = new float[wpsLen];
            Vector3 prevP = p.wps[0];
            for (int i = 0; i < wpsLen; i++) {
                Vector3 currP = p.wps[i];
                float dist = Vector3.Distance(currP, prevP);
                pathLen += dist;
                prevP = currP;
                wpLengths[i] = dist;
            }
            float[] timesTable = new float[wpsLen];
            float tmpLen = 0;
            for (int i = 1; i < wpsLen; i++) {
                tmpLen += wpLengths[i];
                timesTable[i] = tmpLen / pathLen;
            }

            // Assign
            p.length = pathLen;
            p.wpLengths = wpLengths;
            p.timesTable = timesTable;
        }

        internal void SetWaypointsLengths(Path p, int subdivisions)
        {
            // Does nothing (waypoints lenghts were stored inside SetTimeToLengthTables)
        }
    }
}