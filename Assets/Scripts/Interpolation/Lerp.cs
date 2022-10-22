using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour
{
    public Transform src;   // start point (source)
    public Transform dst;   // end point (destination)

    private const float timeMin = 0.0f;
    private const float timeMax = 10.0f;
    private float time = timeMin;
    private bool right = true;

    void Update()
    {
        float t = right ? time / timeMax : 1.0f - (time / timeMax);
		Quaternion rotSrc = Quaternion.Euler(0.0f, 0.0f, 0.0f);
		Quaternion rotDst = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
		transform.rotation = Quaternion.Slerp(rotSrc, rotDst, t);
		transform.position = Vector3.Lerp(src.position, dst.position, t);
        GetComponent<Renderer>().material.color =
            Color.Lerp(Color.green, Color.red, t);

        time += Time.smoothDeltaTime;
        if (time >= timeMax)
        {
            time = timeMin;
            right = !right;
        }
    }

	void Start()
	{
        //SolveQuestions();
	}

	void SolveQuestions()
    {
        {   // Question 1
            float t = Interpolation.SolveT(4.0f, 7.0f, 2.0f);     // 0.6
            float x = Mathf.Lerp(-7.0f, -1.0f, t);  // -3.4
            float y = Mathf.Lerp(7.0f, 2.0f, t);    // 4
        }

        {   // Question 2
            float t = Interpolation.SolveT(5.0f, 4.0f, 7.0f);     // 0.33
            float x = Mathf.Lerp(4.0f, 7.0f, t);    // 5
            float y = Mathf.Lerp(3.0f, -7.0f, t);   // -0.33
        }

        {   // Question 3
            float t = Interpolation.SolveT(-6.5f, -7.0f, -1.0f);  // 0.083
            float x = Mathf.Lerp(-7.0f, -1.0f, t);  // -6.5
            float y = Mathf.Lerp(7.0f, 2.0f, t);    // 6.583
        }

        {   // Question 4
            float t = 62.0f / 100.0f;               // 0.62
            float x = Mathf.Lerp(1.0f, -6.0f, t);   // -3.34
            float y = Mathf.Lerp(-6.0f, -3.0f, t);  // -4.14
        }

        {   // Question 5 (distance along curve = 0.6)
            float t = Interpolation.SolveT(0.6f, 0.574f, 0.739f);
            float x = Mathf.Lerp(0.275f, 0.363f, t);
            float y = Mathf.Lerp(0.675f, 0.535f, t);
        }

        {   // Question 6
            Vector3 solution = Interpolation.Decasteljau
            (
                new Vector3(-6.0f, -3.0f, 0.0f),
                new Vector3(-1.0f, 2.0f, 0.0f),
                new Vector3(4.0f, 3.0f, 0.0f),
                new Vector3(1.0f, -6.0f, 0.0f),
                0.75f
            );
        }
    }
}
