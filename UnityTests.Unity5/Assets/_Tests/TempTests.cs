using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    public float TimeScale = 1;
    public int ConcurrentRunningSequences = 10000;
    public int loops = 1;
    public LoopType loopType = LoopType.Restart;

    int _createdSequenceCount;
    int _completedSequenceCount;

    int _callbackZeroCount;
    int _callbackOneCount;
    int _callbackTwoCount;
    int _callbackThreeCount;

    bool _isTestingInProgress = false;
    float _time;

    void Start()
    {
        DOTween.Init();
        DOTween.SetTweensCapacity(200, 30000);
    }

    void Update()
    {
        Time.timeScale = TimeScale;
    }

    void OnGUI()
    {
        GUILayout.Label((Time.realtimeSinceStartup - _time).ToString("n3"));
        GUILayout.Space(4);

        if (!_isTestingInProgress)
        {
            if (GUILayout.Button("Start Testing"))
            {
                _isTestingInProgress = true;
                _time = Time.realtimeSinceStartup;

                for (int i = 0; i < ConcurrentRunningSequences; i++)
                {
                    StartSequence();
                }
            }
        }
        else
        {
            if (GUILayout.Button("Stop Testing"))
            {
                _isTestingInProgress = false;
            }
        }

        GUILayout.Label(string.Format("Created Sequences: {0}", _createdSequenceCount.ToString("n0")));
        GUILayout.Label(string.Format("Completed Sequences: {0}", _completedSequenceCount.ToString("n0")));

        GUILayout.Space(20);

        GUILayout.Label(string.Format("Callback 0 Count: {0}", _callbackZeroCount.ToString("n0")));
        GUILayout.Label(string.Format("Callback 1 Count: {0}", _callbackOneCount.ToString("n0")));
        GUILayout.Label(string.Format("Callback 2 Count: {0}", _callbackTwoCount.ToString("n0")));
        GUILayout.Label(string.Format("Callback 3 Count: {0}", _callbackThreeCount.ToString("n0")));
    }

    void StartSequence()
    {
        _createdSequenceCount++;
        int added0 = 0;
        int added1 = 0;
        int added2 = 0;
        int added3 = 0;
        int check = loops - 1;

        DOTween.Sequence()
            .SetLoops(loops, LoopType.Restart)
            .AppendCallback(() =>
            {
                if (added0 > check) LogError(0);
                _callbackZeroCount++;
                added0++;
            })
//            .AppendInterval(0.25f) // THIS WORKS
            .AppendInterval(UnityEngine.Random.Range(0.25f, 1.5f))
            .AppendCallback(() =>
            {
                if (added1 > check) LogError(1);
                _callbackOneCount++;
                added1++;
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                if (added2 > check) LogError(2);
                _callbackTwoCount++;
                added2++;
            })
            .AppendInterval(0.25f)
            .AppendCallback(() =>
            {
                if (added3 > check) LogError(3);
                _callbackThreeCount++;
                added3++;
            })
            .OnComplete(()=> {
                _completedSequenceCount++;
                if (_isTestingInProgress) StartSequence();
            });
    }

    void LogError(int index)
    {
        Debug.LogError(index + " already added");
//        Debug.Log("currUseInvers-currPrevPosIsInverse: " + Sequence.currUseInverse + " - " + Sequence.currPrevPosIsInverse);
//        Debug.Log("currFrom-to: " + Sequence.currFrom.ToString("n10") + " - " + Sequence.currTo.ToString("n10"));
//        Debug.Log("currCallbackStart-End time: " + Sequence.currCallbackTime.ToString("n10") + " - " + Sequence.currCallbackEndTime.ToString("n10"));
    }
}