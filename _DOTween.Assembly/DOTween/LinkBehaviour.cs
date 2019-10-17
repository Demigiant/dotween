// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2019/02/24 13:05
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

namespace DG.Tweening
{
    /// <summary>
    /// Behaviour that can be assigned when chaining a SetLink to a tween
    /// </summary>
    public enum LinkBehaviour
    {
        /// <summary>Pauses the tween when the link target is disabled</summary>
        PauseOnDisable,
        /// <summary>Pauses the tween when the link target is disabled, plays it when it's enabled</summary>
        PauseOnDisablePlayOnEnable,
        /// <summary>Pauses the tween when the link target is disabled, restarts it when it's enabled</summary>
        PauseOnDisableRestartOnEnable,
        /// <summary>Plays the tween when the link target is enabled</summary>
        PlayOnEnable,
        /// <summary>Restarts the tween when the link target is enabled</summary>
        RestartOnEnable,
        /// <summary>Kills the tween when the link target is disabled</summary>
        KillOnDisable,
        /// <summary>Kills the tween when the link target is destroyed (becomes NULL). This is always active even if another behaviour is chosen</summary>
        KillOnDestroy,
        /// <summary>Completes the tween when the link target is disabled</summary>
        CompleteOnDisable,
        /// <summary>Completes and kills the tween when the link target is disabled</summary>
        CompleteAndKillOnDisable,
        /// <summary>Rewinds the tween (delay excluded) when the link target is disabled</summary>
        RewindOnDisable,
        /// <summary>Rewinds and kills the tween when the link target is disabled</summary>
        RewindAndKillOnDisable,
    }
}