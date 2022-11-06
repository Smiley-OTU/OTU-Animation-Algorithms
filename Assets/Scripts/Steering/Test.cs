using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Rigidbody rb;
    float a;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.right * 10.0f;
        a = ArcY.Deceleration(10, rb.velocity.x);
    }

    void FixedUpdate()
    {
        rb.AddForce(-Vector3.right * a);
    }
}
