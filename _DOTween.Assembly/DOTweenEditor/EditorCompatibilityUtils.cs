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
        static Type _findObjectsInactiveType;
        static Type _findObjectsSortModeType;

        public static T FindObjectOfType<T>(bool includeInactive = false)
        {
            if (_miFindObjectOfTypeGeneric == null) {
                if (UnityEditorVersion.MajorVersion < 2023) {
                    _miFindObjectOfTypeGeneric = typeof(UnityEngine.Object).GetMethod(
                        "FindObjectOfType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                        new[] {typeof(bool)},
                        null
                    ).MakeGenericMethod(typeof(T));
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
                return (T)_miFindObjectOfTypeGeneric.Invoke(null, new object[] {includeInactive});
            } else {
                return (T)_miFindObjectOfTypeGeneric.Invoke(null, new object[] {includeInactive ? 0 : 1});
            }
        }
        public static Object FindObjectOfType(Type type, bool includeInactive = false)
        {
            if (_miFindObjectOfType == null) {
                if (UnityEditorVersion.MajorVersion < 2023) {
                    _miFindObjectOfType = typeof(UnityEngine.Object).GetMethod(
                        "FindObjectOfType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                        new[] {typeof(Type), typeof(bool)},
                        null
                    );
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
                return (Object)_miFindObjectOfType.Invoke(null, new object[] {type, includeInactive});
            } else {
                return (Object)_miFindObjectOfType.Invoke(null, new object[] {type, includeInactive ? 0 : 1});
            }
        }

        public static T[] FindObjectsOfType<T>(bool includeInactive = false)
        {
            if (_miFindObjectsOfTypeGeneric == null) {
                if (UnityEditorVersion.MajorVersion < 2023) {
                    _miFindObjectsOfTypeGeneric = typeof(UnityEngine.Object).GetMethod(
                            "FindObjectsOfType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                            new[] {typeof(bool)},
                            null
                        ).MakeGenericMethod(typeof(T));
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
                return (T[])_miFindObjectsOfTypeGeneric.Invoke(null, new object[] {includeInactive});
            } else {
                return (T[])_miFindObjectsOfTypeGeneric.Invoke(null, new object[] {includeInactive ? 0 : 1, 0});
            }
        }
        public static Object[] FindObjectsOfType(Type type, bool includeInactive = false)
        {
            if (_miFindObjectsOfType == null) {
                if (UnityEditorVersion.MajorVersion < 2023) {
                    _miFindObjectsOfType = typeof(UnityEngine.Object).GetMethod(
                        "FindObjectsOfType", BindingFlags.Static | BindingFlags.Public, Type.DefaultBinder,
                        new[] {typeof(Type), typeof(bool)},
                        null
                    );
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
                return (Object[])_miFindObjectsOfType.Invoke(null, new object[] {type, includeInactive});
            } else {
                return (Object[])_miFindObjectsOfType.Invoke(null, new object[] {type, includeInactive ? 0 : 1, 0});
            }
        }
    }
}