// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/07 12:52
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

namespace DG.Tweening.Core.Enums
{
    internal enum UpdateMode
    {
        Update,
        Goto // Treats update as a full goto, thus not calling eventual onStepComplete callbacks
    }
}