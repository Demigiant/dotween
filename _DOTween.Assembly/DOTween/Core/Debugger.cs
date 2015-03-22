// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/03 11:33
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using UnityEngine;

namespace DG.Tweening.Core
{
    internal static class Debugger
    {
        // 0: errors only - 1: default - 2: verbose
        internal static int logPriority;

        internal static void Log(object message)
        {
            Debug.Log("DOTWEEN :: " + message);
        }
        internal static void LogWarning(object message)
        {
            Debug.LogWarning("DOTWEEN :: " + message);
        }
        internal static void LogError(object message)
        {
            Debug.LogError("DOTWEEN :: " + message);
        }

        internal static void LogReport(object message)
        {
            Debug.Log("<color=#00B500FF>DOTWEEN :: " + message + "</color>");
        }

        internal static void LogInvalidTween(Tween t)
        {
            LogWarning("This Tween has been killed and is now invalid");
        }

//        internal static void LogNullTarget()
//        {
//            LogWarning("The target for this tween shortcut is null");
//        }

        internal static void LogNestedTween(Tween t)
        {
            LogWarning("This Tween was added to a Sequence and can't be controlled directly");
        }

        internal static void LogNullTween(Tween t)
        {
            LogWarning("Null Tween");
        }

        internal static void LogNonPathTween(Tween t)
        {
            LogWarning("This Tween is not a path tween");
        }

        internal static void SetLogPriority(LogBehaviour logBehaviour)
        {
            switch (logBehaviour) {
            case LogBehaviour.Default:
                logPriority = 1;
                break;
            case LogBehaviour.Verbose:
                logPriority = 2;
                break;
            default:
                logPriority = 0;
                break;
            }
        }
    }
}