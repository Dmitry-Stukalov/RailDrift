using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class RailWay : MonoBehaviour
{
	[SerializeField] private List<TrackChoice> _trackChoices;
	[SerializeField] private List<Transform> _points;
	[SerializeField] private List<Transform> _leftPoint;
	[SerializeField] private List<Transform> _rightPoint;
	private RailWayGenerator _pool;
	private int id;

	public void Initializing(int id, RailWayGenerator pool)
	{
		_pool = pool;
		this.id = id;
	}

	public void AddFrontStraightRailWay() => GameEvents.OnAddFrontRailWay?.Invoke(_points);

	public void AddFrontLeftRailWay() => GameEvents.OnAddFrontRailWay?.Invoke(_leftPoint);

	public void AddFrontRightRailWay() => GameEvents.OnAddFrontRailWay?.Invoke(_rightPoint);

	public void AddBackStraightRailWay() => GameEvents.OnAddBackRailWay?.Invoke(_points);

	public void AddBackLeftRailWay() => GameEvents.OnAddBackRailWay?.Invoke(_leftPoint);

	public void AddBackRightRailWay() => GameEvents.OnAddBackRailWay?.Invoke(_rightPoint);

	public List<TrackChoice> GetChoices() => _trackChoices;
	public List<Transform> LeftWayLength() => _leftPoint;
	public List<Transform> StraightWayLength() => _points;
	public List<Transform> RightWayLength() => _rightPoint;

	public void Release()
	{
		_pool.ReleaseRailWay(id, gameObject);
		GetComponent<CoinsWay>().ReleaseCoins();
	}
}
