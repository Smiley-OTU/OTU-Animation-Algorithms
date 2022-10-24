using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : CatmullRomSpeedControlled
{
    private Rigidbody rb;
    private BoxCollider bc;

    private bool avoid = false;
    private Vector3 avoidPosition = Vector3.zero;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    protected override void Update()
    {
        float dt = Time.deltaTime;
        if (avoid)
        {
            Vector3 targetDirection = (avoidPosition - transform.position).normalized;
            Steering.Seek(rb, speed, targetDirection);
            Steering.RotateTowards(rb, 1.0f, targetDirection, dt);
            distance += rb.velocity.magnitude * dt;
        }
        else
        {
            base.Update();
        }
    }

    public void OnObstacleDetected(Collider collider)
    {
        avoid = true;
        avoidPosition = collider.transform.position +
            (collider.transform.forward * speed) +
            (collider.transform.right * speed);

        // Positive if obstacle is ahead, negative if obstacle is behind
        //Debug.Log(Vector3.Dot(transform.forward, collider.transform.forward));

        // Positive if obstacle is right, negative if obstacle is left
        //Debug.Log(Vector3.Dot(transform.right, collider.transform.right));
    }

    public void OnObstacleAvoided(Collider collider)
    {
        avoid = false;
    }
}
