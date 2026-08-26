using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class Track : MonoBehaviour
{
	private List<Vector2> _frontPoints = new List<Vector2>();
	private List<Vector2> _backPoints = new List<Vector2>();
	private ExitPoint _lastExitPoint;
	private float _frontDistance = 0;
	private float _backDistance = 0;

	public void Initializing()
	{
		GameEvents.OnAddExitPoint += AddExitPoint;
		GameEvents.OnAddFrontRailWay += AddFrontPoints;
		GameEvents.OnAddBackRailWay += AddBackPoints;
	}

	private void AddExitPoint(ExitPoint lastExitPoint) => _lastExitPoint = lastExitPoint;

	public void AddFrontPoints(List<Transform> points)
	{
		if (_frontPoints.Count > 0)
		{
			_frontPoints.RemoveRange(0, _frontPoints.Count - 2);
		}

		for (int i = 0; i < points.Count; i++) _frontPoints.Add(points[i].position);
	}

	public void AddBackPoints(List<Transform> points)
	{
		if (_backPoints.Count > 0)
		{
			//_backPoints.RemoveRange(0, _backPoints.Count - 2);
			GameEvents.OnReleaseRailWay?.Invoke();
		}

		for (int i = 0; i < points.Count; i++) _backPoints.Add(points[i].position);
	}

	public int GetPointsCount() => _frontPoints.Count;

	public Vector2 GetFrontWheelPosition(float distance)
	{
		if (_frontPoints.Count == 0) return Vector2.zero;

		if (distance <= 0) return _frontPoints[0];

		float coveredDistance = _frontDistance;

		for (int i = 0; i < _frontPoints.Count - 1; i++)
		{
			Vector2 currentPoint = _frontPoints[i];
			Vector2 nextPoint = _frontPoints[i + 1];

			float segmentLength = Vector2.Distance(currentPoint, nextPoint);

			if (coveredDistance + segmentLength >= distance) 
			{
				float localDistance = distance - coveredDistance;
				float t = localDistance / segmentLength;

				if (i == _frontPoints.Count - 2 && t <= 0.1f)
				{
					GameEvents.OnChoiceTime?.Invoke(_lastExitPoint);
					GameEvents.OnFrontWheelStartChoice?.Invoke();
				}

				if (i == _frontPoints.Count - 2 && t >= 0.9f)
				{
					GameEvents.OnFrontWheelEndChoice?.Invoke();
					_frontDistance = coveredDistance;
				}

				return Vector2.Lerp(currentPoint, nextPoint, t);
			}

			coveredDistance += segmentLength;
		}

		return _frontPoints[_frontPoints.Count - 1];
	}

	public Vector2 GetBackWheelPosition(float distance)
	{
		if (_backPoints.Count == 0) return Vector2.zero;

		if (distance <= 0) return _backPoints[0];

		float coveredDistance = 0;

		for (int i = 0; i < _backPoints.Count - 1; i++)
		{
			Vector2 currentPoint = _backPoints[i];
			Vector2 nextPoint = _backPoints[i + 1];

			float segmentLength = Vector2.Distance(currentPoint, nextPoint);

			if (coveredDistance + segmentLength >= distance)
			{
				float localDistance = distance - coveredDistance;
				float t = localDistance / segmentLength;

				if (i == _backPoints.Count - 2 && t <= 0.1f) GameEvents.OnBackWheelStartChoice?.Invoke();

				if (i == _backPoints.Count - 2 && t >= 0.9f)
				{
					GameEvents.OnBackWheelEndChoice?.Invoke();
					_backDistance = coveredDistance;
				}

				return Vector2.Lerp(currentPoint, nextPoint, t);
			}

			coveredDistance += segmentLength;
		}

		return _backPoints[_backPoints.Count - 1];
	}

	private void OnDestroy()
	{
		GameEvents.OnAddExitPoint -= AddExitPoint;
		GameEvents.OnAddFrontRailWay -= AddFrontPoints;
		GameEvents.OnAddBackRailWay -= AddBackPoints;
	}
}
