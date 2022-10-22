using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering
{
    public static void Line(Rigidbody rb, float linearSpeed, Vector3 targetDirection)
    {
        // Applies constant speed by subtracting direction vectors and multiplying the result by linear velocity.
        Vector3 linearVelocityDirection = rb.velocity.normalized;
        rb.AddForce((targetDirection - linearVelocityDirection) * linearSpeed);
    }

    public static void Seek(Rigidbody rb, float linearSpeed, Vector3 targetDirection)
    {
        // Seek with increasing/decreasing speed by subtracting the constant of target direction *
        // linear velocity by the ever-changing rigid body velocity to create a feedback loop!
        rb.AddForce(targetDirection * linearSpeed - rb.velocity);
    }

    public static void Arrive(Rigidbody rb, float linearSpeed, Vector3 targetDirection, float targetDistance, float slowRadius)
    {
        // Attenuate velocity based on proximity;
        // (targetDistance / slowRadius) approaches zero as the object approaches the target.
        if (targetDistance <= slowRadius)
        {
            rb.velocity = (targetDirection * linearSpeed - rb.velocity) * (targetDistance / slowRadius);
        }
        else
        {
            Seek(rb, linearSpeed, targetDirection);
        }
    }

    public static void ProximityFlee(Rigidbody rb, float linearSpeed, Vector3 targetDirection, float targetDistance, float fleeRadius)
    {
        // Similar to arrive but with a lazier attenuation method.
        // (Must pass more information if we want to attenuate more gradually).
        if (targetDistance <= fleeRadius)
        {
            Seek(rb, linearSpeed , - targetDirection);
        }
        else
        {
            rb.velocity *= 0.90f;
        }
    }

    public static void RotateTowards(Rigidbody rb, float angularSpeed, Vector3 targetDirection, float dt)
    {
        // Using AddTorque() to rotate towards a target is hard to control. This suffices.
        Vector3 rotation = Vector3.RotateTowards(rb.transform.forward, targetDirection, dt * angularSpeed, 0.0f);
        rb.transform.rotation = Quaternion.LookRotation(rotation);
    }
}
