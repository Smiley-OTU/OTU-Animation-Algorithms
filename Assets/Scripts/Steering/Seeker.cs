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
        NONE,
        LINE,
        SEEK,
        FLEE,
        PROXIMITY_FLEE,
        ARRIVE,
        PURSUE,
        EVADE
    };

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 toTarget = target.position - transform.position;
        Vector3 targetDirection = toTarget.normalized;
        float targetDistance = toTarget.magnitude;

        switch (state)
        {
            case SteeringBehaviour.LINE:
                Steering.Line(rb, speed, targetDirection);
                break;

            case SteeringBehaviour.SEEK:
                Steering.Seek(rb, speed, targetDirection);
                break;

            case SteeringBehaviour.FLEE:
                Steering.Seek(rb, speed, -targetDirection);
                break;

            case SteeringBehaviour.PROXIMITY_FLEE:
                Steering.ProximityFlee(rb, speed, targetDirection, targetDistance, proximity);
                break;

            case SteeringBehaviour.ARRIVE:
                Steering.Arrive(rb, speed, targetDirection, targetDistance, proximity);
                break;

            case SteeringBehaviour.PURSUE:
                Steering.Seek(rb, speed,
                    (target.position + target.velocity - transform.position).normalized);
                break;

            case SteeringBehaviour.EVADE:
                Steering.ProximityFlee(rb, speed,
                    (target.position + target.velocity - transform.position).normalized, targetDistance, proximity);
                break;
        }

        if (state != SteeringBehaviour.NONE)
            Steering.RotateTowards(rb, 1.0f, targetDirection);
    }

    public void OnObstacleAhead(Collider collider)
    {
        Debug.Log(collider.name);
    }
}
