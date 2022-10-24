using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Strictly spline-following at a constant speed.
// More elaborate update function needed for racing!
public class CatmullRomSpeedControlled : MonoBehaviour
{
	public Transform[] points;
	public float speed = 10.0f;
	protected float distance = 0.0f;

	[Range(1, 32)]
	public int intervals = 16;
	private int samples;
	private int interval = 0;
	private int sample = 0;

	// Look-up table (LUT) that maps interpolation values to distances
	private List<List<Interpolation.SamplePoint>> table = new List<List<Interpolation.SamplePoint>>();

    virtual protected void Start()
	{
		// Disable the component if there are less than 4 points
		if (points.Length < 4)
		{
			enabled = false;
		}

		// n intervals = n + 1 points
		samples = intervals + 1;
		table = Interpolation.CreateLookupTable(points, samples);
	}

    virtual protected void Update()
	{
        Vector3 p0 = Interpolation.EvaluateCatmull(
            Interpolation.DistanceSample(distance, table, interval, sample, samples),
            interval, points);

		distance += speed * Time.deltaTime;

        // Increment indices until travelled distance matches desired distance
        while (distance > table[interval][(sample + 1) % samples].accumulatedDistance)
		{
            if (++sample >= samples)
			{
                sample = 0;
                distance = 0.0f;

				++interval;
                interval %= points.Length;
            }
        }

        Vector3 p1 = Interpolation.EvaluateCatmull(
            Interpolation.DistanceSample(distance, table, interval, sample, samples),
            interval, points);

		transform.rotation = Interpolation.FrenetFrame(p0, p1, Vector3.up).rotation;
		transform.position = p1;
    }

    private void OnDrawGizmos()
	{
        Interpolation.DrawCatmull(points, Gizmos.DrawLine);
        Interpolation.DrawCatmullPoints(points, intervals, Gizmos.DrawLine);
    }
}
