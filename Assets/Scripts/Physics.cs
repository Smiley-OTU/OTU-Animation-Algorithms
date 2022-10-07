using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

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

    public static float ArcDuration(float height, float vi)
    {
        // df = di + vi * t + 0.5(a * t * t)
        return Quadratic(0.5f * UnityEngine.Physics.gravity.y, vi, height);
    }

    private static float Quadratic(float a, float b, float c)
	{
		//t = (-b - sqrt(b^2 - 4ac))/2a

		// a is the y acceleration
		// b is initial velocity in y direction
		// c is the initial height above ground
		// t is the arc duration (return value)

		float twoA = (2.0f * a);
        float b2 = (b * b);
        float fourAC = (4.0f * a * c);

		return (-b - Mathf.Sqrt(b2 - fourAC)) / twoA;
	}
}
