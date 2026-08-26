using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitPoint : MonoBehaviour
{
	[SerializeField] private RailWay _railWay;
	public ExitPoint NextExitPoint { get; set; }
	public int ID { get; set; }
	public bool IsStraight { get; set; }
	public bool IsLeft { get; set; }
	public bool IsRight { get; set; }

	public void AddFrontStraightRailWay()
	{
		_railWay.AddFrontStraightRailWay();
		AddExitPoint();
	}
	public void AddFrontLeftRailWay()
	{
		_railWay.AddFrontLeftRailWay();
		AddExitPoint();
	}
	public void AddFrontRightRailWay()
	{
		_railWay.AddFrontRightRailWay();
		AddExitPoint();
	}

	public void AddBackStraightRailWay() => _railWay.AddBackStraightRailWay();
	public void AddBackLeftRailWay() => _railWay.AddBackLeftRailWay();
	public void AddBackRightRailWay() => _railWay.AddBackRightRailWay();

	public void AddExitPoint() => GameEvents.OnAddExitPoint?.Invoke(this);

	public RailWay GetRailWay() => _railWay;

	public List<TrackChoice> GetChoices() => NextExitPoint.GetRailWay().GetChoices();

	public float LeftWayLength()
	{
		float length = 0;

		List<Transform> points = new List<Transform>(_railWay.LeftWayLength());

		for (int i = 0; i < points.Count - 1; i++)
		{
			float segmentLength = Vector2.Distance(points[i].position, points[i + 1].position);
			length += segmentLength;
		}

		return length;
	}

	public float StraightWayLength()
	{
		float length = 0;

		List<Transform> points = new List<Transform>(_railWay.StraightWayLength());

		for (int i = 0; i < points.Count - 1; i++)
		{
			float segmentLength = Vector2.Distance(points[i].position, points[i + 1].position);
			length += segmentLength;
		}

		return length;
	}

	public float RightWayLength()
	{
		float length = 0;

		List<Transform> points = new List<Transform>(_railWay.RightWayLength());

		for (int i = 0; i < points.Count - 1; i++)
		{
			float segmentLength = Vector2.Distance(points[i].position, points[i + 1].position);
			length += segmentLength;
		}

		return length;
	}
}
