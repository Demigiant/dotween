// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/06 18:11
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

#if COMPATIBLE
using DOVector2 = DG.Tweening.Core.Surrogates.Vector2Wrapper;
using DOVector3 = DG.Tweening.Core.Surrogates.Vector3Wrapper;
using DOVector4 = DG.Tweening.Core.Surrogates.Vector4Wrapper;
using DOQuaternion = DG.Tweening.Core.Surrogates.QuaternionWrapper;
using DOColor = DG.Tweening.Core.Surrogates.ColorWrapper;
using DOVector2Plugin = DG.Tweening.Plugins.Vector2WrapperPlugin;
using DOVector3Plugin = DG.Tweening.Plugins.Vector3WrapperPlugin;
using DOVector4Plugin = DG.Tweening.Plugins.Vector4WrapperPlugin;
using DOQuaternionPlugin = DG.Tweening.Plugins.QuaternionWrapperPlugin;
using DOColorPlugin = DG.Tweening.Plugins.ColorWrapperPlugin;
#else
using DOVector2 = UnityEngine.Vector2;
using DOVector3 = UnityEngine.Vector3;
using DOVector4 = UnityEngine.Vector4;
using DOQuaternion = UnityEngine.Quaternion;
using DOColor = UnityEngine.Color;
using DOVector2Plugin = DG.Tweening.Plugins.Vector2Plugin;
using DOVector3Plugin = DG.Tweening.Plugins.Vector3Plugin;
using DOVector4Plugin = DG.Tweening.Plugins.Vector4Plugin;
using DOQuaternionPlugin = DG.Tweening.Plugins.QuaternionPlugin;
using DOColorPlugin = DG.Tweening.Plugins.ColorPlugin;
#endif
using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins.Core
{
    internal static class PluginsManager
    {
        // Default plugins
        static ITweenPlugin _floatPlugin;
        static ITweenPlugin _doublePlugin;
        static ITweenPlugin _intPlugin;
        static ITweenPlugin _uintPlugin;
        static ITweenPlugin _longPlugin;
        static ITweenPlugin _ulongPlugin;
        static ITweenPlugin _vector2Plugin;
        static ITweenPlugin _vector3Plugin;
        static ITweenPlugin _vector4Plugin;
        static ITweenPlugin _quaternionPlugin;
        static ITweenPlugin _colorPlugin;
        static ITweenPlugin _rectPlugin;
        static ITweenPlugin _rectOffsetPlugin;
        static ITweenPlugin _stringPlugin;
        static ITweenPlugin _vector3ArrayPlugin;
        static ITweenPlugin _color2Plugin;

        // Advanced and custom plugins
        const int _MaxCustomPlugins = 20;
        static Dictionary<Type, ITweenPlugin> _customPlugins;

        // ===================================================================================
        // INTERNAL METHODS ------------------------------------------------------------------

        internal static ABSTweenPlugin<T1,T2,TPlugOptions> GetDefaultPlugin<T1,T2,TPlugOptions>() where TPlugOptions : struct, IPlugOptions
        {
            Type t1 = typeof(T1);
            Type t2 = typeof(T2);
            ITweenPlugin plugin = null;
            if (t1 == typeof(DOVector3) && t1 == t2) {
                if (_vector3Plugin == null) _vector3Plugin = new DOVector3Plugin();
                plugin = _vector3Plugin;
            } else if (t1 == typeof(Vector3) && t2 == typeof(Vector3[])) {
                if (_vector3ArrayPlugin == null) _vector3ArrayPlugin = new Vector3ArrayPlugin();
                plugin = _vector3ArrayPlugin;
            } else if (t1 == typeof(DOQuaternion)) {
                if (t2 == typeof(Quaternion)) Debugger.LogError("Quaternion tweens require a Vector3 endValue");
                else {
                    if (_quaternionPlugin == null) _quaternionPlugin = new DOQuaternionPlugin();
                    plugin = _quaternionPlugin;
                }
            } else if (t1 == typeof(DOVector2)) {
                if (_vector2Plugin == null) _vector2Plugin = new DOVector2Plugin();
                plugin = _vector2Plugin;
            } else if (t1 == typeof(float)) {
                if (_floatPlugin == null) _floatPlugin = new FloatPlugin();
                plugin = _floatPlugin;
            } else if (t1 == typeof(DOColor)) {
                if (_colorPlugin == null) _colorPlugin = new DOColorPlugin();
                plugin = _colorPlugin;
            } else if (t1 == typeof(int)) {
                if (_intPlugin == null) _intPlugin = new IntPlugin();
                plugin = _intPlugin;
            } else if (t1 == typeof(DOVector4)) {
                if (_vector4Plugin == null) _vector4Plugin = new DOVector4Plugin();
                plugin = _vector4Plugin;
            } else if (t1 == typeof(Rect)) {
                if (_rectPlugin == null) _rectPlugin = new RectPlugin();
                plugin = _rectPlugin;
            } else if (t1 == typeof(RectOffset)) {
                if (_rectOffsetPlugin == null) _rectOffsetPlugin = new RectOffsetPlugin();
                plugin = _rectOffsetPlugin;
            } else if (t1 == typeof(uint)) {
                if (_uintPlugin == null) _uintPlugin = new UintPlugin();
                plugin = _uintPlugin;
            } else if (t1 == typeof(string)) {
                if (_stringPlugin == null) _stringPlugin = new StringPlugin();
                plugin = _stringPlugin;
            } else if (t1 == typeof(Color2)) {
                if (_color2Plugin == null) _color2Plugin = new Color2Plugin();
                plugin = _color2Plugin;
            } else if (t1 == typeof(long)) {
                if (_longPlugin == null) _longPlugin = new LongPlugin();
                plugin = _longPlugin;
            } else if (t1 == typeof(ulong)) {
                if (_ulongPlugin == null) _ulongPlugin = new UlongPlugin();
                plugin = _ulongPlugin;
            } else if (t1 == typeof(double)) {
                if (_doublePlugin == null) _doublePlugin = new DoublePlugin();
                plugin = _doublePlugin;
            }

            if (plugin != null) return plugin as ABSTweenPlugin<T1, T2, TPlugOptions>;

            return null;
        }

        // Public so it can be used by custom plugins Get method
        public static ABSTweenPlugin<T1, T2, TPlugOptions> GetCustomPlugin<TPlugin, T1, T2, TPlugOptions>()
            where TPlugin : ITweenPlugin, new()
            where TPlugOptions : struct, IPlugOptions
        {
            Type t = typeof(TPlugin);
            ITweenPlugin plugin;

            if (_customPlugins == null) _customPlugins = new Dictionary<Type, ITweenPlugin>(_MaxCustomPlugins);
            else if (_customPlugins.TryGetValue(t, out plugin)) return plugin as ABSTweenPlugin<T1, T2, TPlugOptions>;

            plugin = new TPlugin();
            _customPlugins.Add(t, plugin);
            return plugin as ABSTweenPlugin<T1, T2, TPlugOptions>;
        }

        // Un-caches all plugins
        internal static void PurgeAll()
        {
            _floatPlugin = null;
            _intPlugin = null;
            _uintPlugin = null;
            _longPlugin = null;
            _ulongPlugin = null;
            _vector2Plugin = null;
            _vector3Plugin = null;
            _vector4Plugin = null;
            _quaternionPlugin = null;
            _colorPlugin = null;
            _rectPlugin = null;
            _rectOffsetPlugin = null;
            _stringPlugin = null;
            _vector3ArrayPlugin = null;
            _color2Plugin = null;

            if (_customPlugins != null) _customPlugins.Clear();
        }
    }
}