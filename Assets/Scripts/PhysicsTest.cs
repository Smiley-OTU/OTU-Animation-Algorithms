using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsTest : MonoBehaviour
{
    [SerializeField] InputAction jumpAction;

    private Rigidbody body;
    public float height;
    public float duration;

    private void OnEnable()
    {
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        jumpAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.velocity = Vector3.up * SolveArc();
        //Debug.Log(SolveArc());
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpAction.triggered)
        {
            body.AddForce(Vector3.up * 250.0f);
        }
    }

    // Assume the only acceleration is gravitational acceleration
    // As long as we have height and time, we can solve for initial velocity
    float SolveArc()
    {
        // vf = vi + at
        // vf - at = vi
        // 0 - (-9.81t) = vi

        float vf = 0.0f;
        float a = Physics.gravity.y;
        float t = duration;
        float vi = vf - a * t;
        return vi;
    }

    //IEnumerator Arc()
    //{
    //    // d = v * t + 0.5(a * t^2)
    //
    //}
}
