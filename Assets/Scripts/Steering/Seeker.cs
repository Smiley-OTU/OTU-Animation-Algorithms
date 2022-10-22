using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
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
        NONE,
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
        //rb.AddForce(transform.forward * linearSpeed);
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 toTarget = target.position - transform.position;
        Vector3 targetDirection = toTarget.normalized;
        float targetDistance = toTarget.magnitude;

        switch (state)
        {
            case SteeringBehaviour.LINE:
                Steering.Line(rb, linearSpeed, targetDirection);
                break;

            case SteeringBehaviour.SEEK:
                Steering.Seek(rb, linearSpeed, targetDirection);
                break;

            case SteeringBehaviour.FLEE:
                Steering.Seek(rb, linearSpeed, -targetDirection);
                break;

            case SteeringBehaviour.PROXIMITY_FLEE:
                Steering.ProximityFlee(rb, linearSpeed, targetDirection, targetDistance, proximity);
                break;

            case SteeringBehaviour.ARRIVE:
                Steering.Arrive(rb, linearSpeed, targetDirection, targetDistance, proximity);
                break;
        }

        if (state != SteeringBehaviour.NONE)
            Steering.RotateTowards(rb, angularSpeed, targetDirection, dt);
    }

    public void OnObstacleAhead(Collider collider)
    {
        Debug.Log(collider.name);
    }
}
