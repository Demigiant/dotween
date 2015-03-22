// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/07/07 14:52
// 
// License Copyright (c) Daniele Giardini.
// This work is subject to the terms at http://dotween.demigiant.com/license.php

using DG.Tweening.Core;

#pragma warning disable 1591
namespace DG.Tweening.Plugins.Core
{
    public interface IPlugSetter<T1, out T2, TPlugin, out TPlugOptions>
    {
        DOGetter<T1> Getter();
        DOSetter<T1> Setter();
        T2 EndValue();
        TPlugOptions GetOptions();
    }
}