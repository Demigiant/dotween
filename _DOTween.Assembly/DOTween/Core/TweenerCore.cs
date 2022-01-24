// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/07 12:56
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

#if COMPATIBLE
using DG.Tweening.Core.Surrogates;
#endif
using System;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1591
namespace DG.Tweening.Core
{
    // Public so it can be used with SetOptions to show the correct overload
    // and also to allow custom plugins to change start/end/changeValue.
    // T1: type of value to tween
    // T2: format in which value is stored while tweening
    // TPlugOptions: options type
    public class TweenerCore<T1,T2,TPlugOptions> : Tweener where TPlugOptions : struct, IPlugOptions
    {
        // SETUP DATA ////////////////////////////////////////////////

        public T2 startValue, endValue, changeValue;
        public TPlugOptions plugOptions;
        public DOGetter<T1> getter;
        public DOSetter<T1> setter;
        internal ABSTweenPlugin<T1, T2, TPlugOptions> tweenPlugin;

        const string _TxtCantChangeSequencedValues = "You cannot change the values of a tween contained inside a Sequence";
        Type _colorType = typeof(Color);
        Type _color32Type = typeof(Color32);

        #region Constructor

        internal TweenerCore()
        {
            typeofT1 = typeof(T1);
            typeofT2 = typeof(T2);
            typeofTPlugOptions = typeof(TPlugOptions);
            tweenType = TweenType.Tweener;
            Reset();
        }

        #endregion

        #region Public Methods

        // No generics because T to T2 conversion isn't compatible with AOT
        public override Tweener ChangeStartValue(object newStartValue, float newDuration = -1)
        {
            if (isSequenced) {
                Debugger.LogError(_TxtCantChangeSequencedValues, this);
                return this;
            }
#if COMPATIBLE
            ConvertToWrapper(ref newStartValue);
#endif
            Type valT = newStartValue.GetType();
            if (!ValidateChangeValueType(valT, out bool isColor32ToColor)) {
                Debugger.LogError("ChangeStartValue: incorrect newStartValue type (is " + valT + ", should be " + typeofT2 + ")", this);
                return this;
            }
            if (isColor32ToColor) return DoChangeStartValue(this, (T2)(object)(Color)(Color32)newStartValue, newDuration);
            return DoChangeStartValue(this, (T2)newStartValue, newDuration);
        }

        // No generics because T to T2 conversion isn't compatible with AOT
        public override Tweener ChangeEndValue(object newEndValue, bool snapStartValue)
        { return ChangeEndValue(newEndValue, -1, snapStartValue); }
        // No generics because T to T2 conversion isn't compatible with AOT
        public override Tweener ChangeEndValue(object newEndValue, float newDuration = -1, bool snapStartValue = false)
        {
            if (isSequenced) {
                Debugger.LogError(_TxtCantChangeSequencedValues, this);
                return this;
            }
#if COMPATIBLE
            ConvertToWrapper(ref newEndValue);
#endif
            Type valT = newEndValue.GetType();
            if (!ValidateChangeValueType(valT, out bool isColor32ToColor)) {
                Debugger.LogError("ChangeEndValue: incorrect newEndValue type (is " + valT + ", should be " + typeofT2 + ")", this);
                return this;
            }
            if (isColor32ToColor) return DoChangeEndValue(this, (T2)(object)(Color)(Color32)newEndValue, newDuration, snapStartValue);
            return DoChangeEndValue(this, (T2)newEndValue, newDuration, snapStartValue);
        }

