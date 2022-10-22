using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bezier splines are not good for looping paths because not all points lie on the curve
public class BezierSpline : MonoBehaviour
{
    public Transform p0;
    public Transform p1;
    public Transform p2;
    public Transform p3;

    void Update()
    {
        float t = Mathf.Cos(Time.realtimeSinceStartup) * 0.5f + 0.5f;
        transform.position = Interpolation.EvaluateBezier(p0.position, p1.position, p2.position, p3.position, t);
    }

    void OnDrawGizmos()
    {
        Interpolation.DrawBezier(p0.position, p1.position, p2.position, p3.position, Gizmos.DrawLine);
    }
}
