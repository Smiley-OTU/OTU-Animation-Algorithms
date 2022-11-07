using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Racer : CatmullRomSpeedControlled
{
    public float proximity = 10.0f;

    private Rigidbody rb;

    private Vector3 avoidPosition = Vector3.zero;

    private enum State
    {
        FOLLOW,
        AVOID,
        JOIN
    }

    private State state;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        // If avoiding, the target is the obstacle. Otherwise the target is the next point on the curve.
        Vector3 target = state == State.AVOID
            ? avoidPosition : points[(interval + 1) % points.Length].position;

        float dt = Time.deltaTime;
        if (state != State.FOLLOW)
        {
            rb.AddForce(Steering.Seek(target, rb, speed));
            transform.rotation = Steering.RotateAt(target, rb, dt * 2.5f);
            distance += rb.velocity.magnitude * dt;

            // If close to goal point, transition from avoid to join, or join to follow
            if ((target - transform.position).magnitude / proximity <= 0.1f)
            {
                state = state == State.AVOID ? State.JOIN : State.FOLLOW;
                if (state == State.JOIN) avoidPosition = Vector3.zero;
            }
        }
        else
            base.Update();
    }

    public void OnObstacleDetected(Collider collider)
    {
        Vector3 forward = (collider.transform.position - transform.position).normalized;
        Vector3 right = Vector3.Cross(forward, Vector3.up);

        Vector3 avoidDirection = Vector3.Angle(forward, right) < Vector3.Angle(forward, -right) ? right : -right;
        avoidPosition = collider.transform.position + avoidDirection * 5.0f;

        state = State.AVOID;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (avoidPosition != Vector3.zero)
            Gizmos.DrawSphere(avoidPosition, 1.0f);
    }
}
