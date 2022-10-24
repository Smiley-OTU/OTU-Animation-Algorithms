using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Racer : CatmullRomSpeedControlled
{
    public float proximity = 10.0f;

    private Rigidbody rb;
    private BoxCollider bc;

    private Vector3 avoidPosition = Vector3.zero;

    private enum Follow
    {
        FOLLOW,
        AVOID,
        JOIN
    }

    private Follow state;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        //BoxCollider sensor = GetComponentInChildren<BoxCollider>();
        //Debug.Log(sensor.size.z);
    }

    protected override void Update()
    {
        float dt = Time.deltaTime;
        if (state == Follow.AVOID)
        {
            Vector3 targetDirection = (avoidPosition - transform.position).normalized;
            Steering.Seek(rb, speed, targetDirection);
            Steering.RotateTowards(rb, 1.0f, targetDirection, dt);
            distance += rb.velocity.magnitude * dt;
        }
        else if (state == Follow.JOIN)
        {
            Vector3 targetPosition = points[interval + (1 % intervals)].position;
            Vector3 toTarget = targetPosition - transform.position;
            Vector3 targetDirection = toTarget.normalized;
            float targetDistance = toTarget.magnitude;
            Steering.Arrive(rb, speed, targetDirection, targetDistance, proximity);
            if (targetDistance / proximity <= 1.0f)
                state = Follow.FOLLOW;
        }
        else if (state == Follow.FOLLOW)
            base.Update();
        Debug.Log(state);
    }

    public void OnObstacleDetected(Collider collider)
    {
        Vector3 avoidDirection =
            Vector3.Angle(transform.forward, collider.transform.right) <
            Vector3.Angle(transform.forward, -collider.transform.right) ?
            collider.transform.right : -collider.transform.right;

        avoidPosition = collider.transform.position + avoidDirection * 5.0f;//bc.size.z;

        state = Follow.AVOID;
    }

    public void OnObstacleAvoided(Collider collider)
    {
        state = Follow.JOIN;
    }
}
