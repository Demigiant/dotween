using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ShapeTweens : MonoBehaviour
{
    public enum FromMode
    {
        None,
        Dynamic,
        FromValue
    }

    public RectTransform pivot;
    public float duration = 2;
    public Ease ease = Ease.Linear;
    public FromMode fromMode = FromMode.None;
    public bool isRelative;
    public int loops;
    public LoopType loopType = LoopType.Yoyo;
    public Circle circle = new Circle();

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        if (circle.target != null) circle.Execute(this, pivot);

        float f = 50;
        // DOTween.To(() => f, x => f = x, 100, 2).From(-50, true, true)
        //     .OnUpdate(()=> Debug.Log(f));
        DOTween.To(() => f, x => f = x, 100, 2).From(true)
            .OnUpdate(()=> Debug.Log(f));
    }

    // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
    // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
    // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

    [Serializable]
    public abstract class Shape2D
    {
        public RectTransform target;
        public bool useRelativeCenter;
        public Vector2 relativeCenter = Vector2.zero; // If Vector2.zero is ignored
        public bool snapping;
    }

    [Serializable]
    public class Circle : Shape2D
    {
        public float degrees = 360;
        public float fromDegrees;

        public void Execute(ShapeTweens data, RectTransform pivot)
        {
            var t = target.DOShapeCircle(useRelativeCenter ? relativeCenter : pivot.anchoredPosition, degrees, data.duration, useRelativeCenter, snapping);
            if (data.fromMode != FromMode.None) {
                if (data.fromMode == FromMode.Dynamic) t.From(data.isRelative);
                else t.From(new Vector2(fromDegrees, 0), true, data.isRelative);
            } else t.SetRelative(data.isRelative);
            t.SetEase(data.ease)
                .SetLoops(data.loops, data.loopType);
        }
    }
}