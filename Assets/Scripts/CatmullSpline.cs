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
        Vector3 p0 = Utility.EvaluateCatmull(t, i, points);
        t += Time.smoothDeltaTime;
		Vector3 p1 = Utility.EvaluateCatmull(t, i, points);
        transform.position = p1;//Utility.EvaluateCatmull(t, i, points);

        Vector3 forward = (p1 - p0).normalized;
        Vector3 right = Vector3.Cross(forward, Vector3.up);
        Vector3 up = Vector3.Cross(forward, right);
        Matrix4x4 rotation = Matrix4x4.identity;
        rotation.SetColumn(0, right);
        rotation.SetColumn(1, up);
        rotation.SetColumn(2, forward);
        transform.rotation = rotation.rotation;

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
