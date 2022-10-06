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

    void Start()
    {
        viy = Physics.ArcFromDistance(height);
        duration = Physics.ArcDuration(height, viy);
        body = GetComponent<Rigidbody>();
        body.velocity = Vector3.up * viy;
    }

    void Update()
    {
        if (time >= duration)
        {
            time = 0.0f;
            body.velocity = Vector3.up * viy;
        }
        time += Time.deltaTime;
    }

    
    
}
