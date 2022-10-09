using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TempTests : BrainBase
{
    public Transform target;
    public Vector3 rot;
    public RotateMode rotMode;
    public float duration = 1;
    public bool relative = false;
    
    Tween t;
    
    override protected void Update()
    {
        base.Update();    
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            CompleteTween();
            t = target.DORotate(rot, duration, rotMode);
            if (relative) t.SetRelative();
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            CompleteTween();
            t = target.DORotate(-rot, duration, rotMode);
            if (relative) t.SetRelative();
        }
    }
    
    void CompleteTween()
    {
        if (t != null && t.IsActive() && t.IsPlaying()) {
            t.Kill(true);
        }
    }
}