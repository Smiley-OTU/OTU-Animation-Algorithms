using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] GameObject target;
    const float fov = 60.0f;
    const float length = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.0f, Mathf.Cos(Time.realtimeSinceStartup), 0.0f));
        Vector3 front = transform.rotation.eulerAngles;
        Vector3 left = front;
        Vector3 right = front;
        left.y -= fov * 0.5f;
        right.y += fov * 0.5f;

        Quaternion ql = new Quaternion();
        Quaternion qr = new Quaternion();
        ql.eulerAngles = left;
        qr.eulerAngles = right;

        Debug.DrawLine(transform.position, transform.position + ql * Vector3.forward * length);
        Debug.DrawLine(transform.position, transform.position + qr * Vector3.forward * length);
        Debug.DrawLine(transform.position, target.transform.position);
    }
}
