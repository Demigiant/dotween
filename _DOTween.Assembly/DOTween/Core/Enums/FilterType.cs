// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/05 14:42
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

namespace DG.Tweening.Core.Enums
{
    internal enum FilterType
    {
        All,
        TargetOrId, // Check both for id and target
        TargetAndId, // Check for both id and target on the same tween
        AllExceptTargetsOrIds, // Excludes given targets or ids
        DOGetter
    }
}