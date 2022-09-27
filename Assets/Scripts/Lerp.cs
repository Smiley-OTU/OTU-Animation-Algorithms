using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour
{
    public Transform src;   // start point (source)
    public Transform dst;   // end point (destination)

    [Range(0.0f, 10.0f)]
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.smoothDeltaTime;
        if (time >= 10.0f)
        {
            time = 0.0f;
        }

        float t = time / 10.0f;
        transform.position = Vector3.Lerp(src.position, dst.position, t);
    }
}
