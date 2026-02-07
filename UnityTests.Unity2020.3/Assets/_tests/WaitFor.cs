using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitFor : BrainBase
{
    [Header("References")]
    [SerializeField] RectTransform _target;
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] RectTransform _content;
    [SerializeField] TMP_Text _tfLog;

    float _startTime;
    
    async void Start()
    {
        _tfLog.text = "";
        
        // Note: not using Task.Delay because that doesn't work on WebGL
        const float startDelay = 1;
        _startTime = Time.realtimeSinceStartup;
        Log("Async Start called", "Will avoid using Task.Delay because it doesn't work with WebGL, instead will use Task.Yield");
        while (Time.realtimeSinceStartup - _startTime < startDelay) await Task.Yield();
        Log($"Async awaited {startDelay}\"");
        Log("Starting tween of 2\"");
        Tween tween = _target.DOAnchorPosY(50, 1f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
        await tween.AsyncWaitForCompletion();
        Log("Tween completed", "Notified correctly via tween.AsyncWaitForCompletion");
    }

    void Log(string text, string note = null)
    {
        string msg = $"<size=40%>\n\n</size><size=80%><color=#62fcf7>frame: {Time.frameCount}</color> - <color=#b9fc62>elapsed: {Time.realtimeSinceStartup - _startTime:0.###}</color></size>\n<indent=8>{text}</indent>";
        if (note != null) msg += $"\n<indent=8>: </indent><indent=16><size=85%><color=#AAAAAA>{note}</color></size></indent>";
        _tfLog.text += msg;
        Refresh();
    }

    void Refresh()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_scrollRect.transform);
        _scrollRect.verticalNormalizedPosition = 0;
    }
}
