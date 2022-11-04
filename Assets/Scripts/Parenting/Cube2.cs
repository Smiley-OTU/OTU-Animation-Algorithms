using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2 : MonoBehaviour
{
    void Start()
    {
        transform.rotation = Quaternion.Euler(45.0f, 45.0f, 45.0f);
        Vector3 forward = transform.forward;
        Quaternion rotation = Quaternion.LookRotation(forward);
        transform.rotation = rotation;
    }
}
