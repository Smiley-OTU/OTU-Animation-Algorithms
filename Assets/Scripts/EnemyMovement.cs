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
        //transform.Rotate(new Vector3(0.0f, Mathf.Cos(Time.realtimeSinceStartup), 0.0f));
        Vector3 front = transform.rotation.eulerAngles;
        Vector3 left = front;
        Vector3 right = front;
        left.y -= fov * 0.5f;
        right.y += fov * 0.5f;

        Quaternion ql = new Quaternion();
        Quaternion qr = new Quaternion();
        ql.eulerAngles = left;
        qr.eulerAngles = right;

        // We want to determine whether the angle between the player and enemy is greater than half the enemy's fov
        // We can solve for this angle using the dot product
        // a . b = ||a|| * ||b|| * cos(x)
        // We can simplify this equation because we know that a and b are normalized, so we can remove their magnitude
        // a . b = ||a|| * ||b|| * cos(x) = a . b = 1 * 1 * cos(x) = a . b = cos(x)
        // Furthermore, we can compare a . b to cos(x) instead of solving for x to reduce complexity.
        // result = a . b <= cos(x / 2) ? seen : unseen

        Vector3 playerDirection = (target.transform.position - transform.position).normalized;
        Vector3 enemyDirection = transform.rotation.normalized * Vector3.forward;
        Color color = Vector3.Dot(playerDirection, enemyDirection) <= Mathf.Cos(Mathf.Deg2Rad * (fov * 0.5f))
            ? Color.green : Color.red;

        // Draw lines green if unseen and red if seen
        Debug.DrawLine(transform.position, transform.position + ql * Vector3.forward * length, color);
        Debug.DrawLine(transform.position, transform.position + qr * Vector3.forward * length, color);
    }
}
