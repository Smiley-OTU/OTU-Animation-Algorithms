using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    [SerializeField]
    private Rigidbody target;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float proximity;

    [SerializeField]
    private SteeringBehaviour state;

    public enum SteeringBehaviour
    {
        SEEK,
        FLEE,
        PURSUE,
        EVADE,
        ARRIVE
    };

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // t = behaviour a (0.0) when outside proximity, t = behaviour b when inside proximity.
        float t = 1.0f - Steering.Attenuate(target.position, rb.position, proximity);
        Vector3 steeringForce = Vector3.zero;
        switch (state)
        {
            case SteeringBehaviour.SEEK:
                steeringForce = Steering.Seek(target.position, rb, speed);
                break;

            case SteeringBehaviour.FLEE:
                {
                    Vector3 a = -Steering.Multiply(rb);
                    Vector3 b = -Steering.Seek(target.position, rb, speed);
                    steeringForce = Vector3.Lerp(a, b, t);
                }
                break;

            case SteeringBehaviour.PURSUE:
                steeringForce = Steering.Seek(target.position + target.velocity, rb, speed);
                break;
            
            case SteeringBehaviour.EVADE:
                {
                    Vector3 a = -Steering.Multiply(rb);
                    Vector3 b = -Steering.Seek(target.position + target.velocity, rb, speed);
                    steeringForce = Vector3.Lerp(a, b, t);
                }
                break;

            case SteeringBehaviour.ARRIVE:
                {
                    Vector3 a = Steering.Seek(target.position, rb, speed);
                    Vector3 b = Steering.Arrive(target.position, rb);
                    steeringForce = Vector3.Lerp(a, b, t);
                }
                break;
        }
        rb.AddForce(steeringForce);
    }
}
