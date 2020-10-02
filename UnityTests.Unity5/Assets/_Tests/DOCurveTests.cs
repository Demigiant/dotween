using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DOCurveTests : MonoBehaviour
{
    [Range(1, 100)]
    public int resolution = 2;
    public LineRenderer curveR, startCPR, endCPR;
    public Transform startP, startCP, endP, endCP;

    int _lastResolution;
    Vector3 _lastStartP, _lastStartCP, _lastEndP, _lastEndCP;

    void Start()
    {
        Refresh();
    }

    void Update()
    {
        bool refresh = _lastResolution != resolution
                       || _lastStartP != startP.position || _lastStartCP != startCP.position
                       || _lastEndP != endP.position || _lastEndCP != endCP.position;
        if (refresh) Refresh();
    }

    void Refresh()
    {
        _lastResolution = resolution;
        _lastStartP = startP.position;
        _lastStartCP = startCP.position;
        _lastEndP = endP.position;
        _lastEndCP = endCP.position;

        // Draw control points lines
        Vector3[] startCPLine = new[] { startP.position, startCP.position };
        Vector3[] endCPLine = new[] { endP.position, endCP.position };
        startCPR.positionCount = 2;
        endCPR.positionCount = 2;
        startCPR.SetPositions(startCPLine);
        endCPR.SetPositions(endCPLine);

        // Draw curve
        Vector3[] points = DOCurve.CubicBezier.GetSegmentPointCloud(startP.position, startCP.position, endP.position, endCP.position, resolution);
        curveR.positionCount = points.Length;
        curveR.SetPositions(points);
    }
}