using System;
using System.Text;
using DG.Tweening;
using UnityEngine;
using System.Collections;

public class RectTransformConversionTests : BrainBase
{
    public RectTransform a, b;
    public RectTransform target;
    public Camera uiCam;

    void Start()
    {
        // Corners
        Vector3[] aCorners = new Vector3[4];
        Vector3[] targetCorners = new Vector3[4];
        a.GetWorldCorners(aCorners);
        target.GetWorldCorners(targetCorners);
        Debug.Log("A ::::::::::::::::::::::::::::::::::::::");
        Debug.Log("Corners: " + ArrayToString(aCorners));
        Debug.Log("WorldToScreenPoint: position: " + RectTransformUtility.WorldToScreenPoint(uiCam, a.position) + " anchoredPosition: " + RectTransformUtility.WorldToScreenPoint(uiCam, a.anchoredPosition));
        Debug.Log("B ::::::::::::::::::::::::::::::::::::::");
        Debug.Log("TARGET :::::::::::::::::::::::::::::::::");
        Debug.Log("Corners: " + ArrayToString(targetCorners));

        Vector2 localPoint;
        Vector2 aPivotDerivedOffset = new Vector2(a.rect.width * 0.5f + a.rect.xMin, a.rect.height * 0.5f + a.rect.yMin);
        Vector2 screenP = RectTransformUtility.WorldToScreenPoint(uiCam, a.position);
        screenP += aPivotDerivedOffset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(b, screenP, uiCam, out localPoint);
        Debug.Log(b.pivot + " - " + b.rect);
        Vector2 bPivotDerivedOffset = new Vector2(b.rect.width * 0.5f + b.rect.xMin, b.rect.height * 0.5f + b.rect.yMin);
        Debug.Log(bPivotDerivedOffset);
        b.anchoredPosition = b.anchoredPosition + localPoint - bPivotDerivedOffset;
//        Debug.Log(b.anchoredPosition + " - " + DOTweenUtils46.SwitchToRectTransform(a, b));
    }

    string ArrayToString(IList list)
    {
        StringBuilder s = new StringBuilder();
        for (int i = 0; i < list.Count; ++i) {
            if (i > 0) s.Append(", ");
            s.Append(list[i].ToString());
        }
        return s.ToString();
    }
}