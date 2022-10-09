using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// See https://en.wikipedia.org/wiki/Projectile_motion

public struct Apex
{
    public float value;
}

public struct Duration
{
    public float value;
}

public class ArcY
{
    public static ArcY From(Apex apex)
    {
        float d = apex.value;
        float vi = ViFromApex(d);
        float t = SolveApexDuration(vi);
        return new ArcY(d, t, vi);
    }

    public static ArcY From(Duration duration)
    {
        float t = duration.value;
        float vi = ViFromDuration(t);
        float d = SolveApex(vi);
        return new ArcY(d, t, vi);
    }

    // Top of arc
    public static float SolveApex(float vi)
    {
        // Motion equation 3
        // vf^2 = vi^2 + 2a(df - di)
        // 0^2 = vi^2 + 2a(df - 0)
        // df = vi^2 / 2a
        return (vi * vi) / (-2.0f * Physics.gravity.y);
    }

    // Time from launch to top of arc (half of total time)
    private static float SolveApexDuration(float vi)
    {
        // Motion equation 2
        // df = di + vi * t + 0.5a * t^2
        // vi * t = 0.5a * t^2
        // t = 2vi / a
        return -2.0f * vi / Physics.gravity.y;
    }

    // Time until arbitrary height
    private static float SolveDuration(float height, float vi)
    {
        // Motion equation 2
        // df = di + vi * t + 0.5a * t^2
        // t = (-vi + sqrt(vi^2 - 4a * df)) / 2a
        return Utility.Quadratic(0.5f * Physics.gravity.y, vi, -height) * 2.0f;
    }

    // Launch velocity given max height
    private static float ViFromApex(float apex)
    {
        // Motion equation 3
        // vf^2 = vi^2 + 2a(df - di)
        // vi = sqrt(-2a * d)

        float a = Physics.gravity.y;
        float vi = Mathf.Sqrt(-2.0f * a * apex);
        return vi;
    }

    // Launch velocity given flight time
    private static float ViFromDuration(float duration)
    {
        // Motion equation 1
        // vf = vi + at
        // vi = vf - at

        float vf = 0.0f;
        float t = duration * 0.5f;
        float a = Physics.gravity.y;
        float vi = vf - a * t;
        return vi;
    }

    public float LaunchVelocity
    {
        get; private set;
    }

    public float Apex
    {
        get; private set;
    }

    public float Duration
    {
        get; private set;
    }

    private ArcY(float apex, float duration, float vi)
    {
        Apex = apex;
        Duration = duration;
        LaunchVelocity = vi;
    }

    public void Log()
    {
        Debug.Log("vi " + LaunchVelocity);
        Debug.Log("d " + Apex);
        Debug.Log("t " + Duration);
    }
}
