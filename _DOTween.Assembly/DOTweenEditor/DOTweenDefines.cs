// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/09/30 11:59
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using System;
using System.IO;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    /// <summary>
    /// Not used as menu item anymore, but as a utility function
    /// </summary>
    static class DOTweenDefines
    {
        public static readonly Def DOTween = new Def("DOTWEEN");
        // Inverted defines (module is active if the define IS NOT present)
        public static readonly Def NoAudio = new Def("DOTWEEN_NOAUDIO", true);
        public static readonly Def NoPhysics = new Def("DOTWEEN_NOPHYSICS", true);
        public static readonly Def NoPhysics2D = new Def("DOTWEEN_NOPHYSICS2D", true);
        public static readonly Def NoSprites = new Def("DOTWEEN_NOSPRITES", true);
        public static readonly Def NoUI = new Def("DOTWEEN_NOUI", true);
        // Normal defines (module is active if the define IS present)
        public static readonly Def UIToolkit = new Def("DOTWEEN_UITOOLKIT");
        // - Demigiant assets
        public static readonly Def DeAudio = new Def("DOTWEEN_DEAUDIO");
        public static readonly Def DeUnityExtended = new Def("DOTWEEN_DEUNITYEXTENDED");
        // - External assets
        public static readonly Def TK2D = new Def("DOTWEEN_TK2D");
        public static readonly Def TextMeshPro = new Def("DOTWEEN_TEXTMESHPRO");
        public static readonly Def EasyPerformantOutline = new Def("DOTWEEN_EPO");
        
        // Legacy (in versions older than 1.2.050)
        static readonly Def _LegacyAudio = new Def("DOTAUDIO");
        static readonly Def _LegacyPhysics = new Def("DOTPHYSICS");
        static readonly Def _LegacyPhysics2D = new Def("DOTPHYSICS2D");
        static readonly Def _LegacySprite = new Def("DOTSPRITE");
        static readonly Def _LegacyUI = new Def("DOTUI");
        static readonly Def _LegacyTextMeshPro = new Def("DOTWEEN_TMP");
        static readonly Def _LegacyNoRigidBody = new Def("DOTWEEN_NORBODY");

        // All defines except legacy ones
        static readonly Def[] _AllValidDefines = new Def[] {
            DOTween,
            NoAudio, NoPhysics, NoPhysics2D, NoSprites, NoUI,
            UIToolkit,
            DeAudio, DeUnityExtended,
            TK2D, TextMeshPro, EasyPerformantOutline
        };
        // All legacy defines
        static readonly Def[] _AllLegacyDefines = new Def[] {
            _LegacyAudio, _LegacyPhysics, _LegacyPhysics2D, _LegacySprite, _LegacyUI, _LegacyTextMeshPro,
            _LegacyTextMeshPro, _LegacyNoRigidBody
        };

        #region Public Methods

        /// <summary>Sets the modules bool values to the current defines state</summary>
        public static void RefreshDOTweenSettings(DOTweenSettings src)
        {
            if (src == null) return;
            
            src.modules.audioEnabled = !NoAudio.enabled;
            src.modules.physicsEnabled = !NoPhysics.enabled;
            src.modules.physics2DEnabled = !NoPhysics2D.enabled;
            src.modules.spriteEnabled = !NoSprites.enabled;
            src.modules.uiEnabled = !NoUI.enabled;
            
            src.modules.uiToolkitEnabled = UIToolkit.enabled;
            
            src.modules.epoOutlineEnabled = EasyPerformantOutline.enabled;
            
            src.modules.deAudioEnabled = DeAudio.enabled;
            src.modules.deUnityExtendedEnabled = DeUnityExtended.enabled;
            src.modules.textMeshProEnabled = TextMeshPro.enabled;
            src.modules.tk2DEnabled = TK2D.enabled;
            
            EditorUtility.SetDirty(src);
        }

        /// <summary>Refreshes the enabled state of all defines</summary>
        public static void RefreshAll()
        {
            foreach (Def def in _AllValidDefines) def.Refresh();
        }

        /// <summary>Applies the guiEnabled value state to each define, adding or removing them based on that</summary>
        public static void ApplyGUIEnabledToAll()
        {
            foreach (Def def in _AllValidDefines) {
                if (def == DOTween) continue;
                if (def.guiEnabled) def.Add();
                else def.Remove();
                def.Refresh();
            }
        }

        /// <summary>Removes all DOTween defines including the ones for external assets</summary>
        public static void RemoveAll()
        {
            foreach (Def def in _AllValidDefines) def.Remove();
        }

        /// <summary>Removes all legacy defines</summary>
        public static void RemoveAllLegacy()
        {
            foreach (Def def in _AllLegacyDefines) def.Remove();
        }

        #endregion

        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
        // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        public class Def
        {
            public readonly string id;
            public readonly bool isInverted; // Inverted defines activate code when they are FALSE instead of TRUE
            public bool enabled { get { return _foo_enabled; } }
            public bool guiEnabled; // Refreshed along enabled but also changed by DOTweenUtilityWindowModules GUI
            
            bool _foo_enabled;

            public Def(string id, bool isInverted = false)
            {
                this.id = id;
                this.isInverted = isInverted;
            }

            public void Refresh()
            {
                _foo_enabled = guiEnabled = EditorUtils.HasGlobalDefine(id);
            }

            public void Enable(bool enable)
            {
                if (enable) Add();
                else Remove();
            }

            /// <summary>Removes the define if present</summary>
            public void Remove()
            {
                EditorUtils.RemoveGlobalDefine(id);
            }

            /// <summary>Adds the define if it's not already present</summary>
            public void Add()
            {
                EditorUtils.AddGlobalDefine(id);
            }
        }
    }
}