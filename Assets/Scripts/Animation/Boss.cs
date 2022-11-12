using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, Time.smoothDeltaTime * 50.0f, 0.0f));
    }
}
