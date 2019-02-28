using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PathsBezier : BrainBase
{
    public Transform target;
    public float duration = 3;
    public bool closedPath = false;
    public Ease easeType = Ease.Linear;
    public bool loop = true;
    public LoopType loopType = LoopType.Restart;
    public PathType pathType = PathType.CubicBezier;
    public int wpsToUse = 0;
    public Vector3[] wps0 = new[] {
        new Vector3(1, 1, 0), // wp
        new Vector3(0, 0.75f, 0),
        new Vector3(0.25f, 1, 0),
        new Vector3(2, 0, 0), // wp
        new Vector3(1.75f, 1, 0),
        new Vector3(2, 0.75f, 0),
        new Vector3(1, -1, 0), // wp
        new Vector3(2, -0.75f, 0),
        new Vector3(1.75f, -1, 0),
    };
    public Vector3[] wps1 = new[] {
        new Vector3(1, 1, 0), // wp
        new Vector3(0, 0.75f, 0),
        new Vector3(0.25f, 1, 0),
        new Vector3(2, 0, 0), // wp
        new Vector3(1.75f, 1, 0),
        new Vector3(2, 0.75f, 0),
        new Vector3(1, -1, 0), // wp
        new Vector3(2, -0.75f, 0),
        new Vector3(1.75f, -1, 0),
        new Vector3(0, 0, 0), // wp
        new Vector3(0.25f, -1, 0),
        new Vector3(0f, -0.75f, 0)
    };
    public Vector3[] wps2 = new[] {
        new Vector3(0, 0, 0), // wp
        new Vector3(0, 0.75f, 0),
        new Vector3(0.25f, 1, 0),
        new Vector3(1, 1, 0), // wp
        new Vector3(0, 0.75f, 0),
        new Vector3(0.25f, 1, 0),
        new Vector3(2, 0, 0), // wp
        new Vector3(1.75f, 1, 0),
        new Vector3(2, 0.75f, 0),
        new Vector3(1, -1, 0), // wp
        new Vector3(2, -0.75f, 0),
        new Vector3(1.75f, -1, 0),
        new Vector3(0, 0, 0), // wp
        new Vector3(0.25f, -1, 0),
        new Vector3(0f, -0.75f, 0)
    };

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Creating CubicBezier path");
        Vector3[] wps = wpsToUse == 0 ? wps0 : wpsToUse == 1 ? wps1 : wps2;
        target.DOPath(wps, duration, pathType).SetOptions(closedPath).SetLookAt(0.001f)
            .SetLoops(loop ? -1 : 1, loopType).SetEase(easeType)
            .OnWaypointChange(x=> Debug.Log("Waypoint changed ► " + x));
    }
}