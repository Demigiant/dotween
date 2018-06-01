// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/06/01 10:55
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.CustomYieldInstructions
{
    public class WaitForCompletion : CustomYieldInstruction
    {
        public override bool keepWaiting {
            get {
                return _tween.active && !_tween.isComplete;
            }
        }

        readonly Tween _tween;

        public WaitForCompletion(Tween tween)
        {
            _tween = tween;
        }
    }
}