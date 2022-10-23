using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : CatmullRomSpeedControlled
{
    protected override void Update()
    {
        base.Update();
    }

    public void OnObstacleDetected(Collider collider)
    {
        Debug.Log(collider.name);
    }
}
