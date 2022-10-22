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
        Matrix4x4 rotation = Utility.FrenetFrame(p1, p0, Vector3.up);
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
        Utility.DrawCatmull(points, Gizmos.DrawLine);
    }
}
