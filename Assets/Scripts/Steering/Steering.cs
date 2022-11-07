using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering
{
    public static Vector3 Seek(Vector3 target, Rigidbody current, float speed)
    {
        Vector3 desiredVelocity = (target - current.position).normalized * speed;
        Vector3 currentVelocity = current.velocity;
        return desiredVelocity - currentVelocity;
    }

    public static Vector3 Arrive(Vector3 target, Rigidbody current)
    {
        float distance = (target - current.position).magnitude;
        return -current.velocity.normalized * ArcY.Deceleration(distance, current.velocity.magnitude);
    }

    // Approaches 0 as current approaches target. Returns 1 if current is length or more units away from target. 
    public static float Attenuate(Vector3 target, Vector3 current, float length)
    {
        return Mathf.Clamp01((target - current).magnitude / length);
    }
    
    // A poor man's AddTorque().
    public static Quaternion RotateAt(Vector3 target, Rigidbody current, float maxAngle = Mathf.Deg2Rad)
    {
        return Quaternion.LookRotation(
            Vector3.RotateTowards(current.transform.forward, (target - current.position).normalized,
            maxAngle, 0.0f)
        );
    }
}

// Don't move directly towards the target because that's boring!
//case SteeringBehaviour.LINE:
//    transform.position += Steering.Line(target.position, rb.position, speed * Time.fixedDeltaTime);
//    break;
//
//public static Vector3 Line(Vector3 target, Vector3 current, float speed)
//{
//    return (target - current).normalized * speed;
//}
