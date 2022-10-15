using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    //public Vector3 v;
    //public Vector3 a;

    void Start()
    {
    }

    void Update()
    {
        // w = v / r


        float dt = Time.deltaTime;

        //a = transform.right * 10.0f * dt;
        //v += a * dt;
        transform.Rotate(new Vector3(0.0f, 100.0f * dt, 0.0f));
        transform.position += transform.forward * 10.0f * dt;

        //transform.Rotate(new Vector3(0.0f, w * dt, 0.0f));
        //transform.position += transform.forward * v * dt;
    }

    // These are more useful in arcs where things are constant up-front.
    // v += a * dt, p += v * dt is generally the best
    /*
    // Only used for horizontal position (due to no acceleration)
    public static Vector3 NewGroundPosition(Vector3 pos, Vector3 vi, Vector3 vf, float dt)
    {
        return pos + 0.5f * (vf - vi) * dt;
    }

    // Only used for vertical position (due to gravity)
    public static Vector3 NewAirPosition(Vector3 pos, Vector3 vel, Vector3 acc, float dt)
    {
        return pos + vel * dt + 0.5f * acc * dt * dt;
    }
    */
}
