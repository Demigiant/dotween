// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/04 10:15
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core.PathCore
{
    // Public so it can be used internally as a T2 for PathPlugin.
    // Also used by DOTweenPath and relative Inspector to set up visual paths.
    [System.Serializable]
    public class Path
    {
        // Static decoders stored to avoid creating new ones each time
        static CatmullRomDecoder _catmullRomDecoder;
        static LinearDecoder _linearDecoder;
        public float[] wpLengths; // Unit length of each waypoint (public so it can be accessed at runtime by external scripts)

        [SerializeField] internal PathType type;
        [SerializeField] internal int subdivisionsXSegment; // Subdivisions x each segment
        [SerializeField] internal int subdivisions; // Stored by PathPlugin > total subdivisions for whole path (derived automatically from subdivisionsXSegment)
        [SerializeField] internal Vector3[] wps; // Waypoints (modified by PathPlugin when setting relative end value and change value) - also modified by DOTweenPathInspector
        [SerializeField] internal ControlPoint[] controlPoints; // Control points used by non-linear paths
        [SerializeField] internal float length; // Unit length of the path
        [SerializeField] internal bool isFinalized; // TRUE when the path has been finalized (either by starting the tween or if the path was created by the Path Editor)

        [SerializeField] internal float[] timesTable; // Connected to lengthsTable, used for constant speed calculations
        [SerializeField] internal float[] lengthsTable; // Connected to timesTable, used for constant speed calculations
        internal int linearWPIndex = -1; // Waypoint towards which we're moving (only stored for linear paths, when calling GetPoint)
        Path _incrementalClone; // Last incremental clone. Stored in case of incremental loops, to avoid recreating a new path every time
        int _incrementalIndex = 0;

        ABSPathDecoder _decoder;

        // GIZMOS DATA ///////////////////////////////////////////////

        bool _changed; // Indicates that the path has changed (after an incremental loop moved on/back) and the gizmo points need to be recalculated
        internal Vector3[] nonLinearDrawWps; // Used to store non-linear path gizmo points when inside Unity editor (also used by DOTweenPathInspector)
        internal Vector3 targetPosition; // Set by PathPlugin at each update
        internal Vector3? lookAtPosition; // Set by PathPlugin in case there's a lookAt active
        internal Color gizmoColor = new Color(1, 1, 1, 0.7f);

        #region Main

        // ***********************************************************************************
        // CONSTRUCTOR
        // ***********************************************************************************

        public Path(PathType type, Vector3[] waypoints, int subdivisionsXSegment, Color? gizmoColor = null)
        {
            this.type = type;
            this.subdivisionsXSegment = subdivisionsXSegment;
            if (gizmoColor != null) this.gizmoColor = (Color)gizmoColor;
            AssignWaypoints(waypoints, true);
            AssignDecoder(type);

            if (DOTween.isUnityEditor) DOTween.GizmosDelegates.Add(Draw);
        }

        internal Path()
        {
            // Used when cloning it
        }

        // Needs to be called once waypoints and decoder are assigned, to setup or refresh path data.
        internal void FinalizePath(bool isClosedPath, AxisConstraint lockPositionAxes, Vector3 currTargetVal)
        {
            // Rebuild path to lock eventual axes
            if (lockPositionAxes != AxisConstraint.None) {
                bool lockX = ((lockPositionAxes & AxisConstraint.X) == AxisConstraint.X);
                bool lockY = ((lockPositionAxes & AxisConstraint.Y) == AxisConstraint.Y);
                bool lockZ = ((lockPositionAxes & AxisConstraint.Z) == AxisConstraint.Z);
                for (int i = 0; i < wps.Length; ++i) {
                    Vector3 pt = wps[i];
                    wps[i] = new Vector3(
                        lockX ? currTargetVal.x : pt.x,
                        lockY ? currTargetVal.y : pt.y,
                        lockZ ? currTargetVal.z : pt.z
                    );
                }
            }

            _decoder.FinalizePath(this, wps, isClosedPath);
            isFinalized = true;
        }

        /// <summary>
        /// Gets the point on the path at the given percentage (0 to 1)
        /// </summary>
        /// <param name="perc">The percentage (0 to 1) at which to get the point</param>
        /// <param name="convertToConstantPerc">If TRUE constant speed is taken into account, otherwise not</param>
        internal Vector3 GetPoint(float perc, bool convertToConstantPerc = false)
        {
            if (convertToConstantPerc) perc = ConvertToConstantPathPerc(perc);
            return _decoder.GetPoint(perc, wps, this, controlPoints);
        }

        // Converts the given raw percentage to the correct percentage considering constant speed
        internal float ConvertToConstantPathPerc(float perc)
        {
            if (type == PathType.Linear) return perc;

            if (perc > 0 && perc < 1) {
                float tLen = length * perc;
                // Find point in time/length table
                float t0 = 0, l0 = 0, t1 = 0, l1 = 0;
                int count = lengthsTable.Length;
                for (int i = 0; i < count; ++i) {
                    if (lengthsTable[i] > tLen) {
                        t1 = timesTable[i];
                        l1 = lengthsTable[i];
                        if (i > 0) l0 = lengthsTable[i - 1];
                        break;
                    }
                    t0 = timesTable[i];
                }
                // Find correct time
                perc = t0 + ((tLen - l0) / (l1 - l0)) * (t1 - t0);
            }

            // Clamp value because path has limited range of 0-1
            if (perc > 1) perc = 1;
            else if (perc < 0) perc = 0;

            return perc;
        }

        // Returns the waypoint associated with the given path percentage
        internal int GetWaypointIndexFromPerc(float perc, bool isMovingForward)
        {
            if (perc >= 1) return wps.Length - 1;
            if (perc <= 0) return 0;
            float totPercLen = length * perc;
            float currLen = 0;
            for (int i = 0, count = wpLengths.Length; i < count; i++) {
                currLen += wpLengths[i];
                if (i == count - 1) return isMovingForward ? i - 1 : i;
                if (currLen < totPercLen) continue;
                if (currLen > totPercLen) return isMovingForward ? i - 1 : i;
                return i;
            }
            return 0;
        }

        // USED EXTERNALLY, to output a series of points that can be used to draw the path outside of DOTween
        // (called by TweenExtensions.PathGetDrawPoints)
        internal static Vector3[] GetDrawPoints(Path p, int drawSubdivisionsXSegment)
        {
            int wpsCount = p.wps.Length;
            if (p.type == PathType.Linear) return p.wps;

            int gizmosSubdivisions = wpsCount * drawSubdivisionsXSegment;
            Vector3[] drawPoints = new Vector3[gizmosSubdivisions + 1];
            for (int i = 0; i <= gizmosSubdivisions; ++i) {
                float perc = i / (float)gizmosSubdivisions;
                Vector3 wp = p.GetPoint(perc);
                drawPoints[i] = wp;
            }
            return drawPoints;
        }

        // Refreshes the waypoints used to draw non-linear gizmos and the PathInspector scene view.
        // Called by Draw method or by DOTweenPathInspector
        internal static void RefreshNonLinearDrawWps(Path p)
        {
            int wpsCount = p.wps.Length;

            int gizmosSubdivisions = wpsCount * 10;
            if (p.nonLinearDrawWps == null || p.nonLinearDrawWps.Length != gizmosSubdivisions + 1)
                p.nonLinearDrawWps = new Vector3[gizmosSubdivisions + 1];
            for (int i = 0; i <= gizmosSubdivisions; ++i) {
                float perc = i / (float)gizmosSubdivisions;
                Vector3 wp = p.GetPoint(perc);
                p.nonLinearDrawWps[i] = wp;
            }
        }

        // Stops drawing the path gizmo
        internal void Destroy()
        {
            if (DOTween.isUnityEditor) DOTween.GizmosDelegates.Remove(Draw);
            wps = null;
            wpLengths = timesTable = lengthsTable = null;
            nonLinearDrawWps = null;
            isFinalized = false;
        }

        // Clones this path with the given loop increment
        internal Path CloneIncremental(int loopIncrement)
        {
            if (_incrementalClone != null) {
                if (_incrementalIndex == loopIncrement) return _incrementalClone;
                _incrementalClone.Destroy();
            }

            int wpsLen = wps.Length;
            Vector3 diff = wps[wpsLen - 1] - wps[0];
            Vector3[] incrWps = new Vector3[wps.Length];
            for (int i = 0; i < wpsLen; ++i) incrWps[i] = wps[i] + (diff * loopIncrement);

            int cpsLen = controlPoints.Length;
            ControlPoint[] incrCps = new ControlPoint[cpsLen];
            for (int i = 0; i < cpsLen; ++i) incrCps[i] = controlPoints[i] + (diff * loopIncrement);

            Vector3[] incrNonLinearDrawWps  = null;
            if (nonLinearDrawWps != null) {
                int nldLen = nonLinearDrawWps.Length;
                incrNonLinearDrawWps = new Vector3[nldLen];
                for (int i = 0; i < nldLen; ++i) incrNonLinearDrawWps[i] = nonLinearDrawWps[i] + (diff * loopIncrement);
            }
            
            _incrementalClone = new Path();
            _incrementalIndex = loopIncrement;
            _incrementalClone.type = type;
            _incrementalClone.subdivisionsXSegment = subdivisionsXSegment;
            _incrementalClone.subdivisions = subdivisions;
            _incrementalClone.wps = incrWps;
            _incrementalClone.controlPoints = incrCps;
            if (DOTween.isUnityEditor) DOTween.GizmosDelegates.Add(_incrementalClone.Draw);

            _incrementalClone.length = length;
            _incrementalClone.wpLengths = wpLengths;
            _incrementalClone.timesTable = timesTable;
            _incrementalClone.lengthsTable = lengthsTable;
            _incrementalClone._decoder = _decoder;
            _incrementalClone.nonLinearDrawWps = incrNonLinearDrawWps;
            _incrementalClone.targetPosition = targetPosition;
            _incrementalClone.lookAtPosition = lookAtPosition;

            _incrementalClone.isFinalized = true;
            return _incrementalClone;
        }

        #endregion

        // Deletes the previous waypoints and assigns the new ones
        // (newWps length must be at least 1).
        // Internal so DOTweenPathInspector can use it
        internal void AssignWaypoints(Vector3[] newWps, bool cloneWps = false)
        {
            if (cloneWps) {
                int count = newWps.Length;
                wps = new Vector3[count];
                for (int i = 0; i < count; ++i) wps[i] = newWps[i];
            } else wps = newWps;
        }
//        // Deletes the previous waypoints and assigns the new ones, always cloning them
//        // (newWps length must be at least 1).
//        // Internal so DOTweenPathInspector can use it
//        internal void AssignWaypoints(List<Vector3> newWps)
//        {
//            int count = newWps.Count;
//            wps = new Vector3[count];
//            for (int i = 0; i < count; ++i) wps[i] = newWps[i];
//        }

        // Internal so DOTweenPathInspector and DOTweenPath can use it
        internal void AssignDecoder(PathType pathType)
        {
            type = pathType;
            switch (pathType) {
            case PathType.Linear:
                if (_linearDecoder == null) _linearDecoder = new LinearDecoder();
                _decoder = _linearDecoder;
                break;
            default: // Catmull-Rom
                if (_catmullRomDecoder == null) _catmullRomDecoder = new CatmullRomDecoder();
                _decoder = _catmullRomDecoder;
                break;
            }
        }

//        // If path is linear wpLengths were stored when calling FinalizePath
//        void StoreWaypointsLengths()
//        {
//            _decoder.SetWaypointsLengths(this, subdivisions);
//        }

        // Used in DOTween.OnDrawGizmos if we're inside Unity Editor
        // and in DOTweenPath when setting up the pre-compiled path
        internal void Draw() { Draw(this); }
        static void Draw(Path p)
        {
            if (p.timesTable == null) return;

            Color gizmosFadedCol = p.gizmoColor;
            gizmosFadedCol.a *= 0.5f;
            Gizmos.color = p.gizmoColor;
            int wpsCount = p.wps.Length;

            if (p._changed || p.type != PathType.Linear && p.nonLinearDrawWps == null) {
                p._changed = false;
                if (p.type != PathType.Linear) {
                    // Store draw points
                    RefreshNonLinearDrawWps(p);
                }
            }

            // Draw path
            Vector3 currPt;
            Vector3 prevPt;
            switch (p.type) {
            case PathType.Linear:
                prevPt = p.wps[0];
                for (int i = 0; i < wpsCount; ++i) {
                    currPt = p.wps[i];
                    Gizmos.DrawLine(currPt, prevPt);
                    prevPt = currPt;
                }
                break;
            default: // Curved
                prevPt = p.nonLinearDrawWps[0];
                int count = p.nonLinearDrawWps.Length;
                for (int i = 1; i < count; ++i) {
                    currPt = p.nonLinearDrawWps[i];
                    Gizmos.DrawLine(currPt, prevPt);
                    prevPt = currPt;
                }
                break;
            }

            Gizmos.color = gizmosFadedCol;
            const float spheresSize = 0.075f;

            // Draw path control points
            for (int i = 0; i < wpsCount; ++i) Gizmos.DrawSphere(p.wps[i], spheresSize);

            // Draw eventual path lookAt
            if (p.lookAtPosition != null) {
                Vector3 lookAtP = (Vector3)p.lookAtPosition;
                Gizmos.DrawLine(p.targetPosition, lookAtP);
                Gizmos.DrawWireSphere(lookAtP, spheresSize);
            }
        }
    }
}