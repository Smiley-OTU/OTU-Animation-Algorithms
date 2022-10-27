using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public static class Utility
{
    public static float Quadratic(float a, float b, float c)
    {
        float twoA = (2.0f * a);
        float b2 = (b * b);
        float fourAC = (4.0f * a * c);
        return (-b + Mathf.Sqrt(b2 - fourAC)) / twoA;
    }
}
