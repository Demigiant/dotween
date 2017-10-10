using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class IndexOutOfRangeOnKill02 : MonoBehaviour
{
    public Transform target;
    public RectTransform targetUI;

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.L)) ReproTweenBug();
    }

    void ReproTweenBugKiller()
    {
        target.DOKill(true);
        targetUI.DOKill(true);
    }

    void ReproTweenBug()
    {
        ReproTweenBugKiller();

        var rotation = 2 * 360;
        var duration = rotation / 360f;

        target.DORotate(new Vector3(0, 0, rotation), duration, RotateMode.FastBeyond360).SetEase(Ease.OutCubic)
            .OnComplete(() => {
                targetUI.DOAnchorPos(Vector2.zero, 0.4f).SetEase(Ease.InBack)
                    .OnComplete(() => targetUI.DOKill(true));
            });
    }
}