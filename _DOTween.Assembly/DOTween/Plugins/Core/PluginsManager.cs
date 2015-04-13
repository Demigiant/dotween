// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/06 18:11
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

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

        internal static ABSTweenPlugin<T1,T2,TPlugOptions> GetDefaultPlugin<T1,T2,TPlugOptions>() where TPlugOptions : struct
        {
            Type t1 = typeof(T1);
            Type t2 = typeof(T2);
            ITweenPlugin plugin = null;

            if (t1 == typeof(Vector3)) {
                if (t1 == t2) {
                    if (_vector3Plugin == null) _vector3Plugin = new Vector3Plugin();
                    plugin = _vector3Plugin;
                } else if (t2 == typeof(Vector3[])) {
                    if (_vector3ArrayPlugin == null) _vector3ArrayPlugin = new Vector3ArrayPlugin();
                    plugin = _vector3ArrayPlugin;
                }
            } else if (t1 == typeof(Quaternion)) {
                if (t2 == typeof(Quaternion)) Debugger.LogError("Quaternion tweens require a Vector3 endValue");
                else {
                    if (_quaternionPlugin == null) _quaternionPlugin = new QuaternionPlugin();
                    plugin = _quaternionPlugin;
                }
            } else if (t1 == typeof(Vector2)) {
                if (_vector2Plugin == null) _vector2Plugin = new Vector2Plugin();
                plugin = _vector2Plugin;
            } else if (t1 == typeof(float)) {
                if (_floatPlugin == null) _floatPlugin = new FloatPlugin();
                plugin = _floatPlugin;
            } else if (t1 == typeof(Color)) {
                if (_colorPlugin == null) _colorPlugin = new ColorPlugin();
                plugin = _colorPlugin;
            } else if (t1 == typeof(int)) {
                if (_intPlugin == null) _intPlugin = new IntPlugin();
                plugin = _intPlugin;
            } else if (t1 == typeof(Vector4)) {
                if (_vector4Plugin == null) _vector4Plugin = new Vector4Plugin();
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
            }

#if !WP81
            if (plugin != null) return plugin as ABSTweenPlugin<T1, T2, TPlugOptions>;
#else
            // WP8.1 fix tries
            if (plugin != null) {
                Debug.Log("PLUGIN FOUND, trying to assign it correctly...");
                ABSTweenPlugin<T1, T2, TPlugOptions> p;
                ABSTweenPlugin<Vector3, Vector3, VectorOptions> pExplicit;
                // Explicit casting to Vector3Plugin
                try {
                    pExplicit = (ABSTweenPlugin<Vector3, Vector3, VectorOptions>)plugin;
                    if (pExplicit != null) Debug.Log("- EXPLICIT CAST SUCCESS X");
                    p = pExplicit as ABSTweenPlugin<T1, T2, TPlugOptions>;
                    if (p != null) {
                        Debug.Log("- PLUGIN SUCCESS X");
                        return p;
                    }
                } catch (Exception e) {
                    Debug.Log("- PLUGIN FAIL X > " + e.Message);
                }
                // More regular ways
                try {
                    p = plugin as ABSTweenPlugin<T1, T2, TPlugOptions>;
                    if (p != null) {
                        Debug.Log("- PLUGIN SUCCESS A");
                        return p;
                    }
                } catch (Exception e) {
                    Debug.Log("- PLUGIN FAIL A > " + e.Message);
                }
                try {
                    System.Object obj = (object)plugin;
                    p = obj as ABSTweenPlugin<T1, T2, TPlugOptions>;
                    if (p != null) {
                        Debug.Log("- PLUGIN SUCCESS A2");
                        return p;
                    }
                } catch (Exception e) {
                    Debug.Log("- PLUGIN FAIL A2 > " + e.Message);
                }
                try {
                    p = (ABSTweenPlugin<T1, T2, TPlugOptions>)plugin;
                    Debug.Log("- PLUGIN SUCCESS B");
                    return p;
                } catch (Exception e) {
                    Debug.Log("- PLUGIN FAIL B > " + e.Message);
                }
                try {
                    System.Object obj = (object)plugin;
                    p = (ABSTweenPlugin<T1, T2, TPlugOptions>)obj;
                    Debug.Log("- PLUGIN SUCCESS B2");
                    return p;
                } catch (Exception e) {
                    Debug.Log("- PLUGIN FAIL B2 > " + e.Message);
                }
                return null;
            }
            Debug.Log("PLUGIN NOT FOUND");
            // WP8.1 fix tries END
#endif

            return null;
        }

        // Public so it can be used by custom plugins Get method
        public static ABSTweenPlugin<T1, T2, TPlugOptions> GetCustomPlugin<TPlugin, T1, T2, TPlugOptions>()
            where TPlugin : ITweenPlugin, new()
            where TPlugOptions : struct
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