using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRomSpeedControlled : MonoBehaviour
{
	public Transform[] points;
	public float speed = 1f;
	[Range(1, 32)]
	public int sampleRate = 16;

	[System.Serializable]
	class SamplePoint
	{
		public float samplePosition;
		public float accumulatedDistance;

		public SamplePoint(float samplePosition, float distanceCovered)
		{
			this.samplePosition = samplePosition;
			this.accumulatedDistance = distanceCovered;
		}
	}
	
	// Look-up table (LUT) that maps interpolation values to distances
	List<List<SamplePoint>> table = new List<List<SamplePoint>>();

	float distance = 0.0f;
	float accumDistance = 0.0f;
	int currentIndex = 0;
	int currentSample = 0;

	private void Start()
	{
		// Disable the component if there are less than 4 points
		if (points.Length < 4)
		{
			enabled = false;
		}

		// Create the look-up table
		for (int i = 0; i < points.Length; ++i)
		{
            Vector3 p0, p1, p2, p3;
            Utility.PointsFromIndex(i, points, out p0, out p1, out p2, out p3);
			float previousDistance = Utility.EvaluateCatmull(p0, p1, p2, p3, 0.0f).magnitude;
            List<SamplePoint> segment = new List<SamplePoint>();
			for (int sample = 1; sample <= sampleRate; ++sample)
			{
				float t = (float)sample / (float)sampleRate;
				float distance = Utility.EvaluateCatmull(p0, p1, p2, p3, t).magnitude;
                accumDistance += Mathf.Abs(distance - previousDistance);
                segment.Add(new SamplePoint(t, accumDistance));
				previousDistance = distance;
			}
			table.Add(segment);
		}
	}

	private void Update()
	{
		distance += speed * Time.deltaTime;
		float sampleDistance = table[currentIndex][currentSample].accumulatedDistance;

        // Increment indices until travelled distance matches desired distance
        while (distance > table[currentIndex][currentSample].accumulatedDistance)
		{
			if (++currentSample >= sampleRate)
			{
				currentSample = 0;
				++currentIndex;
				currentIndex %= points.Length;
            }
        }

		transform.position = Utility.EvaluateCatmull(GetAdjustedT(), currentIndex, points);
    }

	float GetAdjustedT()
	{
		SamplePoint current = table[currentIndex][currentSample];
		SamplePoint next = table[currentIndex][currentSample + 1];

		return Mathf.Lerp(current.samplePosition, next.samplePosition,
			(distance - current.accumulatedDistance) / (next.accumulatedDistance - current.accumulatedDistance)
		);
	}

	private void OnDrawGizmos()
	{
		Utility.DrawCatmull(points, Gizmos.DrawLine);
        //Utility.DrawCatmullPoint(0.5f, 0, points);
    }
}
