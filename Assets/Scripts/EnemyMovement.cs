using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] const float fov = 60.0f;
    [SerializeField] const float length = 10.0f;
    [SerializeField] const float rotationAmount = 90.0f;
    [SerializeField] const float rotationDuration = 3.0f;

    private float rotSrc, rotDst;
    private float elapsedTime = 0.0f;
    private bool right = true;

    void Start()
    {
        rotSrc = transform.rotation.eulerAngles.y;
        rotDst = rotSrc + rotationAmount;
    }

    void Update()
    {
        float y = right ? Mathf.Lerp(rotSrc, rotDst, elapsedTime / rotationDuration) :
            Mathf.Lerp(rotDst, rotSrc, elapsedTime / rotationDuration);
        elapsedTime += Time.smoothDeltaTime;
        if (elapsedTime > rotationDuration)
        {
            elapsedTime = 0.0f;
            right = !right;
        }

        Vector3 euler = transform.eulerAngles;
        euler.y = y;
        //euler.y = rotSrc + Mathf.Cos(Time.realtimeSinceStartup) * Mathf.Rad2Deg;
        transform.eulerAngles = euler;
        Utility.InRangeDebug(transform, target.transform, length, fov);
    }
}
