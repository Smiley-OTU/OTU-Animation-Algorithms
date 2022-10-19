using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematic : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 acceleration;

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        velocity += acceleration * dt;
        transform.position += velocity * dt;
    }
}
