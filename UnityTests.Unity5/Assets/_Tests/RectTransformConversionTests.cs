using System;
using System.Text;
using UnityEngine;
using System.Collections;

public class RectTransformConversionTests : BrainBase
{
    public RectTransform a, b;
    public RectTransform target;

    void Start()
    {
        // Corners
        Vector3[] aCorners = new Vector3[4];
        Vector3[] targetCorners = new Vector3[4];
        a.GetWorldCorners(aCorners);
        target.GetWorldCorners(targetCorners);
        Debug.Log("A ::::::::::::::::::::::::::::::::::::::");
        Debug.Log("Corners: " + ArrayToString(aCorners));
        Debug.Log("B ::::::::::::::::::::::::::::::::::::::");
        Debug.Log("TARGET :::::::::::::::::::::::::::::::::");
        Debug.Log("Corners: " + ArrayToString(targetCorners));
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