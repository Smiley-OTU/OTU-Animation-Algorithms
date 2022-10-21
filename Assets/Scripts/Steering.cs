using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Steering : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float linearSpeed;

    [SerializeField]
    private float angularSpeed;

    [SerializeField]
    private float proximity;

    [SerializeField]
    private SteeringBehaviour state;

    public enum SteeringBehaviour
    {
        LINE,
        SEEK,
        FLEE,
        PROXIMITY_FLEE,
        ARRIVE
    };

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * linearSpeed);
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 toTarget = target.position - transform.position;
        Vector3 targetDirection = toTarget.normalized;
        float targetDistance = toTarget.magnitude;

        switch(state)
        {
            case SteeringBehaviour.LINE:
                Line(targetDirection);
                break;

            case SteeringBehaviour.SEEK:
                Seek(targetDirection);
                break;

            case SteeringBehaviour.FLEE:
                Seek(-targetDirection);
                break;

            case SteeringBehaviour.PROXIMITY_FLEE:
                ProximityFlee(targetDirection, targetDistance, proximity);
                break;

            case SteeringBehaviour.ARRIVE:
                Arrive(targetDirection, targetDistance, proximity);
                break;
        }

        RotateTowards(targetDirection, dt);
    }

    private void Line(Vector3 targetDirection)
    {
        // Applies constant speed by subtracting direction vectors and multiplying the result by linear velocity.
        Vector3 linearVelocityDirection = rb.velocity.normalized;
        rb.AddForce((targetDirection - linearVelocityDirection) * linearSpeed);
    }

    private void Seek(Vector3 targetDirection)
    {
        // Seek with increasing/decreasing speed by subtracting the constant of target direction *
        // linear velocity by the ever-changing rigid body velocity to create a feedback loop!
        rb.AddForce(targetDirection * linearSpeed - rb.velocity);
    }

    private void Arrive(Vector3 targetDirection, float targetDistance, float slowRadius)
    {
        // Attenuate velocity based on proximity;
        // (targetDistance / slowRadius) approaches zero as the object approaches the target.
        if (targetDistance <= slowRadius)
        {
            rb.velocity = (targetDirection * linearSpeed - rb.velocity) * (targetDistance / slowRadius);
        }
        else
        {
            Seek(targetDirection);
        }
    }

    private void ProximityFlee(Vector3 targetDirection, float targetDistance, float fleeRadius)
    {
        // Similar to arrive but with a lazier attenuation method.
        // (Must pass more information if we want to attenuate more gradually).
        if (targetDistance <= fleeRadius)
        {
            Seek(-targetDirection);
        }
        else
        {
            rb.velocity *= 0.90f;
        }
    }

    private void RotateTowards(Vector3 targetDirection, float dt)
    {
        // Using AddTorque() to rotate towards a target is hard to control. This suffices.
        Vector3 rotation = Vector3.RotateTowards(transform.forward, targetDirection, dt * angularSpeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(rotation);
    }
}
