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

	// Look-up table (LUT) that maps interpolation values to distances
	List<List<Interpolation.SamplePoint>> table = new List<List<Interpolation.SamplePoint>>();

	float distance = 0.0f;
	int intervalIndex = 0;
	int sampleIndex = 0;

	private void Start()
	{
		// Disable the component if there are less than 4 points
		if (points.Length < 4)
		{
			enabled = false;
		}

		// n intervals = n + 1 points
		samples = intervals + 1;
		table = Interpolation.CreateLookupTable(points, samples);

		DistanceUpdater = () => { return speed * Time.deltaTime; };
	}

	public delegate float DistanceFunction();
	public DistanceFunction DistanceUpdater
	{
		set; private get;
	}

    private void Update()
	{
		// Make this a delegate?
		// distance += rb.velocity (based on avoid or follow spline)
		//distance += speed * Time.deltaTime;
		distance += DistanceUpdater();

        // Increment indices until travelled distance matches desired distance
        while (distance > table[intervalIndex][(sampleIndex + 1) % samples].accumulatedDistance)
		{
            if (++sampleIndex >= samples)
			{
				sampleIndex = 0;
                distance = 0.0f;

				++intervalIndex;
				intervalIndex %= points.Length;
            }
        }

        transform.position = Interpolation.EvaluateCatmull(
            Interpolation.DistanceSample(distance, table, intervalIndex, sampleIndex, samples),
			intervalIndex, points);
    }

    private void OnDrawGizmos()
	{
        Interpolation.DrawCatmull(points, Gizmos.DrawLine);
        Interpolation.DrawCatmullPoints(points, intervals, Gizmos.DrawLine);
    }
}
