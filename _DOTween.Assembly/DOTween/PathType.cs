// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/04 10:19
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

namespace DG.Tweening
{
    /// <summary>
    /// Type of path to use with DOPath tweens
    /// </summary>
    public enum PathType
    {
        /// <summary>Linear, composed of straight segments between each waypoint</summary>
        Linear,
        /// <summary>Curved path (which uses Catmull-Rom curves)</summary>
        CatmullRom
    }
}