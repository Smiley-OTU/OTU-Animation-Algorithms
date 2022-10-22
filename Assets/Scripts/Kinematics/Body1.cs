using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body1 : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private float mass = 1.0f;

    private void FixedUpdate()
    {
        AddForce(Vector3.forward * 10.0f, ForceMode.Force);
        transform.position += velocity * Time.fixedDeltaTime;
    }

    public void AddForce(Vector3 force, ForceMode mode)
    {
        float dt = Time.fixedDeltaTime;
        switch (mode)
        {
            case ForceMode.Force:
                velocity += force * dt / mass;
                break;

            case ForceMode.Acceleration:
                velocity += force * dt;
                break;

            case ForceMode.Impulse:
                velocity += force / mass;
                break;

            case ForceMode.VelocityChange:
                velocity += force;
                break;
        }
    }
}
