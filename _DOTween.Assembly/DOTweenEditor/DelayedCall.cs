// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/03/11 18:21

using System;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    public class DelayedCall
    {
        public float delay;
        public Action callback;
        float _startupTime;

        public DelayedCall(float delay, Action callback)
        {
            this.delay = delay;
            this.callback = callback;
            _startupTime = Time.realtimeSinceStartup;
            EditorApplication.update += Update;
        }

        void Update()
        {
            if (Time.realtimeSinceStartup - _startupTime >= delay) {
                if (EditorApplication.update != null) EditorApplication.update -= Update;
                if (callback != null) callback();
            }
        } 
    }
}