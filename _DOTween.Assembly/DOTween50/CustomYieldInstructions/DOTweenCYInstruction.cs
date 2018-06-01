// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/06/01 21:05
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.CustomYieldInstructions
{
    public class DOTweenCYInstruction
    {
        public class WaitForCompletion : CustomYieldInstruction
        {
            public override bool keepWaiting {
                get {
                    return t.active && !t.isComplete;
                }
            }

            readonly Tween t;
            public WaitForCompletion(Tween tween)
            {
                t = tween;
            }
        }

        public class WaitForRewind : CustomYieldInstruction
        {
            public override bool keepWaiting {
                get {
                    return t.active && (!t.playedOnce || t.position * (t.completedLoops + 1) > 0);
                }
            }

            readonly Tween t;
            public WaitForRewind(Tween tween)
            {
                t = tween;
            }
        }

        public class WaitForKill : CustomYieldInstruction
        {
            public override bool keepWaiting {
                get {
                    return t.active;
                }
            }

            readonly Tween t;
            public WaitForKill(Tween tween)
            {
                t = tween;
            }
        }

        public class WaitForElapsedLoops : CustomYieldInstruction
        {
            public override bool keepWaiting {
                get {
                    return t.active && t.completedLoops < elapsedLoops;
                }
            }

            readonly Tween t;
            readonly int elapsedLoops;
            public WaitForElapsedLoops(Tween tween, int elapsedLoops)
            {
                t = tween;
                this.elapsedLoops = elapsedLoops;
            }
        }

        public class WaitForPosition : CustomYieldInstruction
        {
            public override bool keepWaiting {
                get {
                    return t.active && t.position * (t.completedLoops + 1) < position;
                }
            }

            readonly Tween t;
            readonly float position;
            public WaitForPosition(Tween tween, float position)
            {
                t = tween;
                this.position = position;
            }
        }

        public class WaitForStart : CustomYieldInstruction
        {
            public override bool keepWaiting {
                get {
                    return t.active && !t.playedOnce;
                }
            }

            readonly Tween t;
            public WaitForStart(Tween tween)
            {
                t = tween;
            }
        }
    }
}