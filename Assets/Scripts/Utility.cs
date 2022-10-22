using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public static class Utility
{
    public static bool InRange(Transform viewer, Transform target, float length, float fov)
    {
        // If the target is outside the viewer's sensor region, don't bother continuing
        if ((target.position - viewer.position).magnitude > length)
            return false;

        // We want to determine whether the angle between the player and enemy is less than half the enemy's fov
        // "return angle <= fov * 0.5f;"

        // We can solve for this angle using the dot product
        // a . b = ||a|| * ||b|| * cos(x)

        // We can simplify this equation because we know that a and b are normalized, so we can remove their magnitude
        // a . b = ||a|| * ||b|| * cos(x)
        // a . b = 1 * 1 * cos(x)
        // a . b = cos(x)
        // x = arccos(a . b)

        Vector3 targetDirection = (target.position - viewer.position).normalized;
        Vector3 viewerDirection = viewer.rotation * Vector3.forward;

        //float angle = Mathf.Acos(Vector3.Dot(targetDirection, viewerDirection)) * Mathf.Rad2Deg;
        //return angle <= fov * 0.5f;

        // We can compare a . b to cos(x) instead of solving for x to reduce complexity
        return Vector3.Dot(targetDirection, viewerDirection) > Mathf.Cos(Mathf.Deg2Rad * fov * 0.5f);
    }

    public static bool InRangeDebug(Transform viewer, Transform target, float length, float fov)
    {
        Vector3 front = viewer.transform.rotation.eulerAngles;
        Vector3 left = front;
        Vector3 right = front;
        Quaternion ql = new Quaternion();
        Quaternion qr = new Quaternion();
        left.y -= fov * 0.5f;
        right.y += fov * 0.5f;
        ql.eulerAngles = left;
        qr.eulerAngles = right;

        Color color = InRange(viewer, target, length, fov) ? Color.red : Color.green;
        Debug.DrawLine(viewer.position, viewer.position + ql * Vector3.forward * length, color);
        Debug.DrawLine(viewer.position, viewer.position + qr * Vector3.forward * length, color);
        return color == Color.red;
    }

    // p0 = end point 1, p1 = control point 1, p2 = control point 2, p3 = end point 2
    public static Vector3 EvaluateBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // MP = constraint matrix * points
        Vector3 r0 = -1.0f * p0 + 3.0f * p1 - 3.0f * p2 + p3;
        Vector3 r1 = 3.0f * p0 - 6.0f * p1 + 3.0f * p2;
        Vector3 r2 = -3.0f * p0 + 3.0f * p1;
        Vector3 r3 = p0;

        // U = au^3 + bu^2 + cu + d (cubic polynomial * t)
        Vector3 p = (r0 * t * t * t) + (r1 * t * t) + (r2 * t) + r3;
        return p;
    }

    // p0 = control point 1, p1 = end point 1, p2 = end point 2, p3 = control point 2
    public static Vector3 EvaluateCatmull(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // MP = constraint matrix * points
        Vector3 r0 = -p0 + 3.0f * p1 - 3.0f * p2 + p3;
        Vector3 r1 = 2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3;
        Vector3 r2 = p2 - p0;
        Vector3 r3 = 2.0f * p1;

        // U = au^3 + bu^2 + cu + d (cubic polynomial * t)
        Vector3 p = 0.5f * (r3 + (r2 * t) + (r1 * t * t) + (r0 * t * t * t));
        return p;
    }

    public delegate void LineMethod(Vector3 a, Vector3 b);

    public static void DrawBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, LineMethod lm)
    {
        const int lineResolution = 64;
        float t = 0.0f;
        for (int i = 0; i < lineResolution; i++)
        {
            Vector3 a = EvaluateBezier(p0, p1, p2, p3, t);
            t += (1.0f / (float)lineResolution);
            Vector3 b = EvaluateBezier(p0, p1, p2, p3, t);
            lm(a, b);
        }
    }

    public static void DrawCatmull(Transform[] points, LineMethod lm)
    {
        const int lineResolution = 64;
        Vector3 a, b, p0, p1, p2, p3;
        for (int i = 0; i < points.Length; i++)
        {
            a = points[i].position;
            PointsFromIndex(i, points, out p0, out p1, out p2, out p3);
            for (int j = 1; j <= lineResolution; ++j)
            {
                b = EvaluateCatmull(p0, p1, p2, p3, (float)j / lineResolution);
                lm(a, b);
                a = b;
            }
        }
    }

    public static void PointsFromIndex(int i, Transform[] points, out Vector3 p0, out Vector3 p1, out Vector3 p2, out Vector3 p3)
    {
        int n = points.Length;
        p0 = points[(i - 1 + n) % n].position;
        p1 = points[i % n].position;
        p2 = points[(i + 1) % n].position;
        p3 = points[(i + 2) % n].position;
    }

    // Higher order EvaluateCatmull function :)
    public static Vector3 EvaluateCatmull(float t, int i, Transform[] points)
    {
        Vector3 c0, e0, e1, c1;
        PointsFromIndex(i, points, out c0, out e0, out e1, out c1);
        return EvaluateCatmull(c0, e0, e1, c1, t);
    }

    public static void DrawCatmullPoint(float t, int i, Transform[] points)
    {
        Vector3 p0, p1, p2, p3;
        PointsFromIndex(i, points, out p0, out p1, out p2, out p3);
        Gizmos.DrawSphere(EvaluateCatmull(p0, p1, p2, p3, t), 0.25f);
    }

    public static Matrix4x4 FrenetFrame(Vector3 from, Vector3 to, Vector3 up)
	{
		Matrix4x4 rotation = Matrix4x4.identity;
		Vector3 forward = (to - from).normalized;
		Vector3 right = Vector3.Cross(forward, up);
		Vector3 above = Vector3.Cross(forward, right);
		rotation.SetColumn(0, right);
		rotation.SetColumn(1, above);
		rotation.SetColumn(2, forward);
		return rotation;
	}

    public static float Quadratic(float a, float b, float c)
    {
        float twoA = (2.0f * a);
        float b2 = (b * b);
        float fourAC = (4.0f * a * c);
        return (-b + Mathf.Sqrt(b2 - fourAC)) / twoA;
    }
}
