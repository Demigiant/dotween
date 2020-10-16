// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2020/10/16 11:47
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

namespace DG.Tweening.Core.Enums
{
    /// <summary>
    /// Log types thrown by errors captured and prevented by safe mode
    /// </summary>
    public enum SafeModeLogBehaviour
    {
        /// <summary>No logs. NOT RECOMMENDED</summary>
        None,
        /// <summary>Throw a normal log</summary>
        Normal,
        /// <summary>Throw a warning log (default)</summary>
        Warning,
        /// <summary>Throw an error log</summary>
        Error
    }
}