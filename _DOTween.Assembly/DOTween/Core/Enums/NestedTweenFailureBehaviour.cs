// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2019/02/28 11:08
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

namespace DG.Tweening.Core.Enums
{
    /// <summary>
    /// Behaviour in case a tween nested inside a Sequence fails and is captured by safe mode
    /// </summary>
    public enum NestedTweenFailureBehaviour
    {
        /// <summary>If the Sequence contains other elements, kill the failed tween but preserve the rest</summary>
        TryToPreserveSequence,
        /// <summary>Kill the whole Sequence</summary>
        KillWholeSequence
    }
}