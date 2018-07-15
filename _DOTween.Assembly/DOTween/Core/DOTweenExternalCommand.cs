// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/07/13 19:21
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core
{
    /// <summary>
    /// Used to dispatch commands that need to be captured externally, usually by Modules
    /// </summary>
    public static class DOTweenExternalCommand
    {
        public static event Action<PathOptions,Tween,Quaternion,Transform> SetOrientationOnPath;
        internal static void Dispatch_SetOrientationOnPath(PathOptions options, Tween t, Quaternion newRot, Transform trans)
        { if (SetOrientationOnPath != null) SetOrientationOnPath(options, t, newRot, trans); }
    }
}