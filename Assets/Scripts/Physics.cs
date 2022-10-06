using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics
{
    // Initial velocity based on time it takes to jump (duration)
    public static float ArcFromDuration(float duration)
    {
        // Motion equation 1
        // vf = vi + at

        // Re-arrange to solve for vi
        // vi = vf - at

        float vf = 0.0f;                        // We know the velocity will be 0 at the top of the arc
        float t = duration * 0.5f;              // We know we hit the top of the arc half way through the jump
        float a = UnityEngine.Physics.gravity.y;// We know acceleration is the gravitational constant
        float vi = vf - a * t;                  // Hence, we have all we need to solve!
        return vi;
    }

    // Initial velocity based on apex of jump (height)
    public static float ArcFromDistance(float height)
    {
        // Motion equation 3
        // vf^2 = vi^2 + 2a(df - di)
        // vf^2 - vi^2 = 2a(df - di)
        // - vi^2 = 2a(df - di) - vf^2
        // vi^2 = -2a(df - di) + vf^2
        // vi = sqrt(-2a(df - di) + vf^2)

        // We know initial distance is 0 and final distance is height half way through the jump
        // We also know the velocity will be 0 half way through the arc, so we can simplify:
        // vi = sqrt(-2a * height)

        float a = UnityEngine.Physics.gravity.y;
        float vi = Mathf.Sqrt(-2.0f * a * height);
        return vi;
    }

    // Time of jump from height and initial velocity
    public static float ArcDuration(float height, float vi)
    {
        // Motion equation 1
        // vf = vi + a * t

        // Re-arrange to solve for t
        // (vf - vi) / a = t

        // We know velocity is 0 at the top of the arc, so we have everything we need to solve!
        float a = UnityEngine.Physics.gravity.y;
        float t = -vi / a;

        // Multiply by two because we solved for time till top of arc (which is half the total time)
        return t * 2.0f;
    }
}
