using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DistanceArcTest : MonoBehaviour
{
    public float height;

    private float time;
    private Rigidbody body;
    private ArcY arc;

    void Start()
    {
        arc = ArcY.From(new Distance { value = height });
        body = GetComponent<Rigidbody>();
        body.velocity = Vector3.up * arc.LaunchVelocity;
        //arc.Log();
    }

    void Update()
    {
        if (time >= arc.Duration)
        {
            time = 0.0f;
            body.velocity = Vector3.up * arc.LaunchVelocity;
        }
        time += Time.deltaTime;
    }
}
