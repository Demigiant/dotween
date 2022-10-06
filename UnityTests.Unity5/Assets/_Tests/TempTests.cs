using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TempTests : BrainBase
{
    public Transform target;
    public Vector3 rot;
    public RotateMode rotMode;
    
    Tween t;
    
    override protected void Update()
    {
        base.Update();    
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (t != null && t.IsPlaying()) t.Complete();
            t = target.DORotate(rot, 1.3f, rotMode);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (t != null && t.IsPlaying()) t.Complete();
            t = target.DORotate(-rot, 1.3f, rotMode);
        }
    }
}