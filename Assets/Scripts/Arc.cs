using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static float SolveApex(float vi)
    {
        // Motion equation 3
        // vf^2 = vi^2 + 2a(df - di)
        // 0^2 = vi^2 + 2a(df - 0)
        // df = vi^2 / 2a
        return (vi * vi) / (-2.0f * Physics.gravity.y);
    }

    // Time until top of arc
    private static float SolveApexDuration(float vi)
    {
        // Motion equation 2
        // df = di + vi * t + 0.5a * t^2
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

    private static float ViFromApex(float apex)
    {
        // Motion equation 3            --> vf^2 = vi^2 + 2a(df - di)
        // Re-arrange to solve for vi   --> vi = sqrt(-2a * d)

        float a = Physics.gravity.y;
        float vi = Mathf.Sqrt(-2.0f * a * apex);
        return vi;
    }

    private static float ViFromDuration(float duration)
    {
        // Motion equation 1            --> vf = vi + at
        // Re-arrange to solve for vi   --> vi = vf - at

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

// Hitting a target is a bit different.
// We need to specify a launch angle (ie if 45 then y = sin(45), x = cos(45)

public class Arc
{
    public Arc(Vector3 position, Vector3 target, float pitch, float duration, float projectileMass = 1.0f)
    {
        Vector3 direction = target - position;

        // Horizontal velocity depends on the launch angle (pitch)
        // Higher angle = longer flight = lower horizontal velocity
        float distanceXZ = new Vector2(direction.x, direction.z).magnitude;
        float velocityXZ = distanceXZ * Mathf.Cos(Mathf.Deg2Rad * pitch) / duration;

        // Perhaps we can solve the vertical by determining how long it takes to go from 0 to target?

    }
}