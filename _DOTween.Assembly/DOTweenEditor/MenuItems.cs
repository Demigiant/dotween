// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2018/08/07 18:05
// License Copyright (c) Daniele Giardini
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    static class MenuItems
    {
        [MenuItem("GameObject/Demigiant/DOTween Manager", false, 20)]
        static void CreateDOTweenComponent(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("[DOTween]");
            go.AddComponent<DOTweenComponent>();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}