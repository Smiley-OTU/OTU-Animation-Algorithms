using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    private Racer parent;
    void Start()
    {
        parent = GetComponentInParent<Racer>();
    }

    void OnTriggerEnter(Collider collider)
    {
        parent.OnObstacleDetected(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        parent.OnObstacleAvoided(collider);
    }
}
