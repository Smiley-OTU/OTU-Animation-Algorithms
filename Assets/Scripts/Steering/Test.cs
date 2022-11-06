using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.right * 10.0f;
    }

    void FixedUpdate()
    {
        float distance = (Vector3.zero - transform.position).magnitude;
        Vector3 direction = (Vector3.zero - transform.position).normalized;
        float acc = ArcY.Deceleration(distance, rb.velocity.magnitude);
        rb.AddForce(-rb.velocity.normalized * acc);
    }
}
