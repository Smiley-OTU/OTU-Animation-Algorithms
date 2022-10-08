using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Distance
{
    public float value;
}

public struct Duration
{
    public float value;
}

public class ArcY
{
    public static ArcY From(Distance distance)
    {
        float d = distance.value;
        float vi = ViFromDistance(d);
        float t = SolveDuration(d, vi);
        return new ArcY(d, t, vi);
    }

    public static ArcY From(Duration duration)
    {
        float t = duration.value;
        float vi = ViFromDuration(t);
        float d = SolveDistance(t, vi);
        return new ArcY(d, t, vi);
    }

    public float LaunchVelocity
    {
        get; private set;
    }

    public float Distance
    {
        get; private set;
    }

    public float Duration
    {
        get; private set;
    }

    public static float SolveDistance(float duration, float vi)
    {
        // Motion equation 2 --> df = di + vi * t + 0.5(a * t * t)
        float t = duration * 0.5f;
        float d = vi * t + 0.5f * UnityEngine.Physics.gravity.y * t * t;
        return d;
    }

    public static float SolveDuration(float distance, float vi)
    {
        // Motion equation 2
        // df = di + vi * t + 0.5(a * t * t)
        // t = (-vi + sqrt(vi^2 - 4a * df)) / 2a
        return Utility.Quadratic(0.5f * UnityEngine.Physics.gravity.y, vi, -distance) * 2.0f;
    }

    private static float ViFromDistance(float distance)
    {
        // Motion equation 3            --> vf^2 = vi^2 + 2a(df - di)
        // Re-arrange to solve for vi   --> vi = sqrt(-2a * d)

        float a = UnityEngine.Physics.gravity.y;
        float vi = Mathf.Sqrt(-2.0f * a * distance);
        return vi;
    }

    private static float ViFromDuration(float duration)
    {
        // Motion equation 1            --> vf = vi + at
        // Re-arrange to solve for vi   --> vi = vf - at

        float vf = 0.0f;                        
        float t = duration * 0.5f;        
        float a = UnityEngine.Physics.gravity.y;
        float vi = vf - a * t;                  
        return vi;
    }

    private ArcY(float distance, float duration, float vi)
    {
        Distance = distance;
        Duration = duration;
        LaunchVelocity = vi;
    }

    public void Log()
    {
        Debug.Log("vi " + LaunchVelocity);
        Debug.Log("d " + Distance);
        Debug.Log("t " + Duration);
    }
}

public class ArcXZ
{
    public float LaunchVelocity
    {
        get; protected set;
    }

    public float Distance
    {
        get; protected set;
    }
}

public class Arc
{
    private ArcY vertical;
    private ArcXZ horizontal;
    private float duration;
}