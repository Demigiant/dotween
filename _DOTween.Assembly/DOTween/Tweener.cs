// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/12 16:24
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
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Animates a single value
    /// </summary>
    public abstract class Tweener : Tween
    {
        // TRUE when start value has been changed via From or ChangeStart/Values (allows DoStartup to take it into account).
        // Reset by TweenerCore
        internal bool hasManuallySetStartValue;
        internal bool isFromAllowed = true; // if FALSE from tweens won't be allowed. Reset by TweenerCore

        internal Tweener() {}

        // ===================================================================================
        // ABSTRACT METHODS ------------------------------------------------------------------

        /// <summary>Changes the start value of a tween and rewinds it (without pausing it).
        /// Has no effect with tweens that are inside Sequences</summary>
        /// <param name="newStartValue">The new start value</param>
        /// <param name="newDuration">If bigger than 0 applies it as the new tween duration</param>
        public abstract Tweener ChangeStartValue(object newStartValue, float newDuration = -1);

        /// <summary>Changes the end value of a tween and rewinds it (without pausing it).
        /// Has no effect with tweens that are inside Sequences</summary>
        /// <param name="newEndValue">The new end value</param>
        /// <param name="newDuration">If bigger than 0 applies it as the new tween duration</param>
        /// <param name="snapStartValue">If TRUE the start value will become the current target's value, otherwise it will stay the same</param>
        public abstract Tweener ChangeEndValue(object newEndValue, float newDuration = -1, bool snapStartValue = false);
        /// <summary>Changes the end value of a tween and rewinds it (without pausing it).
        /// Has no effect with tweens that are inside Sequences</summary>
        /// <param name="newEndValue">The new end value</param>
        /// <param name="snapStartValue">If TRUE the start value will become the current target's value, otherwise it will stay the same</param>
        public abstract Tweener ChangeEndValue(object newEndValue, bool snapStartValue);

        /// <summary>Changes the start and end value of a tween and rewinds it (without pausing it).
        /// Has no effect with tweens that are inside Sequences</summary>
        /// <param name="newStartValue">The new start value</param>
        /// <param name="newEndValue">The new end value</param>
        /// <param name="newDuration">If bigger than 0 applies it as the new tween duration</param>
        public abstract Tweener ChangeValues(object newStartValue, object newEndValue, float newDuration = -1);

        internal abstract Tweener SetFrom(bool relative);

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        // CALLED BY DOTween when spawning/creating a new Tweener.
        // Returns TRUE if the setup is successful
        internal static bool Setup<T1, T2, TPlugOptions>(
            TweenerCore<T1, T2, TPlugOptions> t, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration, ABSTweenPlugin<T1, T2, TPlugOptions> plugin = null
        )
            where TPlugOptions : struct, IPlugOptions
        {
            if (plugin != null) t.tweenPlugin = plugin;
            else {
                if (t.tweenPlugin == null) t.tweenPlugin = PluginsManager.GetDefaultPlugin<T1, T2, TPlugOptions>();
                if (t.tweenPlugin == null) {
                    // No suitable plugin found. Kill
                    Debugger.LogError("No suitable plugin found for this type");
                    return false;
                }
            }

            t.getter = getter;
            t.setter = setter;
            t.endValue = endValue;
            t.duration = duration;
            // Defaults
            t.autoKill = DOTween.defaultAutoKill;
            t.isRecyclable = DOTween.defaultRecyclable;
            t.easeType = DOTween.defaultEaseType; // Set to INTERNAL_Zero in case of 0 duration, but in DoStartup
            t.easeOvershootOrAmplitude = DOTween.defaultEaseOvershootOrAmplitude;
            t.easePeriod = DOTween.defaultEasePeriod;
            t.loopType = DOTween.defaultLoopType;
            t.isPlaying = DOTween.defaultAutoPlay == AutoPlay.All || DOTween.defaultAutoPlay == AutoPlay.AutoPlayTweeners;
            return true;
        }

        // CALLED BY TweenerCore
        // Returns the elapsed time minus delay in case of success,
        // -1 if there are missing references and the tween needs to be killed
        internal static float DoUpdateDelay<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, float elapsed) where TPlugOptions : struct, IPlugOptions
        {
            float tweenDelay = t.delay;
            if (elapsed > tweenDelay) {
                // Delay complete
                t.elapsedDelay = tweenDelay;
                t.delayComplete = true;
                return elapsed - tweenDelay;
            }
            t.elapsedDelay = elapsed;
            return 0;
        }

        // CALLED VIA Tween the moment the tween starts, AFTER any delay has elapsed
        // (unless it's a FROM tween, in which case it will be called BEFORE any eventual delay).
        // Returns TRUE in case of success,
        // FALSE if there are missing references and the tween needs to be killed
        internal static bool DoStartup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
        {
            t.startupDone = true;

            // Special startup operations
            if (t.specialStartupMode != SpecialStartupMode.None) {
                if (!DOStartupSpecials(t)) return false;
            }

            if (!t.hasManuallySetStartValue) {
                // Take start value from current target value
                if (DOTween.useSafeMode) {
                    try {
                        t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
                    } catch {
                        return false; // Target/field doesn't exist: kill tween
                    }
                } else t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
            }

            if (t.isRelative) t.tweenPlugin.SetRelativeEndValue(t);

            t.tweenPlugin.SetChangeValue(t);

            // Duration based startup operations
            DOStartupDurationBased(t);

            // Applied here so that the eventual duration derived from a speedBased tween has been set
            if (t.duration <= 0) t.easeType = Ease.INTERNAL_Zero;
            
            return true;
        }

        // CALLED BY TweenerCore
        internal static Tweener DoChangeStartValue<T1, T2, TPlugOptions>(
            TweenerCore<T1, T2, TPlugOptions> t, T2 newStartValue, float newDuration
        ) where TPlugOptions : struct, IPlugOptions
        {
            t.hasManuallySetStartValue = true;
            t.startValue = newStartValue;

            if (t.startupDone) {
                if (t.specialStartupMode != SpecialStartupMode.None) {
                    if (!DOStartupSpecials(t)) return null;
                }
                t.tweenPlugin.SetChangeValue(t);
            }

            if (newDuration > 0) {
                t.duration = newDuration;
                if (t.startupDone) DOStartupDurationBased(t);
            }

            // Force rewind
            DoGoto(t, 0, 0, UpdateMode.IgnoreOnUpdate);

            return t;
        }

        // CALLED BY TweenerCore
        internal static Tweener DoChangeEndValue<T1, T2, TPlugOptions>(
            TweenerCore<T1, T2, TPlugOptions> t, T2 newEndValue, float newDuration, bool snapStartValue
        ) where TPlugOptions : struct, IPlugOptions
        {
            t.endValue = newEndValue;
            t.isRelative = false;

            if (t.startupDone) {
                if (t.specialStartupMode != SpecialStartupMode.None) {
                    if (!DOStartupSpecials(t)) return null;
                }
                if (snapStartValue) {
                    // Reassign startValue with current target's value
                    if (DOTween.useSafeMode) {
                        try {
                            t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
                        } catch {
                            // Target/field doesn't exist: kill tween
                            TweenManager.Despawn(t);
                            return null;
                        }
                    } else t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
                }
                t.tweenPlugin.SetChangeValue(t);
            }

            if (newDuration > 0) {
                t.duration = newDuration;
                if (t.startupDone) DOStartupDurationBased(t);
            }

            // Force rewind
            DoGoto(t, 0, 0, UpdateMode.IgnoreOnUpdate);

            return t;
        }

        internal static Tweener DoChangeValues<T1, T2, TPlugOptions>(
            TweenerCore<T1, T2, TPlugOptions> t, T2 newStartValue, T2 newEndValue, float newDuration
        ) where TPlugOptions : struct, IPlugOptions
        {
            t.hasManuallySetStartValue = true;
            t.isRelative = t.isFrom = false;
            t.startValue = newStartValue;
            t.endValue = newEndValue;

            if (t.startupDone) {
                if (t.specialStartupMode != SpecialStartupMode.None) {
                    if (!DOStartupSpecials(t)) return null;
                }
                t.tweenPlugin.SetChangeValue(t);
            }

            if (newDuration > 0) {
                t.duration = newDuration;
                if (t.startupDone) DOStartupDurationBased(t);
            }

            // Force rewind
            DoGoto(t, 0, 0, UpdateMode.IgnoreOnUpdate);

            return t;
        }

        // Commands shared by DOStartup/ChangeStart/End/Values if the tween has already started up
        // and thus some settings needs to be reapplied.
        // Returns TRUE in case of SUCCESS, FALSE if there were managed errors
        static bool DOStartupSpecials<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
        {
            try {
                switch (t.specialStartupMode) {
                case SpecialStartupMode.SetLookAt:
                    if (!SpecialPluginsUtils.SetLookAt(t as TweenerCore<DOQuaternion, DOVector3, QuaternionOptions>)) return false;
                    break;
                case SpecialStartupMode.SetPunch:
                    if (!SpecialPluginsUtils.SetPunch(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>)) return false;
                    break;
                case SpecialStartupMode.SetShake:
                    if (!SpecialPluginsUtils.SetShake(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>)) return false;
                    break;
                case SpecialStartupMode.SetCameraShakePosition:
                    if (!SpecialPluginsUtils.SetCameraShakePosition(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>)) return false;
                    break;
                }
                return true;
            } catch {
                // Error in SpecialPluginUtils (usually due to target being destroyed)
                return false;
            }
        }
        static void DOStartupDurationBased<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct, IPlugOptions
        {
            if (t.isSpeedBased) t.duration = t.tweenPlugin.GetSpeedBasedDuration(t.plugOptions, t.duration, t.changeValue);
            t.fullDuration = t.loops > -1 ? t.duration * t.loops : Mathf.Infinity;
        }
    }
}