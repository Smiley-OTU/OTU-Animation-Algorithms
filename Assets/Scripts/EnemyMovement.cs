using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float fov = 60.0f;
    [SerializeField] float length = 10.0f;

    private float initialRotation;

    void Start()
    {
        initialRotation = transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        Vector3 euler = transform.eulerAngles;
        euler.y = initialRotation + Mathf.Cos(Time.realtimeSinceStartup) * Mathf.Rad2Deg;
        transform.eulerAngles = euler;
        Utility.InRangeDebug(transform, target.transform, length, fov);
    }
}
