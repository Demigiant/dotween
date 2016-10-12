using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TempPro : MonoBehaviour
{
	public DOTweenPath path;

    void Start ()
    {
        path.GetTween().OnWaypointChange(OnWaypointChange);
    }

    void OnWaypointChange (int index)
    {
        Debug.Log(index);
    }
}