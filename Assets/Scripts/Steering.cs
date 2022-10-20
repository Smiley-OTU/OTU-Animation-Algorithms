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

        //transform.position = Vector3.MoveTowards(transform.position, target.position, dt * linearSpeed);
        //transform.rotation = Quaternion.FromToRotation(transform.forward, toTarget);
        transform.position = Vector3.MoveTowards(transform.position, target.position, dt * linearSpeed);
        Vector3 rotation = Vector3.RotateTowards(transform.forward, toTarget, dt * angularSpeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(rotation);
    }
    /*
    Vector3 lvCurrent = linearSpeed * transform.forward;
    Vector3 lvDesired = linearSpeed * toTarget;
    Vector3 lvDelta = lvDesired - lvCurrent;
    */

    /*
    Vector3 lvDir = rb.velocity.normalized;
    Vector3 avDir = rb.angularVelocity.normalized;
    rb.AddForce((toTarget - lvDir) * linearSpeed, ForceMode.Acceleration);
    rb.AddTorque((toTarget - avDir) * angularSpeed, ForceMode.Acceleration); 
    */
}