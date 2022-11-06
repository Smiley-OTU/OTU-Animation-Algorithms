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
        switch (state)
        {
            // Don't move directly towards the target because that's boring!
            //case SteeringBehaviour.LINE:
            //    transform.position += Steering.Line(target.position, rb.position, speed * Time.fixedDeltaTime);
            //    break;

            case SteeringBehaviour.SEEK:
                // Negate for infinite flee.
                Steering.ApplySeek(target.position, rb, speed);
                break;

            case SteeringBehaviour.FLEE:
                Steering.ApplyFlee(target.position, rb, speed, proximity);
                break;

            case SteeringBehaviour.PURSUE:
                Steering.ApplySeek(target.position + target.velocity, rb, speed);
                break;
            
            case SteeringBehaviour.EVADE:
                Steering.ApplyFlee(target.position + target.velocity, rb, speed, proximity);
                break;

            case SteeringBehaviour.ARRIVE:
                float t = 1.0f - Steering.Attenuate(target.position, rb.position, proximity);
                Vector3 a = Steering.Seek(target.position, rb, speed);
                Vector3 b = Steering.Arrive(target.position, rb);
                rb.AddForce(Vector3.Lerp(a, b, t));
                break;
        }
    }
}
