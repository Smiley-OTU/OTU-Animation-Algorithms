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
        viy = Physics.ArcFromDuration(duration);
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
}
