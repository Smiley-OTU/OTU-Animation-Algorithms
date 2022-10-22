using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolation
{
    public static float SolveT(float n, float a, float b)
    {
        return (n - a) / (b - a);
    }

    public static Vector3 Decasteljau(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 A = Vector3.Lerp(p0, p1, t);
        Vector3 B = Vector3.Lerp(p1, p2, t);
        Vector3 C = Vector3.Lerp(p2, p3, t);

        Vector3 D = Vector3.Lerp(A, B, t);
        Vector3 E = Vector3.Lerp(B, C, t);

        return Vector3.Lerp(D, E, t);
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
}
