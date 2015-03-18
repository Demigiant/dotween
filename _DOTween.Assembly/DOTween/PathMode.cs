// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/07 10:22
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php
namespace DG.Tweening
{
    /// <summary>
    /// Path mode (used to determine correct LookAt orientation)
    /// </summary>
    public enum PathMode
    {
        /// <summary>Ignores the path mode (and thus LookAt behaviour)</summary>
        Ignore,
        /// <summary>Regular 3D path</summary>
        Full3D,
        /// <summary>2D top-down path</summary>
        TopDown2D,
        /// <summary>2D side-scroller path</summary>
        Sidescroller2D
    }
}