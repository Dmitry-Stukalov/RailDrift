using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class RailWayGenerator : MonoBehaviour
{
	[SerializeField] private List<GameObject> _railWayPrefabs;
	private List<GameObject> _currentRailWays = new List<GameObject>();
	private List<ExitPoint> _exitPoints = new List<ExitPoint>();
	private List<ObjectPool<GameObject>> _pools = new List<ObjectPool<GameObject>>();
	private int _lastRailWay = 0;

	public void Initializing()
	{
		for (int i = 0; i < _railWayPrefabs.Count; i++)
		{
			GameObject railWayPrefab = _railWayPrefabs[i];

			_pools.Add(new ObjectPool<GameObject>(
			createFunc: () =>
			{
				GameObject obj = Instantiate(railWayPrefab);
				SceneManager.MoveGameObjectToScene(obj, gameObject.scene);

				return obj;
			},
			actionOnGet: (obj) => obj.SetActive(true),
			actionOnRelease: (obj) => obj.SetActive(false),
			actionOnDestroy: (obj) => Destroy(obj),
			defaultCapacity: 5,
			maxSize: 15
			));
		}

		for (int i = 0; i < 10; i++)
		{
			GenerateRandomRailWay();
		}

		GameEvents.OnChoiceTime += TryGenerateRailWay;
		GameEvents.OnReleaseRailWay += TryReleaseRailWay;
	}

	private void TryGenerateRailWay(ExitPoint exitPoint)
	{
		if (exitPoint.ID >= _currentRailWays.Count - 6) GenerateRandomRailWay();
	}

	private void GenerateRandomRailWay()
	{
		int randomCoinGenerate = Random.Range(0, 10);
		bool IsGenerateCoin = false;

		if (randomCoinGenerate > 5) IsGenerateCoin = true;

		GameObject newRailWay;
		int randomRailWay = 0;
		int[] arr = new int[0];

		switch (_lastRailWay)
		{
			case 0:
				arr = new int[] { 1, 2, 5 };
				randomRailWay = arr[Random.Range(0, arr.Length)];
			break;

			case 1:
				randomRailWay = 3;
			break;

			case 2:
				randomRailWay = 4;
			break;

			case 3 or 4:
				arr = new int[] { 0, 1, 2, 5 };
				randomRailWay = arr[Random.Range(0, arr.Length)];
			break;

			case 5:
				arr = new int[] { 0/*, 1, 2, 5*/ };
				randomRailWay = arr[Random.Range(0, arr.Length)];
			break;
		}

		_lastRailWay = randomRailWay;

		if (_currentRailWays.Count == 0)
		{
			newRailWay = _pools[0].Get();
			newRailWay.GetComponent<RailWay>().Initializing(0, this);
			newRailWay.transform.position = Vector2.zero;
			_lastRailWay = 0;
		}
		else
		{
			newRailWay = _pools[randomRailWay].Get();
			newRailWay.GetComponent<RailWay>().Initializing(randomRailWay, this);
			newRailWay.transform.position = _currentRailWays[_currentRailWays.Count - 1].transform.GetChild(2).position;
		}

		if (IsGenerateCoin) newRailWay.GetComponent<CoinsWay>().GenerateCoins();

		_exitPoints.Add(newRailWay.transform.GetChild(2).GetComponent<ExitPoint>());

		_exitPoints[_exitPoints.Count - 1].ID = _currentRailWays.Count - 1;
		_currentRailWays.Add(newRailWay);

		if (_exitPoints.Count > 1)
		{
			_exitPoints[_exitPoints.Count - 2].NextExitPoint = _exitPoints[_exitPoints.Count - 1];
			ExitPoint _lastExitPoint = _exitPoints[_exitPoints.Count - 2];

			switch (_lastRailWay)
			{
				case 0:
					_lastExitPoint.IsStraight = true;
					break;

				case 1:
					_lastExitPoint.IsRight = true;
					break;

				case 2:
					_lastExitPoint.IsLeft = true;
					break;

				case 3:
					_lastExitPoint.IsLeft = true;
					break;

				case 4:
					_lastExitPoint.IsRight = true;
					break;

				case 5:
					_lastExitPoint.IsStraight = true;
					_lastExitPoint.IsRight = true;
					_lastExitPoint.IsLeft = true;
					break;
			}
		}

		if (_exitPoints.Count == 1)
		{
			_exitPoints[0].AddFrontStraightRailWay();
			_exitPoints[0].AddBackStraightRailWay();
		}
	}

	private void TryReleaseRailWay()
	{
		if (_currentRailWays.Count > 10)
		{
			_currentRailWays[0].GetComponent<RailWay>().Release();
		}
	}

	public void ReleaseRailWay(int id, GameObject railWay)
	{
		_currentRailWays.Remove(railWay);
		_pools[id].Release(railWay);
	}

	private void OnDestroy()
	{
		GameEvents.OnChoiceTime -= TryGenerateRailWay;
		GameEvents.OnReleaseRailWay -= TryReleaseRailWay;
	}
}
