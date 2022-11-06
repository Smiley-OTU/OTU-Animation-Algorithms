using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 10.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 velocity = new Vector3(
            Mathf.Cos(Time.realtimeSinceStartup),
            0.0f,
            Mathf.Sin(Time.realtimeSinceStartup)
        );
        rb.velocity = velocity * speed;
    }
}