        // No generics because T to T2 conversion isn't compatible with AOT
        public override Tweener ChangeValues(object newStartValue, object newEndValue, float newDuration = -1)
        {
            if (isSequenced) {
                Debugger.LogError(_TxtCantChangeSequencedValues, this);
                return this;
            }
#if COMPATIBLE
            ConvertToWrapper(ref newStartValue);
            ConvertToWrapper(ref newEndValue);
#endif
            Type valT0 = newStartValue.GetType();
            Type valT1 = newEndValue.GetType();
            if (!ValidateChangeValueType(valT0, out bool isColor32ToColor)) {
                Debugger.LogError("ChangeValues: incorrect value type (is " + valT0 + ", should be " + typeofT2 + ")", this);
                return this;
            }
            if (!ValidateChangeValueType(valT1, out isColor32ToColor)) {
                Debugger.LogError("ChangeValues: incorrect value type (is " + valT1 + ", should be " + typeofT2 + ")", this);
                return this;
            }
            if (isColor32ToColor) return DoChangeValues(this, (T2)(object)(Color)(Color32)newStartValue, (T2)(object)(Color)(Color32)newEndValue, newDuration);
            return DoChangeValues(this, (T2)newStartValue, (T2)newEndValue, newDuration);
        }

        #region Advanced Usage (direct from TweenerCore reference)

        /// <summary>NO-GC METHOD: changes the start value of a tween and rewinds it (without pausing it).
        /// Has no effect with tweens that are inside Sequences</summary>
        /// <param name="newStartValue">The new start value</param>
        /// <param name="newDuration">If bigger than 0 applies it as the new tween duration</param>
        public TweenerCore<T1,T2,TPlugOptions> ChangeStartValue(T2 newStartValue, float newDuration = -1)
        {
            if (isSequenced) {
                Debugger.LogError(_TxtCantChangeSequencedValues, this);
                return this;
            }
            return DoChangeStartValue(this, newStartValue, newDuration);
        }

        /// <summary>NO-GC METHOD: changes the end value of a tween and rewinds it (without pausing it).
        /// Has no effect with tweens that are inside Sequences</summary>
        /// <param name="newEndValue">The new end value</param>
        /// <param name="snapStartValue">If TRUE the start value will become the current target's value, otherwise it will stay the same</param>
        public TweenerCore<T1,T2,TPlugOptions> ChangeEndValue(T2 newEndValue, bool snapStartValue)
        { return ChangeEndValue(newEndValue, -1, snapStartValue); }
        /// <summary>NO-GC METHOD: changes the end value of a tween and rewinds it (without pausing it).
        /// Has no effect with tweens that are inside Sequences</summary>
        /// <param name="newEndValue">The new end value</param>
        /// <param name="newDuration">If bigger than 0 applies it as the new tween duration</param>
        /// <param name="snapStartValue">If TRUE the start value will become the current target's value, otherwise it will stay the same</param>
        public TweenerCore<T1,T2,TPlugOptions> ChangeEndValue(T2 newEndValue, float newDuration = -1, bool snapStartValue = false)
        {
            if (isSequenced) {
                Debugger.LogError(_TxtCantChangeSequencedValues, this);
                return this;
            }
            return DoChangeEndValue(this, newEndValue, newDuration, snapStartValue);
        }

        /// <summary>NO-GC METHOD: changes the start and end value of a tween and rewinds it (without pausing it).
        /// Has no effect with tweens that are inside Sequences</summary>
        /// <param name="newStartValue">The new start value</param>
        /// <param name="newEndValue">The new end value</param>
        /// <param name="newDuration">If bigger than 0 applies it as the new tween duration</param>
        public TweenerCore<T1,T2,TPlugOptions> ChangeValues(T2 newStartValue, T2 newEndValue, float newDuration = -1)
        {
            if (isSequenced) {
                Debugger.LogError(_TxtCantChangeSequencedValues, this);
                return this;
            }
            return DoChangeValues(this, newStartValue, newEndValue, newDuration);
        }

        #endregion

        #endregion

        // Sets From tweens, immediately sending the target to its endValue and assigning new start/endValues.
        // Called by TweenSettings.From.
        // Plugins that don't support From:
        // - Vector3ArrayPlugin
        // - Pro > PathPlugin, SpiralPlugin
        internal override Tweener SetFrom(bool relative)
        {
            tweenPlugin.SetFrom(this, relative);
            hasManuallySetStartValue = true;
            return this;
        }
        // Sets From tweens in an alternate way where you can set the start value directly
        // (instead of setting it from the endValue).
        // Plugins that don't support From:
        // - Vector3ArrayPlugin
        // - Pro > PathPlugin, SpiralPlugin
        internal Tweener SetFrom(T2 fromValue, bool setImmediately, bool relative)
        {
            tweenPlugin.SetFrom(this, fromValue, setImmediately, relative);
            hasManuallySetStartValue = true;
            return this;
        }

