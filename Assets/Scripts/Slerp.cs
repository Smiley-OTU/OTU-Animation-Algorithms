using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slerp : MonoBehaviour
{
    private Quaternion src;
    private Quaternion dst;
    public bool correct = false;

    // Start is called before the first frame update
    void Start()
    {
        src = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        dst = Quaternion.Euler(0.0f, 0.0f, 90.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.Cos(Time.realtimeSinceStartup) * 0.5f + 0.5f;
        Quaternion orientation = correct ? Quaternion.Slerp(src, dst, t) : Quaternion.Lerp(src, dst, t);
        transform.rotation = orientation;
    }
}
