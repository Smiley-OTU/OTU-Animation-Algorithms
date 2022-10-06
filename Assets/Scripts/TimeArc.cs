using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeArc : MonoBehaviour
{
    public float duration;

    private float time;
    private float viy;
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        viy = ArcFromDuration(duration);
        body.velocity = Vector3.up * viy;
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= duration)
        {
            time = 0.0f;
            body.velocity = Vector3.up * viy;
        }
        time += Time.deltaTime;
    }

    // Initial velocity based on time it takes to jump (duration)
    float ArcFromDuration(float duration)
    {
        // Motion equation 1
        // vf = vi + at

        // Re-arrange to solve for vi
        // vi = vf - at

        float vf = 0.0f;            // We know the velocity will be 0 at the top of the arc
        float t = duration * 0.5f;  // We know we hit the top of the arc half way through the jump
        float a = Physics.gravity.y;// We know acceleration is the gravitational constant
        float vi = vf - a * t;      // Hence, we have all we need to solve!
        return vi;
    }
}
