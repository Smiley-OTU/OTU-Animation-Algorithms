using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour
{
    public Transform src;   // start point (source)
    public Transform dst;   // end point (destination)

    private const float timeMin = 0.0f;
    private const float timeMax = 10.0f;
    private float time = timeMin;
    private bool right = true;

    // Start is called before the first frame update
    void Start()
    {
        Quaternion rotSrc = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        Quaternion rotDst = Quaternion.Euler(0.0f, 90.0f, 0.0f);

        // rotate 45 degrees by interpolating half way between 0 and 90 degrees
        transform.rotation = Quaternion.Slerp(rotSrc, rotDst, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        float t = right ? time / timeMax : 1.0f - (time / timeMax);
        transform.position = Vector3.Lerp(src.position, dst.position, t);

        time += Time.smoothDeltaTime;
        if (time >= timeMax)
        {
            time = timeMin;
            right = !right;
        }
    }
}
