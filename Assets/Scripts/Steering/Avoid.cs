using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoid : MonoBehaviour
{
    private Seeker parent;
    void Start()
    {
        parent = GetComponentInParent<Seeker>();
    }

    // TODO -- refactor so steering is a collection of static methods
    // The "AheadCollider" becomes a "sensor" responsible for steering its parent
    void OnTriggerEnter(Collider collider)
    {
        parent.OnObstacleAhead(collider);
    }
}
