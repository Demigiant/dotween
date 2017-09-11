// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/05 16:36
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

#if COMPATIBLE
using DOVector2 = DG.Tweening.Core.Surrogates.Vector2Wrapper;
using DOVector3 = DG.Tweening.Core.Surrogates.Vector3Wrapper;
using DOVector4 = DG.Tweening.Core.Surrogates.Vector4Wrapper;
using DOQuaternion = DG.Tweening.Core.Surrogates.QuaternionWrapper;
using DOColor = DG.Tweening.Core.Surrogates.ColorWrapper;
#else
using DOVector2 = UnityEngine.Vector2;
using DOVector3 = UnityEngine.Vector3;
using DOVector4 = UnityEngine.Vector4;
using DOQuaternion = UnityEngine.Quaternion;
using DOColor = UnityEngine.Color;
#endif
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#pragma warning disable 1573
namespace DG.Tweening
{
    /// <summary>
    /// Methods that extend Tween objects and allow to set their parameters
    /// </summary>
    public static class TweenSettingsExtensions
    {
        #region Tweeners + Sequences

        /// <summary>Sets the autoKill behaviour of the tween. 
        /// Has no effect if the tween has already started</summary>
        public static T SetAutoKill<T>(this T t) where T : Tween
        {
            if (t == null || !t.active || t.creationLocked) return t;

            t.autoKill = true;
            return t;
        }
        /// <summary>Sets the autoKill behaviour of the tween. 
        /// Has no effect if the tween has already started</summary>
        /// <param name="autoKillOnCompletion">If TRUE the tween will be automatically killed when complete</param>
        public static T SetAutoKill<T>(this T t, bool autoKillOnCompletion) where T : Tween
        {
            if (t == null || !t.active || t.creationLocked) return t;

            t.autoKill = autoKillOnCompletion;
            return t;
        }

