using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeArc : MonoBehaviour
{
    public float duration;

    private float time;
    private Rigidbody body;
    private ArcY arc;

    void Start()
    {
        arc = ArcY.From(new Duration { value = duration });
        body = GetComponent<Rigidbody>();
        body.velocity = Vector3.up * arc.LaunchVelocity;

        Debug.Log("Time arc parameters:");
        arc.Log();
    }

    void Update()
    {
        if (time >= arc.Duration)
        {
            time = 0.0f;
            body.velocity = new Vector3(body.velocity.x, arc.LaunchVelocity, body.velocity.z);
        }
        time += Time.deltaTime;
    }
}
