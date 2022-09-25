using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRomSpeedControlled : MonoBehaviour
{
	public Transform[] points;
	public float speed = 1.0f;

	[Range(1, 32)]
	public int intervals = 16;
	private int samples;

	[System.Serializable]
	class SamplePoint
	{
		public float t;
		public float accumulatedDistance;

		public SamplePoint(float t, float distance)
		{
			this.t = t;
			this.accumulatedDistance = distance;
		}
	}
	
	// Look-up table (LUT) that maps interpolation values to distances
	List<List<SamplePoint>> table = new List<List<SamplePoint>>();

	float distance = 0.0f;
	int currentIndex = 0;
	int currentSample = 0;

	private void Start()
	{
        // Disable the component if there are less than 4 points
        if (points.Length < 4)
		{
			enabled = false;
		}

		// n intervals = n + 1 points
		samples = intervals + 1;

		// Create the look-up table
		for (int i = 0; i < points.Length; ++i)
		{
            Vector3 p0, p1, p2, p3;
            Utility.PointsFromIndex(i, points, out p0, out p1, out p2, out p3);
            List<SamplePoint> segment = new List<SamplePoint>();

			float arcLength = 0.0f;
			float step = 1.0f / intervals;
			segment.Add(new SamplePoint(0.0f, 0.0f));
			for (float t = step; t <= 1.0f; t+=step)
			{
				Vector3 a = Utility.EvaluateCatmull(t - step, i, points);
				Vector3 b = Utility.EvaluateCatmull(t, i, points);
				Vector3 line = b - a;
				arcLength += line.magnitude;
				segment.Add(new SamplePoint(t, arcLength));
            }
			table.Add(segment);
		}
    }

	private void Update()
	{
		distance += speed * Time.deltaTime;

        // Increment indices until travelled distance matches desired distance
        while (distance > table[currentIndex][(currentSample + 1) % samples].accumulatedDistance)
		{
            if (++currentSample >= samples)
			{
				currentSample = 0;
                distance = 0.0f;

				++currentIndex;
				currentIndex %= points.Length;
            }
        }

        transform.position = Utility.EvaluateCatmull(GetAdjustedT(), currentIndex, points);
    }

	float GetAdjustedT()
	{
		SamplePoint current = table[currentIndex][currentSample];
		SamplePoint next = table[currentIndex][(currentSample + 1) % samples];

		return Mathf.Lerp(current.t, next.t,
			(distance - current.accumulatedDistance) /
			(next.accumulatedDistance - current.accumulatedDistance)
		);
	}

    private void OnDrawGizmos()
	{
		Utility.DrawCatmull(points, Gizmos.DrawLine);
        for (int i = 0; i < points.Length; ++i)
		{
            for (float t = 0.0f; t < 1.0f; t += 1.0f / intervals)
            {
                Utility.DrawCatmullPoint(t, i, points);
            }
        }
    }
}
