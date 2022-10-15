using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public Vector3 v;
    public Vector3 a;

    void Start()
    {
    }

    void Update()
    {
        float dt = Time.deltaTime;
        v += a * dt;
        transform.position += v * dt;
    }
}
