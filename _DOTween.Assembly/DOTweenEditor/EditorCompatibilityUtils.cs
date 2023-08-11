// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2023/07/21 11:37
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DG.DOTweenEditor
{
    /// <summary>
    /// Contains compatibility methods taken from DemiEditor (for when DOTween is without it)
    /// </summary>
    internal static class EditorCompatibilityUtils
    {
        static MethodInfo _miFindObjectOfTypeGeneric;
        static MethodInfo _miFindObjectOfType;
        static MethodInfo _miFindObjectsOfTypeGeneric;
        static MethodInfo _miFindObjectsOfType;
        static bool _findObjectOfType_hasIncludeInactiveParam;
        static bool _findObjectOfTypeGeneric_hasIncludeInactiveParam;
        static bool _findObjectsOfType_hasIncludeInactiveParam;
        static bool _findObjectsOfTypeGeneric_hasIncludeInactiveParam;
        static Type _findObjectsInactiveType;
        static Type _findObjectsSortModeType;

        /// <summary>
        /// Warning: some versions of this method don't have the includeInactive parameter so it won't be taken into account
        /// </summary>
        public static T FindObjectOfType<T>(bool includeInactive = false)
        {
            if (_miFindObjectOfTypeGeneric == null) {
                if (UnityEditorVersion.MajorVersion < 2023) {
                    _miFindObjectOfTypeGeneric = typeof(UnityEngine.Object).GetMethod(
                        "FindObjectOfType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                        new[] {typeof(bool)},
                        null
                    );
                    _findObjectOfTypeGeneric_hasIncludeInactiveParam = true;
                    if (_miFindObjectOfTypeGeneric == null) {
                        MethodInfo[] ms = typeof(UnityEngine.Object).GetMethods(BindingFlags.Static | BindingFlags.Public);
                        foreach (MethodInfo m in ms) {
                            if (m.Name != "FindObjectOfType" || !m.IsGenericMethod) continue;
                            _miFindObjectOfTypeGeneric = m;
                            break;
                        }
                        _findObjectOfTypeGeneric_hasIncludeInactiveParam = false;
                    }
                    _miFindObjectOfTypeGeneric = _miFindObjectOfTypeGeneric.MakeGenericMethod(typeof(T));
                } else {
                    if (_findObjectsInactiveType == null) _findObjectsInactiveType = typeof(GameObject).Assembly.GetType("UnityEngine.FindObjectsInactive");
                    _miFindObjectOfTypeGeneric = typeof(UnityEngine.Object).GetMethod(
                        "FindAnyObjectByType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                        new[] {_findObjectsInactiveType},
                        null
                    ).MakeGenericMethod(typeof(T));
                }
            }
            if (UnityEditorVersion.MajorVersion < 2023) {
                if (_findObjectOfTypeGeneric_hasIncludeInactiveParam) return (T)_miFindObjectOfTypeGeneric.Invoke(null, new object[] {includeInactive});
                else return (T)_miFindObjectOfTypeGeneric.Invoke(null, null);
            } else {
                return (T)_miFindObjectOfTypeGeneric.Invoke(null, new object[] {includeInactive ? 0 : 1});
            }
        }
        /// <summary>
        /// Warning: some versions of this method don't have the includeInactive parameter so it won't be taken into account
        /// </summary>
        public static Object FindObjectOfType(Type type, bool includeInactive = false)
        {
            if (_miFindObjectOfType == null) {
                if (UnityEditorVersion.MajorVersion < 2023) {
                    _miFindObjectOfType = typeof(UnityEngine.Object).GetMethod(
                        "FindObjectOfType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                        new[] {typeof(Type), typeof(bool)},
                        null
                    );
                    _findObjectOfType_hasIncludeInactiveParam = true;
                    if (_miFindObjectOfType == null) {
                        _miFindObjectOfType = typeof(UnityEngine.Object).GetMethod(
                            "FindObjectOfType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                            new[] {typeof(Type)},
                            null
                        );
                        _findObjectOfType_hasIncludeInactiveParam = false;
                    }
                } else {
                    if (_findObjectsInactiveType == null) _findObjectsInactiveType = typeof(GameObject).Assembly.GetType("UnityEngine.FindObjectsInactive");
                    _miFindObjectOfType = typeof(UnityEngine.Object).GetMethod(
                        "FindAnyObjectByType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                        new[] {typeof(Type), _findObjectsInactiveType},
                        null
                    );
                }
            }
            if (UnityEditorVersion.MajorVersion < 2023) {
                if (_findObjectOfType_hasIncludeInactiveParam) return (Object)_miFindObjectOfType.Invoke(null, new object[] {type, includeInactive});
                else return (Object)_miFindObjectOfType.Invoke(null, new object[] {type});
            } else {
                return (Object)_miFindObjectOfType.Invoke(null, new object[] {type, includeInactive ? 0 : 1});
            }
        }

        /// <summary>
        /// Warning: some versions of this method don't have the includeInactive parameter so it won't be taken into account
        /// </summary>
        public static T[] FindObjectsOfType<T>(bool includeInactive = false)
        {
            if (_miFindObjectsOfTypeGeneric == null) {
                if (UnityEditorVersion.MajorVersion < 2023) {
                    _miFindObjectsOfTypeGeneric = typeof(UnityEngine.Object).GetMethod(
                        "FindObjectsOfType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                        new[] {typeof(bool)},
                        null
                    );
                    _findObjectsOfTypeGeneric_hasIncludeInactiveParam = true;
                    if (_miFindObjectsOfTypeGeneric == null) {
                        MethodInfo[] ms = typeof(UnityEngine.Object).GetMethods(BindingFlags.Static | BindingFlags.Public);
                        foreach (MethodInfo m in ms) {
                            if (m.Name != "FindObjectsOfType" || !m.IsGenericMethod) continue;
                            _miFindObjectsOfTypeGeneric = m;
                            break;
                        }
                        _findObjectsOfTypeGeneric_hasIncludeInactiveParam = false;
                    }
                    _miFindObjectsOfTypeGeneric = _miFindObjectsOfTypeGeneric.MakeGenericMethod(typeof(T));
                } else {
                    if (_findObjectsInactiveType == null) _findObjectsInactiveType = typeof(GameObject).Assembly.GetType("UnityEngine.FindObjectsInactive");
                    if (_findObjectsSortModeType == null) _findObjectsSortModeType = typeof(GameObject).Assembly.GetType("UnityEngine.FindObjectsSortMode");
                    _miFindObjectsOfTypeGeneric = typeof(UnityEngine.Object).GetMethod(
                        "FindObjectsByType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                        new[] {_findObjectsInactiveType, _findObjectsSortModeType},
                        null
                    ).MakeGenericMethod(typeof(T));
                }
            }
            if (UnityEditorVersion.MajorVersion < 2023) {
                if (_findObjectsOfTypeGeneric_hasIncludeInactiveParam) return (T[])_miFindObjectsOfTypeGeneric.Invoke(null, new object[] {includeInactive});
                else return (T[])_miFindObjectsOfTypeGeneric.Invoke(null, null);
            } else {
                return (T[])_miFindObjectsOfTypeGeneric.Invoke(null, new object[] {includeInactive ? 0 : 1, 0});
            }
        }
        /// <summary>
        /// Warning: some versions of this method don't have the includeInactive parameter so it won't be taken into account
        /// </summary>
        public static Object[] FindObjectsOfType(Type type, bool includeInactive = false)
        {
            if (_miFindObjectsOfType == null) {
                if (UnityEditorVersion.MajorVersion < 2023) {
                    _miFindObjectsOfType = typeof(UnityEngine.Object).GetMethod(
                        "FindObjectsOfType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                        new[] {typeof(Type), typeof(bool)},
                        null
                    );
                    _findObjectsOfType_hasIncludeInactiveParam = true;
                    if (_miFindObjectOfType == null) {
                        _miFindObjectsOfType = typeof(UnityEngine.Object).GetMethod(
                            "FindObjectsOfType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                            new[] {typeof(Type)},
                            null
                        );
                        _findObjectsOfType_hasIncludeInactiveParam = false;
                    }
                } else {
                    if (_findObjectsInactiveType == null) _findObjectsInactiveType = typeof(GameObject).Assembly.GetType("UnityEngine.FindObjectsInactive");
                    if (_findObjectsSortModeType == null) _findObjectsSortModeType = typeof(GameObject).Assembly.GetType("UnityEngine.FindObjectsSortMode");
                    _miFindObjectsOfType = typeof(UnityEngine.Object).GetMethod(
                        "FindObjectsByType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                        new[] {typeof(Type), _findObjectsInactiveType, _findObjectsSortModeType},
                        null
                    );
                }
            }
            if (UnityEditorVersion.MajorVersion < 2023) {
                if (_findObjectsOfType_hasIncludeInactiveParam) return (Object[])_miFindObjectsOfType.Invoke(null, new object[] {type, includeInactive});
                else return (Object[])_miFindObjectsOfType.Invoke(null, new object[] {type});
            } else {
                return (Object[])_miFindObjectsOfType.Invoke(null, new object[] {type, includeInactive ? 0 : 1, 0});
            }
        }
    }
}