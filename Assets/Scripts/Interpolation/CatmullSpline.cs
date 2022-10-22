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
        Vector3 p0 = Interpolation.EvaluateCatmull(t, i, points);
        t += Time.smoothDeltaTime;
		Vector3 p1 = Interpolation.EvaluateCatmull(t, i, points);
        Matrix4x4 rotation = Interpolation.FrenetFrame(p1, p0, Vector3.up);
        transform.position = p1;
		transform.rotation = rotation.rotation;

        if (t >= 1.0f)
        {
            t = 0.0f;
            i++;
        }
    }

    void OnDrawGizmos()
    {
        Interpolation.DrawCatmull(points, Gizmos.DrawLine);
    }
}
