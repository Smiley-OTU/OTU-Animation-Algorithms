using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// Its easy to make a looping path with catmull-rom splines because all points lie on the curve
public class CatmullSpline : MonoBehaviour
{
    public Transform[] points;
    private int i = 0;
    private float t = 0.0f;

    void Update()
    {
        transform.position = Utility.EvaluateCatmull(t, i, points);

        t += Time.smoothDeltaTime;
        if (t >= 1.0f)
        {
            t = 0.0f;
            i++;
        }
    }

    void OnDrawGizmos()
    {
        Utility.DrawCatmull(points, Gizmos.DrawLine);
    }
}
