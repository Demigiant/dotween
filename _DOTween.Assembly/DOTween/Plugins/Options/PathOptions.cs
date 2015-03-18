// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/10/02 11:58
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Options
{
    public enum OrientType
    {
        None,
        ToPath,
        LookAtTransform,
        LookAtPosition
    }

    public struct PathOptions
    {
        public PathMode mode;
        public OrientType orientType;
        public AxisConstraint lockPositionAxis, lockRotationAxis;
        public bool isClosedPath;
        public Vector3 lookAtPosition;
        public Transform lookAtTransform;
        public float lookAhead;
        public bool hasCustomForwardDirection;
        public Quaternion forward;
        public bool useLocalPosition;
        public Transform parent; // Only used with OrientType.ToPath and useLocalPosition set as TRUE

        internal float startupZRot; // Used to store Z value in case of lock Z, in order to rotate things differently 
    }
}