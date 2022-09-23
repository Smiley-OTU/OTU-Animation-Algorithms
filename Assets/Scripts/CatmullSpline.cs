using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// Its easy to make a looping path with catmull-rom splines because all points lie on the curve
public class CatmullSpline : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform p0;
    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p;

    private Transform[] points = new Transform[4];
    private int index;
    private float t;

    private void Sample(int i, Transform[] p, out Vector3 p0, out Vector3 p1, out Vector3 p2, out Vector3 p3)
    {
        int n = p.Length;
        p0 = p[(i - 1 + n) % n].position;
        p1 = p[i % n].position;
        p2 = p[(i + 1) % n].position;
        p3 = p[(i + 2) % n].position;
    }

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
        Vector3 c0, e0, e1, c1;
        Sample(index, points, out c0, out e0, out e1, out c1);
        Utility.DrawCatmull(c0, e0, e1, c1, lineRenderer);
        p.position = Utility.EvaluateCatmull(c0, e0, e1, c1, t);

        t += Time.smoothDeltaTime;
        if (t >= 1.0f)
        {
            t = 0.0f;
            index++;
        }
    }
}
