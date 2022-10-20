using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float speed;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Seek();
    }

    private void Seek()
    {
        Vector3 vCurrent = speed * transform.forward;
        Vector3 vDesired = speed * (target.position - transform.position);
        Vector3 vDelta = vDesired - vCurrent;
        rb.AddForce(vDelta, ForceMode.Acceleration);
    }
}
