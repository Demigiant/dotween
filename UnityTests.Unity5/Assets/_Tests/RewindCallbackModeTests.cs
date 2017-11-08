using DG.Tweening;
using UnityEngine;

public class RewindCallbackModeTests : BrainBase
{
    public Transform target;
    Tween t;

    void Start()
    {
        t = target.DOMoveX(3, 2).SetLoops(-1, LoopType.Yoyo).SetAutoKill(false)
            .OnRewind(() => Debug.Log("<color=#00ff00>Rewind callback</color>"));
    }

    public void Play()
    {
        t.Play();
    }

    public void PlayForward()
    {
        t.PlayForward();
    }

    public void Pause()
    {
        t.Pause();
    }

    public void Rewind()
    {
        t.Rewind();
    }

    public void PlayBackwards()
    {
        t.PlayBackwards();
    }

    public void SmoothRewind()
    {
        t.SmoothRewind();
    }
}