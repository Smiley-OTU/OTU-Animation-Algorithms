using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering
{
    public static Vector3 Line(Vector3 target, Vector3 current, float speed)
    {
        return (target - current).normalized * speed;
    }

    public static Vector3 Seek(Vector3 target, Rigidbody current, float speed)
    {
        Vector3 desiredVelocity = (target - current.position).normalized * speed;
        Vector3 currentVelocity = current.velocity;
        return desiredVelocity - currentVelocity;
    }

    public static Vector3 Arrive(Vector3 target, Rigidbody current, float speed, float proximity)
    {
        float distance = (target - current.position).magnitude;
        return distance <= proximity ?
            -current.velocity.normalized * ArcY.Deceleration(distance, current.velocity.magnitude) :
            Seek(target, current, speed);
    }

    public static void ApplySeek(Vector3 target, Rigidbody current, float speed)
    {
        current.AddForce(Seek(target, current, speed));
    }

    public static void ApplyArrive(Vector3 target, Rigidbody current, float speed, float proximity)
    {
        float attenuation = Attenuate(target, current.position, proximity);
        if (attenuation < 1.0f)
            current.velocity = Seek(target, current, speed) * attenuation;
        else
            current.AddForce(Seek(target, current, speed));
    }

    public static void ApplyFlee(Vector3 target, Rigidbody current, float speed, float proximity)
    {
        float attenuation = Attenuate(target, current.position, proximity);
        if (attenuation < 1.0f)
            current.AddForce(-Seek(target, current, speed));
        else
            current.velocity *= 0.9f;
    }

    // Approaches 0 as current approaches target. Returns 1 if current is length or more units away from target. 
    private static float Attenuate(Vector3 target, Vector3 current, float length)
    {
        return Mathf.Clamp01((target - current).magnitude / length);
    }

    // Can't call within Seek() because of calls to -Seek().
    public static Quaternion RotateAt(Vector3 target, Rigidbody current, float maxAngle = Mathf.Deg2Rad)
    {
        return Quaternion.LookRotation(
            Vector3.RotateTowards(current.transform.forward, (target - current.position).normalized,
            maxAngle, 0.0f)
        );
    }
}
