// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/03/21 17:57

using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerTweens : BrainBase
{
    public AudioMixerGroup mixerGroup;

    void Start()
    {
        mixerGroup.audioMixer.DOSetFloat("volume", -80, 2).SetEase(Ease.InQuart).SetAutoKill(false).Pause();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Fade Out")) mixerGroup.audioMixer.DOPlayForward();
        if (GUILayout.Button("Fade In")) mixerGroup.audioMixer.DOPlayBackwards();
    }
}