// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/08/21 13:08
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

#if COMPATIBLE
using DOVector3 = DG.Tweening.Core.Surrogates.Vector3Wrapper;
using DOQuaternion = DG.Tweening.Core.Surrogates.QuaternionWrapper;
#else
using DOVector3 = UnityEngine.Vector3;
using DOQuaternion = UnityEngine.Quaternion;
#endif
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins.Core
{
    // Utils for special plugins
    internal static class SpecialPluginsUtils
    {
        // Returns TRUE if it's successful, FALSE otherwise
        internal static bool SetLookAt(TweenerCore<DOQuaternion, DOVector3, QuaternionOptions> t)
        {
            Transform trans = t.target as Transform;
            Vector3 towards = t.endValue;
            towards -= trans.position;
            switch (t.plugOptions.axisConstraint) {
            case AxisConstraint.X:
                towards.x = 0;
                break;
            case AxisConstraint.Y:
                towards.y = 0;
                break;
            case AxisConstraint.Z:
                towards.z = 0;
                break;
            }
            Vector3 lookAtRotation = Quaternion.LookRotation(towards, t.plugOptions.up).eulerAngles;
            t.endValue = lookAtRotation;
            return true;
        }

        // Returns TRUE if it's successful, FALSE otherwise
        internal static bool SetPunch(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
        {
            Vector3 startupVal;
            try {
                startupVal = t.getter();
            } catch { return false; }

            // Force specific settings
            t.isRelative = t.isSpeedBased = false;
            t.easeType = Ease.OutQuad;
            t.customEase = null;

            int len = t.endValue.Length;
            for (int i = 0; i < len; i++) t.endValue[i] = t.endValue[i] + startupVal;
            return true;
        }

        // Returns TRUE if it's successful, FALSE otherwise
        internal static bool SetShake(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
        {
            if (!SetPunch(t)) return false;
            t.easeType = Ease.Linear;
            return true;
        }

        // Returns TRUE if it's successful, FALSE otherwise
        // Behaves like a regular shake, but also changes the endValues so that they reflect the local axis rotation of the camera
        internal static bool SetCameraShakePosition(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t)
        {
            if (!SetShake(t)) return false;

            Camera target = t.target as Camera;
            if (target == null) return false;

            Vector3 startupVal = t.getter();
            Transform trans = target.transform;
            int len = t.endValue.Length;
            for (int i = 0; i < len; i++) {
                Vector3 endVal = t.endValue[i];
                t.endValue[i] = (trans.localRotation * (endVal - startupVal)) + startupVal;
            }
            return true;
        }
    }
}