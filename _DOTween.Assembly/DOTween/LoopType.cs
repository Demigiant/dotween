// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/05 13:35
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

namespace DG.Tweening
{
    /// <summary>
    /// Types of loop
    /// </summary>
    public enum LoopType
    {
        /// <summary>Each loop cycle restarts from the beginning</summary>
        Restart,
        /// <summary>The tween moves forward and backwards at alternate cycles</summary>
        Yoyo,
        /// <summary>Continuously increments the tween at the end of each loop cycle (A to B, B to B+(A-B), and so on), thus always moving "onward".
        /// <para>In case of String tweens works only if the tween is set as relative</para></summary>
        Incremental
    }
}