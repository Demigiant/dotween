using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ShapeTweens : MonoBehaviour
{
    public RectTransform pivot;
    public float duration = 2;
    public Ease ease = Ease.Linear;
    public Circle circle = new Circle();

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        if (circle.target != null) circle.Execute(pivot, duration, ease);
    }

    // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
    // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
    // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

    [Serializable]
    public abstract class Shape2D
    {
        public RectTransform target;
        public Vector2 relativeCenter = Vector2.zero; // If Vector2.zero is ignored
        public bool snapping;
        protected bool useRelativeCenter { get { return !Mathf.Approximately(relativeCenter.x, 0) || !Mathf.Approximately(relativeCenter.y, 0); } }
    }

    [Serializable]
    public class Circle : Shape2D
    {
        public float degrees = 360;
        public bool relativeDegrees;

        public void Execute(RectTransform pivot, float duration, Ease ease)
        {
            Debug.Log(useRelativeCenter);
            Tween t = target.DOShapeCircle(useRelativeCenter ? relativeCenter : pivot.anchoredPosition, degrees, duration, useRelativeCenter, snapping);
            if (relativeDegrees) t.SetRelative();
            t.SetEase(ease);
        }
    }
}