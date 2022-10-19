using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body2 : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.forward * 10.0f, ForceMode.Force);
    }
}
