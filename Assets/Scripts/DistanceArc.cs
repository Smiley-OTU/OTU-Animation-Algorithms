using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DistanceArc : MonoBehaviour
{
    public float height;

    private float duration;
    private float time;
    private float viy;
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        viy = ArcFromDistance(height);
        duration = ArcDuration(height, viy);
        body = GetComponent<Rigidbody>();
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

    // Initial velocity based on apex of jump (height)
    float ArcFromDistance(float height)
    {
        // Motion equation 3
        // vf^2 = vi^2 + 2a(df - di)
        // vf^2 - vi^2 = 2a(df - di)
        // - vi^2 = 2a(df - di) - vf^2
        // vi^2 = -2a(df - di) + vf^2
        // vi = sqrt(-2a(df - di) + vf^2)

        // We know initial distance is 0 and final distance is height half way through the jump
        // We also know the velocity will be 0 half way through the arc, so we can simplify:
        // vi = sqrt(-2a * height)

        float a = Physics.gravity.y;
        float vi = Mathf.Sqrt(-2.0f * a * height);
        return vi;
    }

    float ArcDuration(float height, float vi)
    {
        // Motion equation 1
        // vf = vi + a * t

        // Re-arrange to solve for t
        // (vf - vi) / a = t

        // We know velocity is 0 at the top of the arc, so we have everything we need to solve!

        float a = Physics.gravity.y;
        float t = -vi / a;

        // Multiply by two because we solved for time till top of arc (which is half the total time)
        return t * 2.0f;
    }
}
