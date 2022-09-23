using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bezier splines are not good for looping paths because not all points lie on the curve
public class BezierSpline : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform p0;
    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p;
    private Transform[] points = new Transform[4];

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 128;
        points[0] = p0;
        points[1] = p1;
        points[2] = p2;
        points[3] = p3;
    }

    void Update()
    {
        float t = Mathf.Cos(Time.realtimeSinceStartup) * 0.5f + 0.5f;
        Utility.DrawBezier(p0.position, p1.position, p2.position, p3.position, lineRenderer);
        p.position = Utility.EvaluateBezier(p0.position, p1.position, p2.position, p3.position, t);
    }
}
