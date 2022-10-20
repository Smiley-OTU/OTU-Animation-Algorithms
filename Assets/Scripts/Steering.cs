using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float linearSpeed;
    [SerializeField]
    private float angularSpeed;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * linearSpeed);
    }

    void FixedUpdate()
    {
        Seek();
    }

    private void Seek()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 toTarget = (target.position - transform.position).normalized;

        // TODO -- make this accelerate?
        Vector3 lvDir = rb.velocity.normalized;
        rb.AddForce((toTarget - lvDir) * linearSpeed); 

        // Using AddTorque() to rotate towards a target is hard to control. This suffices.
        Vector3 rotation = Vector3.RotateTowards(transform.forward, toTarget, dt * angularSpeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(rotation);
    }
}