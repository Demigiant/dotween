using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShakeExample : MonoBehaviour
{
    public Transform target;
    public Tween shakeTween;

    void Start()
    {
        // Create the tween (using default values)
        shakeTween = target.DOShakePosition(1.5f).SetAutoKill(false).Pause();
    }

    // Called by UI button
    public void Shake()
    {
        shakeTween.Restart();
    }
}