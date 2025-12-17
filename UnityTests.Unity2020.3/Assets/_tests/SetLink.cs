using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SetLink : BrainBase
{
    public Transform tweenTarget;
    public bool autoKill = true;
    public bool pauseAtStartup = true;
    public int loops = 2;
    public float duration = 1;
    public GameObject linkTarget;
    public LinkBehaviour linkBehaviour;

    void Start()
    {
        Tween t = tweenTarget.DOMoveX(3, duration).SetLoops(loops, LoopType.Yoyo).SetEase(Ease.Linear)
            .SetLink(linkTarget, linkBehaviour);
        if (autoKill) t.SetAutoKill();
        if (pauseAtStartup) t.Pause();
    }
}