        // _tweenPlugin is not reset since it's useful to keep it as a reference
        internal sealed override void Reset()
        {
            base.Reset();

            if (tweenPlugin != null) tweenPlugin.Reset(this);
//            plugOptions = new TPlugOptions(); // Generates GC because converts to an Activator.CreateInstance
//            plugOptions = DOTweenUtils.InstanceCreator<TPlugOptions>.Create(); // Fixes GC allocation using workaround (doesn't work with IL2CPP)
            plugOptions.Reset(); // Alternate fix that uses IPlugOptions Reset
            getter = null;
            setter = null;
            hasManuallySetStartValue = false;
            isFromAllowed = true;
        }

        // Called by TweenManager.Validate.
        // Returns TRUE if the tween is valid
        internal override bool Validate()
        {
            try {
                getter();
            } catch {
                return false;
            }
            return true;
        }

        // Validates if a ChangeEnd/StartValue passed type is compatible with the current one
        bool ValidateChangeValueType(Type newType, out bool isColor32ToColor)
        {
            if (newType == typeofT2) {
                isColor32ToColor = false;
                return true;
            }
            if (typeofT2 == _colorType && newType == _color32Type) {
                isColor32ToColor = true;
                return true;
            }
            isColor32ToColor = false;
            return false;
        }

        // CALLED BY TweenManager at each update.
        // Returns TRUE if the tween needs to be killed
        internal override float UpdateDelay(float elapsed)
        {
            return DoUpdateDelay(this, elapsed);
        }

        // CALLED BY Tween the moment the tween starts, AFTER any delay has elapsed
        // (unless it's a FROM tween, in which case it will be called BEFORE any eventual delay).
        // Returns TRUE in case of success,
        // FALSE if there are missing references and the tween needs to be killed
        internal override bool Startup()
        {
            return DoStartup(this);
        }

        // Applies the tween set by DoGoto.
        // Returns TRUE if the tween needs to be killed
        internal override bool ApplyTween(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode, UpdateNotice updateNotice)
        {
            if (isInverted) useInversePosition = !useInversePosition;
            float updatePosition = useInversePosition ? duration - position : position;
            if (DOTween.useSafeMode) {
                try {
                    tweenPlugin.EvaluateAndApply(plugOptions, this, isRelative, getter, setter, updatePosition, startValue, changeValue, duration, useInversePosition, newCompletedSteps, updateNotice);
                } catch (Exception e) {
                    // Target/field doesn't exist anymore: kill tween
                    if (Debugger.ShouldLogSafeModeCapturedError()) {
                        Debugger.LogSafeModeCapturedError(string.Format(
                            "Target or field is missing/null ({0}) ► {1}\n\n{2}\n\n", e.TargetSite, e.Message, e.StackTrace
                        ), this);
                    }
                    DOTween.safeModeReport.Add(SafeModeReport.SafeModeReportType.TargetOrFieldMissing);
                    return true;
                }
            } else {
                tweenPlugin.EvaluateAndApply(plugOptions, this, isRelative, getter, setter, updatePosition, startValue, changeValue, duration, useInversePosition, newCompletedSteps, updateNotice);
            }
            return false;
        }

#if COMPATIBLE

        // Eventually converts a Unity struct to the correct wrapper
        static void ConvertToWrapper(ref object value)
        {
            Type t = value.GetType();
            if (t == typeof(Vector3)) value = (Vector3Wrapper)((Vector3)value);
            else if (t == typeof(Vector2)) value = (Vector2Wrapper)((Vector2)value);
            else if (t == typeof(Quaternion)) value = (QuaternionWrapper)((Quaternion)value);
            else if (t == typeof(Color)) value = (ColorWrapper)((Color)value);
            else if (t == typeof(Vector4)) value = (Vector4Wrapper)((Vector4)value);
        }

#endif
    }
}