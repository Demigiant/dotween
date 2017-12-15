// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/07 14:05
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
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
    /// <summary>
    /// Main DOTween class. Contains static methods to create and control tweens in a generic way
    /// </summary>
    public class DOTween
    {
        /// <summary>DOTween's version</summary>
        public static readonly string Version = "1.1.690";

        ///////////////////////////////////////////////
        // Options ////////////////////////////////////

        /// <summary>If TRUE (default) makes tweens slightly slower but safer, automatically taking care of a series of things
        /// (like targets becoming null while a tween is playing).
        /// <para>Default: TRUE</para></summary>
        public static bool useSafeMode = true;
        /// <summary>If TRUE you will get a DOTween report when exiting play mode (only in the Editor).
        /// Useful to know how many max Tweeners and Sequences you reached and optimize your final project accordingly.
        /// Beware, this will slightly slow down your tweens while inside Unity Editor.
        /// <para>Default: FALSE</para></summary>
        public static bool showUnityEditorReport = false;
        /// <summary>Global DOTween timeScale.
        /// <para>Default: 1</para></summary>
        public static float timeScale = 1;
        /// <summary>If TRUE, DOTween will use Time.smoothDeltaTime instead of Time.deltaTime for UpdateType.Normal and UpdateType.Late tweens
        /// (unless they're set as timeScaleIndependent, in which case a value between the last timestep
        /// and <see cref="maxSmoothUnscaledTime"/> will be used instead).
        /// Setting this to TRUE will lead to smoother animations.
        /// <para>Default: FALSE</para></summary>
        public static bool useSmoothDeltaTime;
        /// <summary>If <see cref="useSmoothDeltaTime"/> is TRUE, this indicates the max timeStep that an independent update call can last.
        /// Setting this to TRUE will lead to smoother animations.
        /// <para>Default: FALSE</para></summary>
        public static float maxSmoothUnscaledTime = 0.15f;
        // Internal ► Can only be set via DOTween's Utility Panel
        internal static RewindCallbackMode rewindCallbackMode = RewindCallbackMode.FireIfPositionChanged;
        /// <summary>DOTween's log behaviour.
        /// <para>Default: LogBehaviour.ErrorsOnly</para></summary>
        public static LogBehaviour logBehaviour {
            get { return _logBehaviour; }
            set { _logBehaviour = value; Debugger.SetLogPriority(_logBehaviour); }
        }
        static LogBehaviour _logBehaviour = LogBehaviour.ErrorsOnly;
        /// <summary>If TRUE draws path gizmos in Unity Editor (if the gizmos button is active).
        /// Deactivate this if you want to avoid gizmos overhead while in Unity Editor</summary>
        public static bool drawGizmos = true;

        ///////////////////////////////////////////////
        // Default options for Tweens /////////////////

        /// <summary>Default updateType for new tweens.
        /// <para>Default: UpdateType.Normal</para></summary>
        public static UpdateType defaultUpdateType = UpdateType.Normal;
        /// <summary>Sets whether Unity's timeScale should be taken into account by default or not.
        /// <para>Default: false</para></summary>
        public static bool defaultTimeScaleIndependent = false;
        /// <summary>Default autoPlay behaviour for new tweens.
        /// <para>Default: AutoPlay.All</para></summary>
        public static AutoPlay defaultAutoPlay = AutoPlay.All;
        /// <summary>Default autoKillOnComplete behaviour for new tweens.
        /// <para>Default: TRUE</para></summary>
        public static bool defaultAutoKill = true;
        /// <summary>Default loopType applied to all new tweens.
        /// <para>Default: LoopType.Restart</para></summary>
        public static LoopType defaultLoopType = LoopType.Restart;
        /// <summary>If TRUE all newly created tweens are set as recyclable, otherwise not.
        /// <para>Default: FALSE</para></summary>
        public static bool defaultRecyclable;
        /// <summary>Default ease applied to all new Tweeners (not to Sequences which always have Ease.Linear as default).
        /// <para>Default: Ease.InOutQuad</para></summary>
        public static Ease defaultEaseType = Ease.OutQuad;
        /// <summary>Default overshoot/amplitude used for eases
        /// <para>Default: 1.70158f</para></summary>
        public static float defaultEaseOvershootOrAmplitude = 1.70158f;
        /// <summary>Default period used for eases
        /// <para>Default: 0</para></summary>
        public static float defaultEasePeriod = 0;

        internal static DOTweenComponent instance; // Assigned/removed by DOTweenComponent.Create/DestroyInstance
        internal static bool isUnityEditor;
        internal static bool isDebugBuild;
        internal static int maxActiveTweenersReached, maxActiveSequencesReached; // Controlled by DOTweenInspector if showUnityEditorReport is active
        internal static readonly List<TweenCallback> GizmosDelegates = new List<TweenCallback>(); // Can be used by other classes to call internal gizmo draw methods
        internal static bool initialized; // Can be set to false by DOTweenComponent OnDestroy
        internal static bool isQuitting; // Set by DOTweenComponent when the application is quitting

        #region Static Constructor

        static DOTween()
        {
            isUnityEditor = Application.isEditor;
#if DEBUG
            isDebugBuild = true;
#endif
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Must be called once, before the first ever DOTween call/reference,
        /// otherwise it will be called automatically and will use default options.
        /// Calling it a second time won't have any effect.
        /// <para>You can chain <code>SetCapacity</code> to this method, to directly set the max starting size of Tweeners and Sequences:</para>
        /// <code>DOTween.Init(false, false, LogBehaviour.Default).SetCapacity(100, 20);</code>
        /// </summary>
        /// <param name="recycleAllByDefault">If TRUE all new tweens will be set for recycling, meaning that when killed,
        /// instead of being destroyed, they will be put in a pool and reused instead of creating new tweens. This option allows you to avoid
        /// GC allocations by reusing tweens, but you will have to take care of tween references, since they might result active
        /// even if they were killed (since they might have been respawned and are now being used for other tweens).
        /// <para>If you want to automatically set your tween references to NULL when a tween is killed 
        /// you can use the OnKill callback like this:</para>
        /// <code>.OnKill(()=> myTweenReference = null)</code>
        /// <para>You can change this setting at any time by changing the static <see cref="DOTween.defaultRecyclable"/> property,
        /// or you can set the recycling behaviour for each tween separately, using:</para>
        /// <para><code>SetRecyclable(bool recyclable)</code></para>
        /// <para>Default: FALSE</para></param>
        /// <param name="useSafeMode">If TRUE makes tweens slightly slower but safer, automatically taking care of a series of things
        /// (like targets becoming null while a tween is playing).
        /// You can change this setting at any time by changing the static <see cref="DOTween.useSafeMode"/> property.
        /// <para>Default: FALSE</para></param>
        /// <param name="logBehaviour">Type of logging to use.
        /// You can change this setting at any time by changing the static <see cref="DOTween.logBehaviour"/> property.
        /// <para>Default: ErrorsOnly</para></param>
        public static IDOTweenInit Init(bool? recycleAllByDefault = null, bool? useSafeMode = null, LogBehaviour? logBehaviour = null)
        {
            if (initialized) return instance;
            if (!Application.isPlaying || isQuitting) return null;

            DOTweenSettings settings = Resources.Load(DOTweenSettings.AssetName) as DOTweenSettings;
            return Init(settings, recycleAllByDefault, useSafeMode, logBehaviour);
        }
        // Auto-init
        static void AutoInit()
        {
            DOTweenSettings settings = Resources.Load(DOTweenSettings.AssetName) as DOTweenSettings;
            Init(settings, null, null, null);
        }
        // Full init
        static IDOTweenInit Init(DOTweenSettings settings, bool? recycleAllByDefault, bool? useSafeMode, LogBehaviour? logBehaviour)
        {
            initialized = true;
            // Options
            if (recycleAllByDefault != null) DOTween.defaultRecyclable = (bool)recycleAllByDefault;
            if (useSafeMode != null) DOTween.useSafeMode = (bool)useSafeMode;
            if (logBehaviour != null) DOTween.logBehaviour = (LogBehaviour)logBehaviour;
            // Gameobject - also assign instance
            DOTweenComponent.Create();
            // Assign settings
            if (settings != null) {
                if (useSafeMode == null) DOTween.useSafeMode = settings.useSafeMode;
                if (logBehaviour == null) DOTween.logBehaviour = settings.logBehaviour;
                if (recycleAllByDefault == null) DOTween.defaultRecyclable = settings.defaultRecyclable;
                DOTween.timeScale = settings.timeScale;
                DOTween.useSmoothDeltaTime = settings.useSmoothDeltaTime;
                DOTween.maxSmoothUnscaledTime = settings.maxSmoothUnscaledTime;
                DOTween.rewindCallbackMode = settings.rewindCallbackMode;
                DOTween.defaultRecyclable = recycleAllByDefault == null ? settings.defaultRecyclable : (bool)recycleAllByDefault;
                DOTween.showUnityEditorReport = settings.showUnityEditorReport;
                DOTween.drawGizmos = settings.drawGizmos;
                DOTween.defaultAutoPlay = settings.defaultAutoPlay;
                DOTween.defaultUpdateType = settings.defaultUpdateType;
                DOTween.defaultTimeScaleIndependent = settings.defaultTimeScaleIndependent;
                DOTween.defaultEaseType = settings.defaultEaseType;
                DOTween.defaultEaseOvershootOrAmplitude = settings.defaultEaseOvershootOrAmplitude;
                DOTween.defaultEasePeriod = settings.defaultEasePeriod;
                DOTween.defaultAutoKill = settings.defaultAutoKill;
                DOTween.defaultLoopType = settings.defaultLoopType;
            }
            // Log
            if (Debugger.logPriority >= 2) Debugger.Log("DOTween initialization (useSafeMode: " + DOTween.useSafeMode + ", recycling: " + (DOTween.defaultRecyclable ? "ON" : "OFF") + ", logBehaviour: " + DOTween.logBehaviour + ")");

            return instance;
        }

        /// <summary>
        /// Directly sets the current max capacity of Tweeners and Sequences
        /// (meaning how many Tweeners and Sequences can be running at the same time),
        /// so that DOTween doesn't need to automatically increase them in case the max is reached
        /// (which might lead to hiccups when that happens).
        /// Sequences capacity must be less or equal to Tweeners capacity
        /// (if you pass a low Tweener capacity it will be automatically increased to match the Sequence's).
        /// Beware: use this method only when there are no tweens running.
        /// </summary>
        /// <param name="tweenersCapacity">Max Tweeners capacity.
        /// Default: 200</param>
        /// <param name="sequencesCapacity">Max Sequences capacity.
        /// Default: 50</param>
        public static void SetTweensCapacity(int tweenersCapacity, int sequencesCapacity)
        {
            TweenManager.SetCapacities(tweenersCapacity, sequencesCapacity);
        }

        /// <summary>
        /// Kills all tweens, clears all cached tween pools and plugins and resets the max Tweeners/Sequences capacities to the default values.
        /// </summary>
        /// <param name="destroy">If TRUE also destroys DOTween's gameObject and resets its initializiation, default settings and everything else
        /// (so that next time you use it it will need to be re-initialized)</param>
        public static void Clear(bool destroy = false)
        {
            TweenManager.PurgeAll();
            PluginsManager.PurgeAll();
            if (!destroy) return;

            initialized = false;
            useSafeMode = false;
            showUnityEditorReport = false;
            drawGizmos = true;
            timeScale = 1;
            useSmoothDeltaTime = false;
            logBehaviour = LogBehaviour.ErrorsOnly;
            defaultEaseType = Ease.OutQuad;
            defaultEaseOvershootOrAmplitude = 1.70158f;
            defaultEasePeriod = 0;
            defaultUpdateType = UpdateType.Normal;
            defaultTimeScaleIndependent = false;
            defaultAutoPlay = AutoPlay.All;
            defaultLoopType = LoopType.Restart;
            defaultAutoKill = true;
            defaultRecyclable = false;
            maxActiveTweenersReached = maxActiveSequencesReached = 0;

            DOTweenComponent.DestroyInstance();
        }

        /// <summary>
        /// Clears all cached tween pools.
        /// </summary>
        public static void ClearCachedTweens()
        {
            TweenManager.PurgePools();
        }

        /// <summary>
        /// Checks all active tweens to find and remove eventually invalid ones (usually because their targets became NULL)
        /// and returns the total number of invalid tweens found and removed.
        /// IMPORTANT: this will cause an error on UWP platform, so don't use it there 
        /// BEWARE: this is a slightly expensive operation so use it with care
        /// </summary>
        public static int Validate()
        {
            return TweenManager.Validate();
        }

        /// <summary>
        /// Updates all tweens that are set to <see cref="UpdateType.Manual"/>.
        /// </summary>
        /// <param name="deltaTime">Manual deltaTime</param>
        /// <param name="unscaledDeltaTime">Unscaled delta time (used with tweens set as timeScaleIndependent)</param>
        public static void ManualUpdate(float deltaTime, float unscaledDeltaTime)
        {
            InitCheck();
            instance.ManualUpdate(deltaTime, unscaledDeltaTime);
        }

        #endregion

        // ===================================================================================
        // PUBLIC TWEEN CREATION METHODS -----------------------------------------------------

        // Sadly can't make generic versions of default tweens with additional options
        // where the TO method doesn't contain the options param, otherwise the correct Option type won't be inferred.
        // So: overloads. Sigh.
        // Also, Unity has a bug which doesn't allow method overloading with its own implicitly casteable types (like Vector4 and Color)
        // and additional parameters, so in those cases I have to create overloads instead than using optionals. ARARGH!

        #region Tween TO

        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<float, float, FloatOptions> To(DOGetter<float> getter, DOSetter<float> setter, float endValue, float duration)
        { return ApplyTo<float, float, FloatOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<double, double, NoOptions> To(DOGetter<double> getter, DOSetter<double> setter, double endValue, float duration)
        { return ApplyTo<double, double, NoOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static Tweener To(DOGetter<int> getter, DOSetter<int> setter, int endValue,float duration)
        { return ApplyTo<int, int, NoOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static Tweener To(DOGetter<uint> getter, DOSetter<uint> setter, uint endValue, float duration)
        { return ApplyTo<uint, uint, UintOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static Tweener To(DOGetter<long> getter, DOSetter<long> setter, long endValue, float duration)
        { return ApplyTo<long, long, NoOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static Tweener To(DOGetter<ulong> getter, DOSetter<ulong> setter, ulong endValue, float duration)
        { return ApplyTo<ulong, ulong, NoOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<string, string, StringOptions> To(DOGetter<string> getter, DOSetter<string> setter, string endValue, float duration)
        { return ApplyTo<string, string, StringOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<DOVector2, DOVector2, VectorOptions> To(DOGetter<DOVector2> getter, DOSetter<DOVector2> setter, Vector2 endValue, float duration)
        { return ApplyTo<DOVector2, DOVector2, VectorOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<DOVector3, DOVector3, VectorOptions> To(DOGetter<DOVector3> getter, DOSetter<DOVector3> setter, Vector3 endValue, float duration)
        { return ApplyTo<DOVector3, DOVector3, VectorOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<DOVector4, DOVector4, VectorOptions> To(DOGetter<DOVector4> getter, DOSetter<DOVector4> setter, Vector4 endValue, float duration)
        { return ApplyTo<DOVector4, DOVector4, VectorOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<DOQuaternion, DOVector3, QuaternionOptions> To(DOGetter<DOQuaternion> getter, DOSetter<DOQuaternion> setter, Vector3 endValue, float duration)
        { return ApplyTo<DOQuaternion, DOVector3, QuaternionOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<DOColor, DOColor, ColorOptions> To(DOGetter<DOColor> getter, DOSetter<DOColor> setter, Color endValue, float duration)
        { return ApplyTo<DOColor, DOColor, ColorOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<Rect, Rect, RectOptions> To(DOGetter<Rect> getter, DOSetter<Rect> setter, Rect endValue, float duration)
        { return ApplyTo<Rect, Rect, RectOptions>(getter, setter, endValue, duration); }
        /// <summary>Tweens a property or field to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static Tweener To(DOGetter<RectOffset> getter, DOSetter<RectOffset> setter, RectOffset endValue, float duration)
        { return ApplyTo<RectOffset, RectOffset, NoOptions>(getter, setter, endValue, duration); }

        /// <summary>Tweens a property or field to the given value using a custom plugin</summary>
        /// <param name="plugin">The plugin to use. Each custom plugin implements a static <code>Get()</code> method
        /// you'll need to call to assign the correct plugin in the correct way, like this:
        /// <para><code>CustomPlugin.Get()</code></para></param>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static TweenerCore<T1, T2, TPlugOptions> To<T1, T2, TPlugOptions>(
            ABSTweenPlugin<T1, T2, TPlugOptions> plugin, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration
        )
            where TPlugOptions : struct, IPlugOptions
        { return ApplyTo(getter, setter, endValue, duration, plugin); }

        /// <summary>Tweens only one axis of a Vector3 to the given value using default plugins.</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        /// <param name="axisConstraint">The axis to tween</param>
        public static TweenerCore<DOVector3, DOVector3, VectorOptions> ToAxis(DOGetter<DOVector3> getter, DOSetter<DOVector3> setter, float endValue, float duration, AxisConstraint axisConstraint = AxisConstraint.X)
        {
            TweenerCore<DOVector3, DOVector3, VectorOptions> t = ApplyTo<DOVector3, DOVector3, VectorOptions>(getter, setter, new Vector3(endValue, endValue, endValue), duration);
            t.plugOptions.axisConstraint = axisConstraint;
            return t;
        }
        /// <summary>Tweens only the alpha of a Color to the given value using default plugins</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValue">The end value to reach</param><param name="duration">The tween's duration</param>
        public static Tweener ToAlpha(DOGetter<DOColor> getter, DOSetter<DOColor> setter, float endValue, float duration)
        { return ApplyTo<DOColor, DOColor, ColorOptions>(getter, setter, new Color(0, 0, 0, endValue), duration).SetOptions(true); }

        #endregion

        #region Special TOs (No FROMs)

        /// <summary>Tweens a virtual property from the given start to the given end value 
        /// and implements a setter that allows to use that value with an external method or a lambda
        /// <para>Example:</para>
        /// <code>To(MyMethod, 0, 12, 0.5f);</code>
        /// <para>Where MyMethod is a function that accepts a float parameter (which will be the result of the virtual tween)</para></summary>
        /// <param name="setter">The action to perform with the tweened value</param>
        /// <param name="startValue">The value to start from</param>
        /// <param name="endValue">The end value to reach</param>
        /// <param name="duration">The duration of the virtual tween
        /// </param>
        public static Tweener To(DOSetter<float> setter, float startValue, float endValue, float duration)
        {
            float v = startValue;
            return To(() => v, x => { v = x; setter(v); }, endValue, duration)
                .NoFrom();
        }

        /// <summary>Punches a Vector3 towards the given direction and then back to the starting one
        /// as if it was connected to the starting position via an elastic.
        /// <para>This tween type generates some GC allocations at startup</para></summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="direction">The direction and strength of the punch</param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="vibrato">Indicates how much will the punch vibrate</param>
        /// <param name="elasticity">Represents how much (0 to 1) the vector will go beyond the starting position when bouncing backwards.
        /// 1 creates a full oscillation between the direction and the opposite decaying direction,
        /// while 0 oscillates only between the starting position and the decaying direction</param>
        public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Punch(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3 direction, float duration, int vibrato = 10, float elasticity = 1)
        {
            if (elasticity > 1) elasticity = 1;
            else if (elasticity < 0) elasticity = 0;
            float strength = direction.magnitude;
            int totIterations = (int)(vibrato * duration);
            if (totIterations < 2) totIterations = 2;
            float decayXTween = strength / totIterations;
            // Calculate and store the duration of each tween
            float[] tDurations = new float[totIterations];
            float sum = 0;
            for (int i = 0; i < totIterations; ++i) {
                float iterationPerc = (i + 1) / (float)totIterations;
                float tDuration = duration * iterationPerc;
                sum += tDuration;
                tDurations[i] = tDuration;
            }
            float tDurationMultiplier = duration / sum; // Multiplier that allows the sum of tDurations to equal the set duration
            for (int i = 0; i < totIterations; ++i) tDurations[i] = tDurations[i] * tDurationMultiplier;
            // Create the tween
            Vector3[] tos = new Vector3[totIterations];
            for (int i = 0; i < totIterations; ++i) {
                if (i < totIterations - 1) {
                    if (i == 0) tos[i] = direction;
                    else if (i % 2 != 0) tos[i] = -Vector3.ClampMagnitude(direction, strength * elasticity);
                    else tos[i] = Vector3.ClampMagnitude(direction, strength);
                    strength -= decayXTween;
                } else tos[i] = Vector3.zero;
            }
            return ToArray(getter, setter, tos, tDurations)
                .NoFrom()
                .SetSpecialStartupMode(SpecialStartupMode.SetPunch);
        }

        /// <summary>Shakes a Vector3 with the given values.</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="strength">The shake strength</param>
        /// <param name="vibrato">Indicates how much will the shake vibrate</param>
        /// <param name="randomness">Indicates how much the shake will be random (0 to 180 - values higher than 90 kind of suck, so beware). 
        /// Setting it to 0 will shake along a single direction and behave like a random punch.</param>
        /// <param name="ignoreZAxis">If TRUE only shakes on the X Y axis (looks better with things like cameras).</param>
        /// <param name="fadeOut">If TRUE the shake will automatically fadeOut smoothly within the tween's duration, otherwise it will not</param>
        public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Shake(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float duration,
            float strength = 3, int vibrato = 10, float randomness = 90, bool ignoreZAxis = true, bool fadeOut = true
        )
        {
            return Shake(getter, setter, duration, new Vector3(strength, strength, strength), vibrato, randomness, ignoreZAxis, false, fadeOut);
        }
        /// <summary>Shakes a Vector3 with the given values.</summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="strength">The shake strength on each axis</param>
        /// <param name="vibrato">Indicates how much will the shake vibrate</param>
        /// <param name="randomness">Indicates how much the shake will be random (0 to 180 - values higher than 90 kind of suck, so beware). 
        /// Setting it to 0 will shake along a single direction and behave like a random punch.</param>
        /// <param name="fadeOut">If TRUE the shake will automatically fadeOut smoothly within the tween's duration, otherwise it will not</param>
        public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Shake(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float duration,
            Vector3 strength, int vibrato = 10, float randomness = 90, bool fadeOut = true
        )
        {
            return Shake(getter, setter, duration, strength, vibrato, randomness, false, true, fadeOut);
        }
        static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Shake(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float duration,
            Vector3 strength, int vibrato, float randomness, bool ignoreZAxis, bool vectorBased, bool fadeOut
        )
        {
            float shakeMagnitude = vectorBased ? strength.magnitude : strength.x;
            int totIterations = (int)(vibrato * duration);
            if (totIterations < 2) totIterations = 2;
            float decayXTween = shakeMagnitude / totIterations;
            // Calculate and store the duration of each tween
            float[] tDurations = new float[totIterations];
            float sum = 0;
            for (int i = 0; i < totIterations; ++i) {
                float iterationPerc = (i + 1) / (float)totIterations;
                float tDuration = fadeOut ? duration * iterationPerc : duration / totIterations;
                sum += tDuration;
                tDurations[i] = tDuration;
            }
            float tDurationMultiplier = duration / sum; // Multiplier that allows the sum of tDurations to equal the set duration
            for (int i = 0; i < totIterations; ++i) tDurations[i] = tDurations[i] * tDurationMultiplier;
            // Create the tween
            float ang = UnityEngine.Random.Range(0f, 360f);
            Vector3[] tos = new Vector3[totIterations];
            for (int i = 0; i < totIterations; ++i) {
                if (i < totIterations - 1) {
                    if (i > 0) ang = ang - 180 + UnityEngine.Random.Range(-randomness, randomness);
                    if (vectorBased) {
                        Quaternion rndQuaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(-randomness, randomness), Vector3.up);
                        Vector3 to = rndQuaternion * Utils.Vector3FromAngle(ang, shakeMagnitude);
                        to.x = Vector3.ClampMagnitude(to, strength.x).x;
                        to.y = Vector3.ClampMagnitude(to, strength.y).y;
                        to.z = Vector3.ClampMagnitude(to, strength.z).z;
                        tos[i] = to;
                        if (fadeOut) shakeMagnitude -= decayXTween;
                        strength = Vector3.ClampMagnitude(strength, shakeMagnitude);
                    } else {
                        if (ignoreZAxis) {
                            tos[i] = Utils.Vector3FromAngle(ang, shakeMagnitude);
                        } else {
                            Quaternion rndQuaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(-randomness, randomness), Vector3.up);
                            tos[i] = rndQuaternion * Utils.Vector3FromAngle(ang, shakeMagnitude);
                        }
                        if (fadeOut) shakeMagnitude -= decayXTween;
                    }
                } else tos[i] = Vector3.zero;
            }
            return ToArray(getter, setter, tos, tDurations)
                .NoFrom().SetSpecialStartupMode(SpecialStartupMode.SetShake);
        }

        /// <summary>Tweens a property or field to the given values using default plugins.
        /// Ease is applied between each segment and not as a whole.
        /// <para>This tween type generates some GC allocations at startup</para></summary>
        /// <param name="getter">A getter for the field or property to tween.
        /// <para>Example usage with lambda:</para><code>()=> myProperty</code></param>
        /// <param name="setter">A setter for the field or property to tween
        /// <para>Example usage with lambda:</para><code>x=> myProperty = x</code></param>
        /// <param name="endValues">The end values to reach for each segment. This array must have the same length as <code>durations</code></param>
        /// <param name="durations">The duration of each segment. This array must have the same length as <code>endValues</code></param>
        public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> ToArray(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3[] endValues, float[] durations)
        {
            int len = durations.Length;
            if (len != endValues.Length) {
                Debugger.LogError("To Vector3 array tween: endValues and durations arrays must have the same length");
                return null;
            }

            // Clone the arrays
            Vector3[] endValuesClone = new Vector3[len];
            float[] durationsClone = new float[len];
            for (int i = 0; i < len; i++) {
                endValuesClone[i] = endValues[i];
                durationsClone[i] = durations[i];
            }

            float totDuration = 0;
            for (int i = 0; i < len; ++i) totDuration += durationsClone[i];
            TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t =
                ApplyTo<Vector3, Vector3[], Vector3ArrayOptions>(getter, setter, endValuesClone, totDuration)
                    .NoFrom();
            t.plugOptions.durations = durationsClone;
            return t;
        }

        #endregion

        #region Special TOs (INTERNAL)

        internal static TweenerCore<Color2, Color2, ColorOptions> To(DOGetter<Color2> getter, DOSetter<Color2> setter, Color2 endValue, float duration)
        { return ApplyTo<Color2, Color2, ColorOptions>(getter, setter, endValue, duration); }

        #endregion

        #region Tween SEQUENCE

        /// <summary>
        /// Returns a new <see cref="Sequence"/> to be used for tween groups
        /// </summary>
        public static Sequence Sequence()
        {
            InitCheck();
            Sequence sequence = TweenManager.GetSequence();
            Tweening.Sequence.Setup(sequence);
            return sequence;
        }
        #endregion

        /////////////////////////////////////////////////////////////////////
        // OTHER STUFF //////////////////////////////////////////////////////

        #region Play Operations

        /// <summary>Completes all tweens and returns the number of actual tweens completed
        /// (meaning tweens that don't have infinite loops and were not already complete)</summary>
        /// <param name="withCallbacks">For Sequences only: if TRUE also internal Sequence callbacks will be fired,
        /// otherwise they will be ignored</param>
        public static int CompleteAll(bool withCallbacks = false)
        {
            return TweenManager.FilteredOperation(OperationType.Complete, FilterType.All, null, false, withCallbacks ? 1 : 0);
        }
        /// <summary>Completes all tweens with the given ID or target and returns the number of actual tweens completed
        /// (meaning the tweens that don't have infinite loops and were not already complete)</summary>
        /// <param name="withCallbacks">For Sequences only: if TRUE internal Sequence callbacks will be fired,
        /// otherwise they will be ignored</param>
        public static int Complete(object targetOrId, bool withCallbacks = false)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Complete, FilterType.TargetOrId, targetOrId, false, withCallbacks ? 1 : 0);
        }
        // Used internally to complete a tween and return only the number of killed tweens instead than just the completed ones
        // (necessary for Kill(complete) operation. Sets optionalBool to TRUE)
        internal static int CompleteAndReturnKilledTot()
        {
            return TweenManager.FilteredOperation(OperationType.Complete, FilterType.All, null, true, 0);
        }
        internal static int CompleteAndReturnKilledTot(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Complete, FilterType.TargetOrId, targetOrId, true, 0);
        }
        internal static int CompleteAndReturnKilledTotExceptFor(params object[] excludeTargetsOrIds)
        {
            // excludeTargetsOrIds is never NULL (checked by DOTween.KillAll)
            return TweenManager.FilteredOperation(OperationType.Complete, FilterType.AllExceptTargetsOrIds, null, true, 0, null, excludeTargetsOrIds);
        }

        /// <summary>Flips all tweens (changing their direction to forward if it was backwards and viceversa),
        /// then returns the number of actual tweens flipped</summary>
        public static int FlipAll()
        {
            return TweenManager.FilteredOperation(OperationType.Flip, FilterType.All, null, false, 0);
        }
        /// <summary>Flips the tweens with the given ID or target (changing their direction to forward if it was backwards and viceversa),
        /// then returns the number of actual tweens flipped</summary>
        public static int Flip(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Flip, FilterType.TargetOrId, targetOrId, false, 0);
        }

        /// <summary>Sends all tweens to the given position (calculating also eventual loop cycles) and returns the actual tweens involved</summary>
        public static int GotoAll(float to, bool andPlay = false)
        {
            return TweenManager.FilteredOperation(OperationType.Goto, FilterType.All, null, andPlay, to);
        }
        /// <summary>Sends all tweens with the given ID or target to the given position (calculating also eventual loop cycles)
        /// and returns the actual tweens involved</summary>
        public static int Goto(object targetOrId, float to, bool andPlay = false)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Goto, FilterType.TargetOrId, targetOrId, andPlay, to);
        }

        /// <summary>Kills all tweens and returns the number of actual tweens killed</summary>
        /// <param name="complete">If TRUE completes the tweens before killing them</param>
        public static int KillAll(bool complete = false)
        {
            int tot = complete ? CompleteAndReturnKilledTot() : 0;
            return tot + TweenManager.DespawnAll();
        }
        /// <summary>Kills all tweens and returns the number of actual tweens killed</summary>
        /// <param name="complete">If TRUE completes the tweens before killing them</param>
        /// <param name="idsOrTargetsToExclude">Eventual IDs or targets to exclude from the killing</param>
        public static int KillAll(bool complete, params object[] idsOrTargetsToExclude)
        {
            int tot;
            if (idsOrTargetsToExclude == null) {
                tot = complete ? CompleteAndReturnKilledTot() : 0;
                return tot + TweenManager.DespawnAll();
            }
            tot = complete ? CompleteAndReturnKilledTotExceptFor(idsOrTargetsToExclude) : 0;
            return tot + TweenManager.FilteredOperation(OperationType.Despawn, FilterType.AllExceptTargetsOrIds, null, false, 0, null, idsOrTargetsToExclude);
        }
        /// <summary>Kills all tweens with the given ID or target and returns the number of actual tweens killed</summary>
        /// <param name="complete">If TRUE completes the tweens before killing them</param>
        public static int Kill(object targetOrId, bool complete = false)
        {
            if (targetOrId == null) return 0;
            int tot = complete ? CompleteAndReturnKilledTot(targetOrId) : 0;
            return tot + TweenManager.FilteredOperation(OperationType.Despawn, FilterType.TargetOrId, targetOrId, false, 0);
        }

        /// <summary>Pauses all tweens and returns the number of actual tweens paused</summary>
        public static int PauseAll()
        {
            return TweenManager.FilteredOperation(OperationType.Pause, FilterType.All, null, false, 0);
        }
        /// <summary>Pauses all tweens with the given ID or target and returns the number of actual tweens paused
        /// (meaning the tweens that were actually playing and have been paused)</summary>
        public static int Pause(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Pause, FilterType.TargetOrId, targetOrId, false, 0);
        }

        /// <summary>Plays all tweens and returns the number of actual tweens played
        /// (meaning tweens that were not already playing or complete)</summary>
        public static int PlayAll()
        {
            return TweenManager.FilteredOperation(OperationType.Play, FilterType.All, null, false, 0);
        }
        /// <summary>Plays all tweens with the given ID or target and returns the number of actual tweens played
        /// (meaning the tweens that were not already playing or complete)</summary>
        public static int Play(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Play, FilterType.TargetOrId, targetOrId, false, 0);
        }
        /// <summary>Plays all tweens with the given target and the given ID, and returns the number of actual tweens played
        /// (meaning the tweens that were not already playing or complete)</summary>
        public static int Play(object target, object id)
        {
            if (target == null || id == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Play, FilterType.TargetAndId, id, false, 0, target);
        }

        /// <summary>Plays backwards all tweens and returns the number of actual tweens played
        /// (meaning tweens that were not already started, playing backwards or rewinded)</summary>
        public static int PlayBackwardsAll()
        {
            return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.All, null, false, 0);
        }
        /// <summary>Plays backwards all tweens with the given ID or target and returns the number of actual tweens played
        /// (meaning the tweens that were not already started, playing backwards or rewinded)</summary>
        public static int PlayBackwards(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.TargetOrId, targetOrId, false, 0);
        }
        /// <summary>Plays backwards all tweens with the given target and ID and returns the number of actual tweens played
        /// (meaning the tweens that were not already started, playing backwards or rewinded)</summary>
        public static int PlayBackwards(object target, object id)
        {
            if (target == null || id == null) return 0;
            return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.TargetAndId, id, false, 0, target);
        }

        /// <summary>Plays forward all tweens and returns the number of actual tweens played
        /// (meaning tweens that were not already playing forward or complete)</summary>
        public static int PlayForwardAll()
        {
            return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.All, null, false, 0);
        }
        /// <summary>Plays forward all tweens with the given ID or target and returns the number of actual tweens played
        /// (meaning the tweens that were not already playing forward or complete)</summary>
        public static int PlayForward(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.TargetOrId, targetOrId, false, 0);
        }
        /// <summary>Plays forward all tweens with the given target and ID and returns the number of actual tweens played
        /// (meaning the tweens that were not already started, playing backwards or rewinded)</summary>
        public static int PlayForward(object target, object id)
        {
            if (target == null || id == null) return 0;
            return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.TargetAndId, id, false, 0, target);
        }

        /// <summary>Restarts all tweens, then returns the number of actual tweens restarted</summary>
        public static int RestartAll(bool includeDelay = true)
        {
            return TweenManager.FilteredOperation(OperationType.Restart, FilterType.All, null, includeDelay, 0);
        }
        /// <summary>Restarts all tweens with the given ID or target, then returns the number of actual tweens restarted</summary>
        /// <param name="includeDelay">If TRUE includes the eventual tweens delays, otherwise skips them</param>
        /// <param name="changeDelayTo">If >= 0 changes the startup delay of all involved tweens to this value, otherwise doesn't touch it</param>
        public static int Restart(object targetOrId, bool includeDelay = true, float changeDelayTo = -1)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Restart, FilterType.TargetOrId, targetOrId, includeDelay, changeDelayTo);
        }
        /// <summary>Restarts all tweens with the given target and the given ID, and returns the number of actual tweens played
        /// (meaning the tweens that were not already playing or complete)</summary>
        /// <param name="includeDelay">If TRUE includes the eventual tweens delays, otherwise skips them</param>
        /// <param name="changeDelayTo">If >= 0 changes the startup delay of all involved tweens to this value, otherwise doesn't touch it</param>
        public static int Restart(object target, object id, bool includeDelay = true, float changeDelayTo = -1)
        {
            if (target == null || id == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Restart, FilterType.TargetAndId, id, includeDelay, changeDelayTo, target);
        }

        /// <summary>Rewinds and pauses all tweens, then returns the number of actual tweens rewinded
        /// (meaning tweens that were not already rewinded)</summary>
        public static int RewindAll(bool includeDelay = true)
        {
            return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.All, null, includeDelay, 0);
        }
        /// <summary>Rewinds and pauses all tweens with the given ID or target, then returns the number of actual tweens rewinded
        /// (meaning the tweens that were not already rewinded)</summary>
        public static int Rewind(object targetOrId, bool includeDelay = true)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.TargetOrId, targetOrId, includeDelay, 0);
        }

        /// <summary>Smoothly rewinds all tweens (delays excluded), then returns the number of actual tweens rewinding/rewinded
        /// (meaning tweens that were not already rewinded).
        /// A "smooth rewind" animates the tween to its start position,
        /// skipping all elapsed loops (except in case of LoopType.Incremental) while keeping the animation fluent.
        /// <para>Note that a tween that was smoothly rewinded will have its play direction flipped</para></summary>
        public static int SmoothRewindAll()
        {
            return TweenManager.FilteredOperation(OperationType.SmoothRewind, FilterType.All, null, false, 0);
        }
        /// <summary>Smoothly rewinds all tweens (delays excluded) with the given ID or target, then returns the number of actual tweens rewinding/rewinded
        /// (meaning the tweens that were not already rewinded).
        /// A "smooth rewind" animates the tween to its start position,
        /// skipping all elapsed loops (except in case of LoopType.Incremental) while keeping the animation fluent.
        /// <para>Note that a tween that was smoothly rewinded will have its play direction flipped</para></summary>
        public static int SmoothRewind(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.SmoothRewind, FilterType.TargetOrId, targetOrId, false, 0);
        }

        /// <summary>Toggles the play state of all tweens and returns the number of actual tweens toggled
        /// (meaning tweens that could be played or paused, depending on the toggle state)</summary>
        public static int TogglePauseAll()
        {
            return TweenManager.FilteredOperation(OperationType.TogglePause, FilterType.All, null, false, 0);
        }
        /// <summary>Toggles the play state of all tweens with the given ID or target and returns the number of actual tweens toggled
        /// (meaning the tweens that could be played or paused, depending on the toggle state)</summary>
        public static int TogglePause(object targetOrId)
        {
            if (targetOrId == null) return 0;
            return TweenManager.FilteredOperation(OperationType.TogglePause, FilterType.TargetOrId, targetOrId, false, 0);
        }
        #endregion

        #region Global Info Getters

        /// <summary>
        /// Returns TRUE if a tween with the given ID or target is active.
        /// <para>You can also use this to know if a shortcut tween is active for a given target.</para>
        /// <para>Example:</para>
        /// <para><code>transform.DOMoveX(45, 1); // transform is automatically added as the tween target</code></para>
        /// <para><code>DOTween.IsTweening(transform); // Returns true</code></para>
        /// </summary>
        /// <param name="targetOrId">The target or ID to look for</param>
        /// <param name="alsoCheckIfIsPlaying">If FALSE (default) returns TRUE as long as a tween for the given target/ID is active,
        /// otherwise also requires it to be playing</param>
        public static bool IsTweening(object targetOrId, bool alsoCheckIfIsPlaying = false)
        {
            return TweenManager.FilteredOperation(OperationType.IsTweening, FilterType.TargetOrId, targetOrId, alsoCheckIfIsPlaying, 0) > 0;
        }

        /// <summary>
        /// Returns the total number of active and playing tweens.
        /// A tween is considered as playing even if its delay is actually playing
        /// </summary>
        public static int TotalPlayingTweens()
        {
            return TweenManager.TotalPlayingTweens();
        }

        /// <summary>
        /// Returns a list of all active tweens in a playing state.
        /// Returns NULL if there are no active playing tweens.
        /// <para>Beware: each time you call this method a new list is generated, so use it for debug only</para>
        /// </summary>
        /// <param name="fillableList">If NULL creates a new list, otherwise clears and fills this one (and thus saves allocations)</param>
        public static List<Tween> PlayingTweens(List<Tween> fillableList = null)
        {
            if (fillableList != null) fillableList.Clear();
            return TweenManager.GetActiveTweens(true, fillableList);
        }

        /// <summary>
        /// Returns a list of all active tweens in a paused state.
        /// Returns NULL if there are no active paused tweens.
        /// <para>Beware: each time you call this method a new list is generated, so use it for debug only</para>
        /// </summary>
        /// <param name="fillableList">If NULL creates a new list, otherwise clears and fills this one (and thus saves allocations)</param>
        public static List<Tween> PausedTweens(List<Tween> fillableList = null)
        {
            if (fillableList != null) fillableList.Clear();
            return TweenManager.GetActiveTweens(false, fillableList);
        }

        /// <summary>
        /// Returns a list of all active tweens with the given id.
        /// Returns NULL if there are no active tweens with the given id.
        /// <para>Beware: each time you call this method a new list is generated</para>
        /// </summary>
        /// <param name="playingOnly">If TRUE returns only the tweens with the given ID that are currently playing</param>
        /// <param name="fillableList">If NULL creates a new list, otherwise clears and fills this one (and thus saves allocations)</param>
        public static List<Tween> TweensById(object id, bool playingOnly = false, List<Tween> fillableList = null)
        {
            if (id == null) return null;

            if (fillableList != null) fillableList.Clear();
            return TweenManager.GetTweensById(id, playingOnly, fillableList);
        }

        /// <summary>
        /// Returns a list of all active tweens with the given target.
        /// Returns NULL if there are no active tweens with the given target.
        /// <para>Beware: each time you call this method a new list is generated</para>
        /// <param name="playingOnly">If TRUE returns only the tweens with the given target that are currently playing</param>
        /// <param name="fillableList">If NULL creates a new list, otherwise clears and fills this one (and thus saves allocations)</param>
        /// </summary>
        public static List<Tween> TweensByTarget(object target, bool playingOnly = false, List<Tween> fillableList = null)
        {
            if (fillableList != null) fillableList.Clear();
            return TweenManager.GetTweensByTarget(target, playingOnly, fillableList);
        }

        #endregion

        // ===================================================================================
        // METHODS ---------------------------------------------------------------------------

        static void InitCheck()
        {
            if (initialized || !Application.isPlaying || isQuitting) return;

            AutoInit();
        }

        static TweenerCore<T1, T2, TPlugOptions> ApplyTo<T1, T2, TPlugOptions>(
            DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration, ABSTweenPlugin<T1, T2, TPlugOptions> plugin = null
        )
            where TPlugOptions : struct, IPlugOptions
        {
            InitCheck();
            TweenerCore<T1, T2, TPlugOptions> tweener = TweenManager.GetTweener<T1, T2, TPlugOptions>();
            if (!Tweener.Setup(tweener, getter, setter, endValue, duration, plugin)) {
                TweenManager.Despawn(tweener);
                return null;
            }
            return tweener;
        }
    }
}