using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Racer : CatmullRomSpeedControlled
{
    public float proximity = 10.0f;

    private Rigidbody rb;

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
    }

    protected override void Update()
    {
        float dt = Time.deltaTime;

        if (state != Follow.FOLLOW)
        {
            Vector3 target = state == Follow.AVOID ? avoidPosition : points[(interval + 1) % intervals].position;
            Vector3 toTarget = target - transform.position;
            Steering.Arrive(rb, speed, toTarget.normalized, toTarget.magnitude, proximity);
            //Steering.RotateTowards(rb, 1.0f, toTarget.normalized, dt);
            distance += rb.velocity.magnitude * dt;
            if (toTarget.magnitude / proximity <= 0.25f)
                state = state == Follow.AVOID ? Follow.JOIN : Follow.FOLLOW;
        }
        else
            base.Update();
    }

    public void OnObstacleDetected(Collider collider)
    {
        float right = Vector3.Angle(transform.forward, collider.transform.right);
        float left = Vector3.Angle(transform.forward, -collider.transform.right);

        Vector3 avoidDirection = right < left ? collider.transform.right : -collider.transform.right;
        avoidPosition = collider.transform.position + avoidDirection * 5.0f;

        state = Follow.AVOID;
    }
}