        /// <summary>Sets an ID for the tween (<see cref="Tween.id"/>), which can then be used as a filter with DOTween's static methods.</summary>
        /// <param name="objectId">The ID to assign to this tween. Can be an int, a string, an object or anything else.</param>
        public static T SetId<T>(this T t, object objectId) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.id = objectId;
            return t;
        }
        /// <summary>Sets a string ID for the tween (<see cref="Tween.stringId"/>), which can then be used as a filter with DOTween's static methods.<para/>
        /// Filtering via string is 2X faster than using an object as an ID (using the alternate obejct overload)</summary>
        /// <param name="stringId">The string ID to assign to this tween.</param>
        public static T SetId<T>(this T t, string stringId) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.stringId = stringId;
            return t;
        }
        /// <summary>Sets an int ID for the tween (<see cref="Tween.intId"/>), which can then be used as a filter with DOTween's static methods.<para/>
        /// Filtering via int is 4X faster than via object, 2X faster than via string (using the alternate object/string overloads)</summary>
        /// <param name="intId">The int ID to assign to this tween.</param>
        public static T SetId<T>(this T t, int intId) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.intId = intId;
            return t;
        }

        /// <summary>Sets the target for the tween, which can then be used as a filter with DOTween's static methods.
        /// <para>IMPORTANT: use it with caution. If you just want to set an ID for the tween use <code>SetId</code> instead.</para>
        /// When using shorcuts the shortcut target is already assigned as the tween's target,
        /// so using this method will overwrite it and prevent shortcut-operations like myTarget.DOPause from working correctly.</summary>
        /// <param name="target">The target to assign to this tween. Can be an int, a string, an object or anything else.</param>
        public static T SetTarget<T>(this T t, object target) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.target = target;
            return t;
        }

        /// <summary>Sets the looping options for the tween. 
        /// Has no effect if the tween has already started</summary>
        /// <param name="loops">Number of cycles to play (-1 for infinite - will be converted to 1 in case the tween is nested in a Sequence)</param>
        public static T SetLoops<T>(this T t, int loops) where T : Tween
        {
            if (t == null || !t.active || t.creationLocked) return t;

            if (loops < -1) loops = -1;
            else if (loops == 0) loops = 1;
            t.loops = loops;
//            if (t.tweenType == TweenType.Tweener) t.fullDuration = loops > -1 ? t.duration * loops : Mathf.Infinity; // Mysteriously Unity doesn't like this form
            if (t.tweenType == TweenType.Tweener) {
                if (loops > -1) t.fullDuration = t.duration * loops;
                else t.fullDuration = Mathf.Infinity;
            }
            return t;
        }
        /// <summary>Sets the looping options for the tween. 
        /// Has no effect if the tween has already started</summary>
        /// <param name="loops">Number of cycles to play (-1 for infinite - will be converted to 1 in case the tween is nested in a Sequence)</param>
        /// <param name="loopType">Loop behaviour type (default: LoopType.Restart)</param>
        public static T SetLoops<T>(this T t, int loops, LoopType loopType) where T : Tween
        {
            if (t == null || !t.active || t.creationLocked) return t;

            if (loops < -1) loops = -1;
            else if (loops == 0) loops = 1;
            t.loops = loops;
            t.loopType = loopType;
//            if (t.tweenType == TweenType.Tweener) t.fullDuration = loops > -1 ? t.duration * loops : Mathf.Infinity;
            if (t.tweenType == TweenType.Tweener) {
                if (loops > -1) t.fullDuration = t.duration * loops;
                else t.fullDuration = Mathf.Infinity;
            }
            return t;
        }

        /// <summary>Sets the ease of the tween.
        /// <para>If applied to Sequences eases the whole sequence animation</para></summary>
        public static T SetEase<T>(this T t, Ease ease) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.easeType = ease;
            if (EaseManager.IsFlashEase(ease)) t.easeOvershootOrAmplitude = (int)t.easeOvershootOrAmplitude;
            
            t.customEase = null;
            return t;
        }
        /// <summary>Sets the ease of the tween.
        /// <para>If applied to Sequences eases the whole sequence animation</para></summary>
        /// <param name="overshoot">
        /// Eventual overshoot to use with Back or Flash ease (default is 1.70158 - 1 for Flash).
        /// <para>In case of Flash ease it must be an intenger and sets the total number of flashes that will happen.
        /// Using an even number will complete the tween on the starting value, while an odd one will complete it on the end value.</para>
        /// </param>
        public static T SetEase<T>(this T t, Ease ease, float overshoot) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.easeType = ease;
            if (EaseManager.IsFlashEase(ease)) overshoot = (int)overshoot;
            t.easeOvershootOrAmplitude = overshoot;
            t.customEase = null;
            return t;
        }
        /// <summary>Sets the ease of the tween.
        /// <para>If applied to Sequences eases the whole sequence animation</para></summary>
        /// <param name="amplitude">Eventual amplitude to use with Elastic easeType or overshoot to use with Flash easeType (default is 1.70158 - 1 for Flash).
        /// <para>In case of Flash ease it must be an integer and sets the total number of flashes that will happen.
        /// Using an even number will complete the tween on the starting value, while an odd one will complete it on the end value.</para>
        /// </param>
        /// <param name="period">Eventual period to use with Elastic or Flash easeType (default is 0).
        /// <para>In case of Flash ease it indicates the power in time of the ease, and must be between -1 and 1.
        /// 0 is balanced, 1 weakens the ease with time, -1 starts the ease weakened and gives it power towards the end.</para>
        /// </param>
        public static T SetEase<T>(this T t, Ease ease, float amplitude, float period) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.easeType = ease;
            if (EaseManager.IsFlashEase(ease)) amplitude = (int)amplitude;
            t.easeOvershootOrAmplitude = amplitude;
            t.easePeriod = period;
            t.customEase = null;
            return t;
        }
        /// <summary>Sets the ease of the tween using an AnimationCurve.
        /// <para>If applied to Sequences eases the whole sequence animation</para></summary>
        public static T SetEase<T>(this T t, AnimationCurve animCurve) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.easeType = Ease.INTERNAL_Custom;
            t.customEase = new EaseCurve(animCurve).Evaluate;
            return t;
        }
        /// <summary>Sets the ease of the tween using a custom ease function (which must return a value between 0 and 1).
        /// <para>If applied to Sequences eases the whole sequence animation</para></summary>
        public static T SetEase<T>(this T t, EaseFunction customEase) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.easeType = Ease.INTERNAL_Custom;
            t.customEase = customEase;
            return t;
        }

        /// <summary>Allows the tween to be recycled after being killed.</summary>
        public static T SetRecyclable<T>(this T t) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.isRecyclable = true;
            return t;
        }
        /// <summary>Sets the recycling behaviour for the tween.</summary>
        /// <param name="recyclable">If TRUE the tween will be recycled after being killed, otherwise it will be destroyed.</param>
        public static T SetRecyclable<T>(this T t, bool recyclable) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.isRecyclable = recyclable;
            return t;
        }

        /// <summary>Sets the update type to UpdateType.Normal and lets you choose if it should be independent from Unity's Time.timeScale</summary>
        /// <param name="isIndependentUpdate">If TRUE the tween will ignore Unity's Time.timeScale</param>
        public static T SetUpdate<T>(this T t, bool isIndependentUpdate) where T : Tween
        {
            if (t == null || !t.active) return t;

            TweenManager.SetUpdateType(t, DOTween.defaultUpdateType, isIndependentUpdate);
            return t;
        }
        /// <summary>Sets the type of update for the tween</summary>
        /// <param name="updateType">The type of update (defalt: UpdateType.Normal)</param>
        public static T SetUpdate<T>(this T t, UpdateType updateType) where T : Tween
        {
            if (t == null || !t.active) return t;

            TweenManager.SetUpdateType(t, updateType, DOTween.defaultTimeScaleIndependent);
            return t;
        }
        /// <summary>Sets the type of update for the tween and lets you choose if it should be independent from Unity's Time.timeScale</summary>
        /// <param name="updateType">The type of update</param>
        /// <param name="isIndependentUpdate">If TRUE the tween will ignore Unity's Time.timeScale</param>
        public static T SetUpdate<T>(this T t, UpdateType updateType, bool isIndependentUpdate) where T : Tween
        {
            if (t == null || !t.active) return t;

            TweenManager.SetUpdateType(t, updateType, isIndependentUpdate);
            return t;
        }

        /// <summary>Sets the <code>onStart</code> callback for the tween, clearing any previous <code>onStart</code> callback that was set.
        /// Called the first time the tween is set in a playing state, after any eventual delay</summary>
        public static T OnStart<T>(this T t, TweenCallback action) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.onStart = action;
            return t;
        }

        /// <summary>Sets the <code>onPlay</code> callback for the tween, clearing any previous <code>onPlay</code> callback that was set.
        /// Called when the tween is set in a playing state, after any eventual delay.
        /// Also called each time the tween resumes playing from a paused state</summary>
        public static T OnPlay<T>(this T t, TweenCallback action) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.onPlay = action;
            return t;
        }

        /// <summary>Sets the <code>onPause</code> callback for the tween, clearing any previous <code>onPause</code> callback that was set.
        /// Called when the tween state changes from playing to paused.
        /// If the tween has autoKill set to FALSE, this is called also when the tween reaches completion.</summary>
        public static T OnPause<T>(this T t, TweenCallback action) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.onPause = action;
            return t;
        }

        /// <summary>Sets the <code>onRewind</code> callback for the tween, clearing any previous <code>onRewind</code> callback that was set.
        /// Called when the tween is rewinded,
        /// either by calling <code>Rewind</code> or by reaching the start position while playing backwards.
        /// Rewinding a tween that is already rewinded will not fire this callback</summary>
        public static T OnRewind<T>(this T t, TweenCallback action) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.onRewind = action;
            return t;
        }

        /// <summary>Sets the <code>onUpdate</code> callback for the tween, clearing any previous <code>onUpdate</code> callback that was set.
        /// Called each time the tween updates</summary>
        public static T OnUpdate<T>(this T t, TweenCallback action) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.onUpdate = action;
            return t;
        }

        /// <summary>Sets the <code>onStepComplete</code> callback for the tween, clearing any previous <code>onStepComplete</code> callback that was set.
        /// Called the moment the tween completes one loop cycle, even when going backwards</summary>
        public static T OnStepComplete<T>(this T t, TweenCallback action) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.onStepComplete = action;
            return t;
        }

        /// <summary>Sets the <code>onComplete</code> callback for the tween, clearing any previous <code>onComplete</code> callback that was set.
        /// Called the moment the tween reaches its final forward position, loops included</summary>
        public static T OnComplete<T>(this T t, TweenCallback action) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.onComplete = action;
            return t;
        }

        /// <summary>Sets the <code>onKill</code> callback for the tween, clearing any previous <code>onKill</code> callback that was set.
        /// Called the moment the tween is killed</summary>
        public static T OnKill<T>(this T t, TweenCallback action) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.onKill = action;
            return t;
        }

        /// <summary>Sets the <code>onWaypointChange</code> callback for the tween, clearing any previous <code>onWaypointChange</code> callback that was set.
        /// Called when a path tween's current waypoint changes</summary>
        public static T OnWaypointChange<T>(this T t, TweenCallback<int> action) where T : Tween
        {
            if (t == null || !t.active) return t;

            t.onWaypointChange = action;
            return t;
        }

        /// <summary>Sets the parameters of the tween (id, ease, loops, delay, timeScale, callbacks, etc) as the parameters of the given one.
        /// Doesn't copy specific SetOptions settings: those will need to be applied manually each time.
        /// <para>Has no effect if the tween has already started.</para>
        /// NOTE: the tween's <code>target</code> will not be changed</summary>
        /// <param name="asTween">Tween from which to copy the parameters</param>
        public static T SetAs<T>(this T t, Tween asTween) where T : Tween
        {
            if (t == null || !t.active || t.creationLocked) return t;

//            t.isFrom = asTween.isFrom;
//            t.target = asTween.target;

            t.timeScale = asTween.timeScale;
            t.isBackwards = asTween.isBackwards;
            TweenManager.SetUpdateType(t, asTween.updateType, asTween.isIndependentUpdate);
            t.id = asTween.id;
            t.onStart = asTween.onStart;
            t.onPlay = asTween.onPlay;
            t.onRewind = asTween.onRewind;
            t.onUpdate = asTween.onUpdate;
            t.onStepComplete = asTween.onStepComplete;
            t.onComplete = asTween.onComplete;
            t.onKill = asTween.onKill;
            t.onWaypointChange = asTween.onWaypointChange;

            t.isRecyclable = asTween.isRecyclable;
            t.isSpeedBased = asTween.isSpeedBased;
            t.autoKill = asTween.autoKill;
            t.loops = asTween.loops;
            t.loopType = asTween.loopType;
            if (t.tweenType == TweenType.Tweener) {
                if (t.loops > -1) t.fullDuration = t.duration * t.loops;
                else t.fullDuration = Mathf.Infinity;
            }

            t.delay = asTween.delay;
            t.delayComplete = t.delay <= 0;
            t.isRelative = asTween.isRelative;
            t.easeType = asTween.easeType;
            t.customEase = asTween.customEase;
            t.easeOvershootOrAmplitude = asTween.easeOvershootOrAmplitude;
            t.easePeriod = asTween.easePeriod;

            return t;
        }

        /// <summary>Sets the parameters of the tween (id, ease, loops, delay, timeScale, callbacks, etc) as the parameters of the given TweenParams.
        /// <para>Has no effect if the tween has already started.</para></summary>
        /// <param name="tweenParams">TweenParams from which to copy the parameters</param>
        public static T SetAs<T>(this T t, TweenParams tweenParams) where T : Tween
        {
            if (t == null || !t.active || t.creationLocked) return t;

            TweenManager.SetUpdateType(t, tweenParams.updateType, tweenParams.isIndependentUpdate);
            t.id = tweenParams.id;
            t.onStart = tweenParams.onStart;
            t.onPlay = tweenParams.onPlay;
            t.onRewind = tweenParams.onRewind;
            t.onUpdate = tweenParams.onUpdate;
            t.onStepComplete = tweenParams.onStepComplete;
            t.onComplete = tweenParams.onComplete;
            t.onKill = tweenParams.onKill;
            t.onWaypointChange = tweenParams.onWaypointChange;

            t.isRecyclable = tweenParams.isRecyclable;
            t.isSpeedBased = tweenParams.isSpeedBased;
            t.autoKill = tweenParams.autoKill;
            t.loops = tweenParams.loops;
            t.loopType = tweenParams.loopType;
            if (t.tweenType == TweenType.Tweener) {
                if (t.loops > -1) t.fullDuration = t.duration * t.loops;
                else t.fullDuration = Mathf.Infinity;
            }

            t.delay = tweenParams.delay;
            t.delayComplete = t.delay <= 0;
            t.isRelative = tweenParams.isRelative;
            if (tweenParams.easeType == Ease.Unset) {
                if (t.tweenType == TweenType.Sequence) t.easeType = Ease.Linear;
                else t.easeType = DOTween.defaultEaseType;
//                t.easeType = t.tweenType == TweenType.Sequence ? Ease.Linear : DOTween.defaultEaseType; // Doesn't work w webplayer (why?)
            } else t.easeType = tweenParams.easeType;
            t.customEase = tweenParams.customEase;
            t.easeOvershootOrAmplitude = tweenParams.easeOvershootOrAmplitude;
            t.easePeriod = tweenParams.easePeriod;

            return t;
        }

        #endregion

        #region Sequences-only

        /// <summary>Adds the given tween to the end of the Sequence. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="t">The tween to append</param>
        public static Sequence Append(this Sequence s, Tween t)
        {
            if (s == null || !s.active || s.creationLocked) return s;
            if (t == null || !t.active || t.isSequenced) return s;

            Sequence.DoInsert(s, t, s.duration);
            return s;
        }
        /// <summary>Adds the given tween to the beginning of the Sequence, pushing forward the other nested content. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="t">The tween to prepend</param>
        public static Sequence Prepend(this Sequence s, Tween t)
        {
            if (s == null || !s.active || s.creationLocked) return s;
            if (t == null || !t.active || t.isSequenced) return s;

            Sequence.DoPrepend(s, t);
            return s;
        }
        /// <summary>Inserts the given tween at the same time position of the last tween, callback or intervale added to the Sequence.
        /// Note that, in case of a Join after an interval, the insertion time will be the time where the interval starts, not where it finishes.
        /// Has no effect if the Sequence has already started</summary>
        public static Sequence Join(this Sequence s, Tween t)
        {
            if (s == null || !s.active || s.creationLocked) return s;
            if (t == null || !t.active || t.isSequenced) return s;

            Sequence.DoInsert(s, t, s.lastTweenInsertTime);
            return s;
        }
        /// <summary>Inserts the given tween at the given time position in the Sequence,
        /// automatically adding an interval if needed. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="atPosition">The time position where the tween will be placed</param>
        /// <param name="t">The tween to insert</param>
        public static Sequence Insert(this Sequence s, float atPosition, Tween t)
        {
            if (s == null || !s.active || s.creationLocked) return s;
            if (t == null || !t.active || t.isSequenced) return s;

            Sequence.DoInsert(s, t, atPosition);
            return s;
        }

        /// <summary>Adds the given interval to the end of the Sequence. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="interval">The interval duration</param>
        public static Sequence AppendInterval(this Sequence s, float interval)
        {
            if (s == null || !s.active || s.creationLocked) return s;

            Sequence.DoAppendInterval(s, interval);
            return s;
        }
        /// <summary>Adds the given interval to the beginning of the Sequence, pushing forward the other nested content. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="interval">The interval duration</param>
        public static Sequence PrependInterval(this Sequence s, float interval)
        {
            if (s == null || !s.active || s.creationLocked) return s;

            Sequence.DoPrependInterval(s, interval);
            return s;
        }

        /// <summary>Adds the given callback to the end of the Sequence. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="callback">The callback to append</param>
        public static Sequence AppendCallback(this Sequence s, TweenCallback callback)
        {
            if (s == null || !s.active || s.creationLocked) return s;
            if (callback == null) return s;

            Sequence.DoInsertCallback(s, callback, s.duration);
            return s;
        }
        /// <summary>Adds the given callback to the beginning of the Sequence, pushing forward the other nested content. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="callback">The callback to prepend</param>
        public static Sequence PrependCallback(this Sequence s, TweenCallback callback)
        {
            if (s == null || !s.active || s.creationLocked) return s;
            if (callback == null) return s;

            Sequence.DoInsertCallback(s, callback, 0);
            return s;
        }
        /// <summary>Inserts the given callback at the given time position in the Sequence,
        /// automatically adding an interval if needed. 
        /// Has no effect if the Sequence has already started</summary>
        /// <param name="atPosition">The time position where the callback will be placed</param>
        /// <param name="callback">The callback to insert</param>
        public static Sequence InsertCallback(this Sequence s, float atPosition, TweenCallback callback)
        {
            if (s == null || !s.active || s.creationLocked) return s;
            if (callback == null) return s;

            Sequence.DoInsertCallback(s, callback, atPosition);
            return s;
        }
        #endregion

        #region Tweeners-only

        /// <summary>Changes a TO tween into a FROM tween: sets the current target's position as the tween's endValue
        /// then immediately sends the target to the previously set endValue.</summary>
        public static T From<T>(this T t) where T : Tweener
        {
            if (t == null || !t.active || t.creationLocked || !t.isFromAllowed) return t;

            t.isFrom = true;
            t.SetFrom(false);
            return t;
        }
        /// <summary>Changes a TO tween into a FROM tween: sets the current target's position as the tween's endValue
        /// then immediately sends the target to the previously set endValue.</summary>
        /// <param name="isRelative">If TRUE the FROM value will be calculated as relative to the current one</param>
        public static T From<T>(this T t, bool isRelative) where T : Tweener
        {
            if (t == null || !t.active || t.creationLocked || !t.isFromAllowed) return t;

            t.isFrom = true;
            if (!isRelative) t.SetFrom(false);
            else t.SetFrom(!t.isBlendable);
            return t;
        }

        /// <summary>Sets a delayed startup for the tween.
        /// <para>Has no effect on Sequences or if the tween has already started</para></summary>
        public static T SetDelay<T>(this T t, float delay) where T : Tween
        {
            if (t == null || !t.active || t.creationLocked) return t;

            if (t.tweenType == TweenType.Sequence) {
                (t as Sequence).PrependInterval(delay);
            } else {
                t.delay = delay;
                t.delayComplete = delay <= 0;
            }
            return t;
        }

        /// <summary>Sets the tween as relative
        /// (the endValue will be calculated as <code>startValue + endValue</code> instead than being used directly).
        /// <para>Has no effect on Sequences or if the tween has already started</para></summary>
        public static T SetRelative<T>(this T t) where T : Tween
        {
            if (t == null || !t.active || t.creationLocked || t.isFrom || t.isBlendable) return t;

            t.isRelative = true;
            return t;
        }
        /// <summary>If isRelative is TRUE sets the tween as relative
        /// (the endValue will be calculated as <code>startValue + endValue</code> instead than being used directly).
        /// <para>Has no effect on Sequences or if the tween has already started</para></summary>
        public static T SetRelative<T>(this T t, bool isRelative) where T : Tween
        {
            if (t == null || !t.active || t.creationLocked || t.isFrom || t.isBlendable) return t;

            t.isRelative = isRelative;
            return t;
        }

        /// <summary>If isSpeedBased is TRUE sets the tween as speed based
        /// (the duration will represent the number of units the tween moves x second).
        /// <para>Has no effect on Sequences, nested tweens, or if the tween has already started</para></summary>
        public static T SetSpeedBased<T>(this T t) where T : Tween
        {
            if (t == null || !t.active || t.creationLocked) return t;

            t.isSpeedBased = true;
            return t;
        }
        /// <summary>If isSpeedBased is TRUE sets the tween as speed based
        /// (the duration will represent the number of units the tween moves x second).
        /// <para>Has no effect on Sequences, nested tweens, or if the tween has already started</para></summary>
        public static T SetSpeedBased<T>(this T t, bool isSpeedBased) where T : Tween
        {
            if (t == null || !t.active || t.creationLocked) return t;

            t.isSpeedBased = isSpeedBased;
            return t;
        }

        #endregion

        #region Tweeners Extra Options

        /// <summary>Options for float tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<float, float, FloatOptions> t, bool snapping)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.snapping = snapping;
            return t;
        }

        /// <summary>Options for Vector2 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<DOVector2, DOVector2, VectorOptions> t, bool snapping)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.snapping = snapping;
            return t;
        }
        /// <summary>Options for Vector2 tweens</summary>
        /// <param name="axisConstraint">Selecting an axis will tween the vector only on that axis, leaving the others untouched</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<DOVector2, DOVector2, VectorOptions> t, AxisConstraint axisConstraint, bool snapping = false)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.axisConstraint = axisConstraint;
            t.plugOptions.snapping = snapping;
            return t;
        }

        /// <summary>Options for Vector3 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<DOVector3, DOVector3, VectorOptions> t, bool snapping)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.snapping = snapping;
            return t;
        }
        /// <summary>Options for Vector3 tweens</summary>
        /// <param name="axisConstraint">Selecting an axis will tween the vector only on that axis, leaving the others untouched</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<DOVector3, DOVector3, VectorOptions> t, AxisConstraint axisConstraint, bool snapping = false)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.axisConstraint = axisConstraint;
            t.plugOptions.snapping = snapping;
            return t;
        }

        /// <summary>Options for Vector4 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<DOVector4, DOVector4, VectorOptions> t, bool snapping)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.snapping = snapping;
            return t;
        }
        /// <summary>Options for Vector4 tweens</summary>
        /// <param name="axisConstraint">Selecting an axis will tween the vector only on that axis, leaving the others untouched</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<DOVector4, DOVector4, VectorOptions> t, AxisConstraint axisConstraint, bool snapping = false)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.axisConstraint = axisConstraint;
            t.plugOptions.snapping = snapping;
            return t;
        }

        /// <summary>Options for Quaternion tweens</summary>
        /// <param name="useShortest360Route">If TRUE (default) the rotation will take the shortest route, and will not rotate more than 360°.
        /// If FALSE the rotation will be fully accounted. Is always FALSE if the tween is set as relative</param>
        public static Tweener SetOptions(this TweenerCore<DOQuaternion, DOVector3, QuaternionOptions> t, bool useShortest360Route = true)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.rotateMode = useShortest360Route ? RotateMode.Fast : RotateMode.FastBeyond360;
            return t;
        }

        /// <summary>Options for Color tweens</summary>
        /// <param name="alphaOnly">If TRUE only the alpha value of the color will be tweened</param>
        public static Tweener SetOptions(this TweenerCore<DOColor, DOColor, ColorOptions> t, bool alphaOnly)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.alphaOnly = alphaOnly;
            return t;
        }

        /// <summary>Options for Vector4 tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Rect, Rect, RectOptions> t, bool snapping)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.snapping = snapping;
            return t;
        }

        /// <summary>Options for Vector4 tweens</summary>
        /// <param name="richTextEnabled">If TRUE, rich text will be interpreted correctly while animated,
        /// otherwise all tags will be considered as normal text</param>
        /// <param name="scrambleMode">The type of scramble to use, if any</param>
        /// <param name="scrambleChars">A string containing the characters to use for scrambling.
        /// Use as many characters as possible (minimum 10) because DOTween uses a fast scramble mode which gives better results with more characters.
        /// Leave it to NULL to use default ones</param>
        public static Tweener SetOptions(this TweenerCore<string, string, StringOptions> t, bool richTextEnabled, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.richTextEnabled = richTextEnabled;
            t.plugOptions.scrambleMode = scrambleMode;
            if (!string.IsNullOrEmpty(scrambleChars)) {
                if (scrambleChars.Length <= 1) scrambleChars += scrambleChars;
                t.plugOptions.scrambledChars = scrambleChars.ToCharArray();
                t.plugOptions.scrambledChars.ScrambleChars();
            }
            return t;
        }

        /// <summary>Options for Vector3Array tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, bool snapping)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.snapping = snapping;
            return t;
        }
        /// <summary>Options for Vector3Array tweens</summary>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener SetOptions(this TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, AxisConstraint axisConstraint, bool snapping = false)
        {
            if (t == null || !t.active) return t;

            t.plugOptions.axisConstraint = axisConstraint;
            t.plugOptions.snapping = snapping;
            return t;
        }

        #region Path Options

        /// <summary>Options for Path tweens (created via the <code>DOPath</code> shortcut)</summary>
        /// <param name="lockPosition">The eventual movement axis to lock. You can input multiple axis if you separate them like this:
        /// <para>AxisConstrain.X | AxisConstraint.Y</para></param>
        /// <param name="lockRotation">The eventual rotation axis to lock. You can input multiple axis if you separate them like this:
        /// <para>AxisConstrain.X | AxisConstraint.Y</para></param>
        public static TweenerCore<Vector3, Path, PathOptions> SetOptions(
            this TweenerCore<Vector3, Path, PathOptions> t,
            AxisConstraint lockPosition, AxisConstraint lockRotation = AxisConstraint.None
        )
        {
            return SetOptions(t, false, lockPosition, lockRotation);
        }
        /// <summary>Options for Path tweens (created via the <code>DOPath</code> shortcut)</summary>
        /// <param name="closePath">If TRUE the path will be automatically closed</param>
        /// <param name="lockPosition">The eventual movement axis to lock. You can input multiple axis if you separate them like this:
        /// <para>AxisConstrain.X | AxisConstraint.Y</para></param>
        /// <param name="lockRotation">The eventual rotation axis to lock. You can input multiple axis if you separate them like this:
        /// <para>AxisConstrain.X | AxisConstraint.Y</para></param>
        public static TweenerCore<Vector3, Path, PathOptions> SetOptions(
            this TweenerCore<Vector3, Path, PathOptions> t,
            bool closePath, AxisConstraint lockPosition = AxisConstraint.None, AxisConstraint lockRotation = AxisConstraint.None
        )
        {
            if (t == null || !t.active) return t;

            t.plugOptions.isClosedPath = closePath;
            t.plugOptions.lockPositionAxis = lockPosition;
            t.plugOptions.lockRotationAxis = lockRotation;
            return t;
        }

        /// <summary>Additional LookAt options for Path tweens (created via the <code>DOPath</code> shortcut).
        /// Orients the target towards the given position.
        /// Must be chained directly to the tween creation method or to a <code>SetOptions</code></summary>
        /// <param name="lookAtPosition">The position to look at</param>
        /// <param name="forwardDirection">The eventual direction to consider as "forward".
        /// If left to NULL defaults to the regular forward side of the transform</param>
        /// <param name="up">The vector that defines in which direction up is (default: Vector3.up)</param>
        public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(
            this TweenerCore<Vector3, Path, PathOptions> t, Vector3 lookAtPosition, Vector3? forwardDirection = null, Vector3? up = null
        )
        {
            if (t == null || !t.active) return t;

            t.plugOptions.orientType = OrientType.LookAtPosition;
            t.plugOptions.lookAtPosition = lookAtPosition;
            SetPathForwardDirection(t, forwardDirection, up);
            return t;
        }
        /// <summary>Additional LookAt options for Path tweens (created via the <code>DOPath</code> shortcut).
        /// Orients the target towards another transform.
        /// Must be chained directly to the tween creation method or to a <code>SetOptions</code></summary>
        /// <param name="lookAtTransform">The transform to look at</param>
        /// <param name="forwardDirection">The eventual direction to consider as "forward".
        /// If left to NULL defaults to the regular forward side of the transform</param>
        /// <param name="up">The vector that defines in which direction up is (default: Vector3.up)</param>
        public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(
            this TweenerCore<Vector3, Path, PathOptions> t, Transform lookAtTransform, Vector3? forwardDirection = null, Vector3? up = null
        )
        {
            if (t == null || !t.active) return t;

            t.plugOptions.orientType = OrientType.LookAtTransform;
            t.plugOptions.lookAtTransform = lookAtTransform;
            SetPathForwardDirection(t, forwardDirection, up);
            return t;
        }
        /// <summary>Additional LookAt options for Path tweens (created via the <code>DOPath</code> shortcut).
        /// Orients the target to the path, with the given lookAhead.
        /// Must be chained directly to the tween creation method or to a <code>SetOptions</code></summary>
        /// <param name="lookAhead">The percentage of lookAhead to use (0 to 1)</param>
        /// <param name="forwardDirection">The eventual direction to consider as "forward".
        /// If left to NULL defaults to the regular forward side of the transform</param>
        /// <param name="up">The vector that defines in which direction up is (default: Vector3.up)</param>
        public static TweenerCore<Vector3, Path, PathOptions> SetLookAt(
            this TweenerCore<Vector3, Path, PathOptions> t, float lookAhead, Vector3? forwardDirection = null, Vector3? up = null
        )
        {
            if (t == null || !t.active) return t;

            t.plugOptions.orientType = OrientType.ToPath;
            if (lookAhead < PathPlugin.MinLookAhead) lookAhead = PathPlugin.MinLookAhead;
            t.plugOptions.lookAhead = lookAhead;
            SetPathForwardDirection(t, forwardDirection, up);
            return t;
        }

        static void SetPathForwardDirection(this TweenerCore<Vector3, Path, PathOptions> t, Vector3? forwardDirection = null, Vector3? up = null)
        {
            if (t == null || !t.active) return;

            t.plugOptions.hasCustomForwardDirection = forwardDirection != null && forwardDirection != Vector3.zero || up != null && up != Vector3.zero;
            if (t.plugOptions.hasCustomForwardDirection) {
                if (forwardDirection == Vector3.zero) forwardDirection = Vector3.forward;
                t.plugOptions.forward = Quaternion.LookRotation(
                    forwardDirection == null ? Vector3.forward : (Vector3)forwardDirection,
                    up == null ? Vector3.up : (Vector3)up
                );
            }
        }

        #endregion

        #endregion
    }
}