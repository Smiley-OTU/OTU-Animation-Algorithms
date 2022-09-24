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
	//list of segment samples makes it easier to index later
	//imagine it like List<SegmentSamples>, and segment sample is a list of SamplePoints
	List<List<SamplePoint>> table = new List<List<SamplePoint>>();

	float distance = 0.0f;
	float accumDistance = 0.0f;
	int currentIndex = 0;
	int currentSample = 0;

	private void Start()
	{
		//make sure there are 4 points, else disable the component
		if (points.Length < 4)
		{
			enabled = false;
		}

		//calculate the speed graph table
		for (int i = 0; i < points.Length; ++i)
		{
			List<SamplePoint> segment = new List<SamplePoint>();
            Vector3 p0 = points[(i - 1 + points.Length) % points.Length].position;
            Vector3 p1 = points[i].position;
            Vector3 p2 = points[(i + 1) % points.Length].position;
            Vector3 p3 = points[(i + 2) % points.Length].position;

            //calculate samples
			for (int sample = 0; sample < sampleRate; ++sample)
			{
				float t = sample / sampleRate;
				accumDistance += Utility.EvaluateCatmull(p0, p1, p2, p3, t).magnitude;
                segment.Add(new SamplePoint(t, accumDistance));
			}
			table.Add(segment);
		}
	}

	private void Update()
	{
		distance += speed * Time.deltaTime;
		float sampleDistance = table[currentIndex][currentSample].accumulatedDistance;

        // Check if we need to update our samples
        while (distance > table[currentIndex][currentSample].accumulatedDistance)
		{
			if (++currentSample >= sampleRate)
			{
				currentSample = 0;
				++currentIndex;
				currentIndex %= points.Length;
            }
        }

		Vector3 p0 = points[(currentIndex - 1 + points.Length) % points.Length].position;
		Vector3 p1 = points[currentIndex].position;
		Vector3 p2 = points[(currentIndex + 1) % points.Length].position;
		Vector3 p3 = points[(currentIndex + 2) % points.Length].position;
		transform.position = Utility.EvaluateCatmull(p0, p1, p2, p3, GetAdjustedT());
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
	}
}
