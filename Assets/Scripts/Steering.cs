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

    private Rigidbody rb;

    public enum SteeringBehaviour
    {
        SEEK_CONSTANT,
        SEEK_LINEAR,
        ARRIVE
    };

    [SerializeField]
    private SteeringBehaviour state;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * linearSpeed);
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 targetDirection = (target.position - transform.position).normalized;

        switch(state)
        {
            case SteeringBehaviour.SEEK_CONSTANT:
                SeekConstantSpeed(targetDirection);
                break;

            case SteeringBehaviour.SEEK_LINEAR:
                SeekLinearSpeed(targetDirection);
                break;
        }

        RotateTowards(targetDirection, dt);
    }

    private void SeekConstantSpeed(Vector3 targetDirection)
    {
        // Applies constant speed by subtracting direction vectors and multiplying the result by linear velocity.
        Vector3 linearVelocityDirection = rb.velocity.normalized;
        rb.AddForce((targetDirection - linearVelocityDirection) * linearSpeed);
    }

    private void SeekLinearSpeed(Vector3 targetDirection)
    {
        // Seek with increasing/decreasing speed by subtracting the constant of target direction *
        // linear velocity by the ever-changing rigid body velocity to create a feedback loop!
        rb.AddForce(targetDirection * linearSpeed - rb.velocity);
    }

    private void RotateTowards(Vector3 targetDirection, float dt)
    {
        // Using AddTorque() to rotate towards a target is hard to control. This suffices.
        Vector3 rotation = Vector3.RotateTowards(transform.forward, targetDirection, dt * angularSpeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(rotation);
    }
}
