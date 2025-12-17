using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins;
using UnityEngine;
using UnityEngine.UI;

public class ShapeTweens : MonoBehaviour
{
    public enum FromMode
    {
        None,
        Dynamic,
        DynamicImmediate,
        FromValue
    }

    public RectTransform pivot;
    public float duration = 2;
    public Ease ease = Ease.Linear;
    public FromMode fromMode = FromMode.None;
    public bool isRelative;
    public int loops;
    public LoopType loopType = LoopType.Yoyo;
    public Circle[] circleTweens;

    void Start()
    {
        // DEBUG TESTS
        float a = 0, b = 0, c = 0, d = 0, e = 0;
        DOTween.To(() => a, x => a = x, 10, 1);
        DOTween.To(() => b, x => b = x, 10, 1).From();
        DOTween.To(() => c, x => c = x, 10, 1).From(false, false);
        DOTween.To(() => d, x => d = x, 10, 1).From(3);
        DOTween.To(() => e, x => e = x, 10, 1).From(3, false)
            .OnUpdate(()=> Debug.Log(a + "/" + b + "/" + c + "/" + d + "/" + e));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            for (int i = 0; i < circleTweens.Length; i++) circleTweens[i].RecreateFromHere(this, pivot);
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            for (int i = 0; i < circleTweens.Length; i++) circleTweens[i].PlayBackwards();
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            for (int i = 0; i < circleTweens.Length; i++) circleTweens[i].PlayForward();
        }
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

        Tween _tween;

        public void RecreateFromHere(ShapeTweens data, RectTransform pivot)
        {
            if (target == null) return;
            if (_tween != null) _tween.Kill();
            _tween = Create(data, pivot);
        }

        public void PlayForward()
        {
            if (_tween != null) _tween.PlayForward();
        }

        public void PlayBackwards()
        {
            if (_tween != null) _tween.PlayBackwards();
        }

        protected abstract Tween Create(ShapeTweens data, RectTransform pivot);
    }

    [Serializable]
    public class Circle : Shape2D
    {
        public float degrees = 360;
        public float fromDegrees;

        protected override Tween Create(ShapeTweens data, RectTransform pivot)
        {
            var t = target.DOShapeCircle(useRelativeCenter ? relativeCenter : pivot.anchoredPosition, degrees, data.duration, useRelativeCenter, snapping)
                .SetAutoKill(false);
            switch (data.fromMode) {
            case FromMode.Dynamic:
                t.From(false, data.isRelative);
                break;
            case FromMode.DynamicImmediate:
                t.From(data.isRelative);
                break;
            case FromMode.FromValue:
                t.From(fromDegrees, true, data.isRelative);
                break;
            default:
                t.SetRelative(data.isRelative);
                break;
            }
            t.SetEase(data.ease)
                .SetLoops(data.loops, data.loopType);
            return t;
        }
    }
}