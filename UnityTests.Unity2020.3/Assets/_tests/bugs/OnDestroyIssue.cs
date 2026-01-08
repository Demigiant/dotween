using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OnDestroyIssue : MonoBehaviour
{
    static List<OnDestroyIssue> list = new List<OnDestroyIssue>();
 
    int _listIndex;
    Tween _tween;
 
    private void Awake()
    {
        _listIndex = list.Count;
        list.Add(this);
    }
 
    void Start()
    {
        NewTween();
    }

    void OnApplicationQuit()
    {
        Debug.Log("<color=#ff0000>QUITTING from " + this.name + "</color>");
    }

    void OnDestroy()
    {
        Debug.Log("<color=#00ff00>Destroy " + this.name + "</color>");
        KillTween();
 
        // Start a new tween on any other registered TweenTest
        int index = _listIndex + 1 < list.Count ? _listIndex + 1 : 0;
        list[index].NewTween();
 
        list.Remove(this);
    }
 
    void NewTween()
    {
        Debug.Log("<color=#00ff00>New tween for " + this.name + "</color>");
        KillTween();
 
        _tween = transform.DOMove(Vector3.zero, 10000f)
            .OnComplete(OnComplete);
    }
 
    void OnComplete()
    {
        _tween = null;
    }
 
    private void KillTween()
    {
        if (_tween != null && _tween.active) {
            Debug.Log("<color=#00ff00>Kill tween for " + this.name + "</color>");
            _tween.Kill();
            _tween = null;
        }
    }
}