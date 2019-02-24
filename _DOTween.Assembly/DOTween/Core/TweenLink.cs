// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2019/02/24 13:10
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

namespace DG.Tweening.Core
{
    internal class TweenLink
    {
        public readonly GameObject target;
        public readonly LinkBehaviour behaviour;
        public bool lastSeenActive;

        public TweenLink(GameObject target, LinkBehaviour behaviour)
        {
            this.target = target;
            this.behaviour = behaviour;
            lastSeenActive = target.activeInHierarchy;
        }
    }
}