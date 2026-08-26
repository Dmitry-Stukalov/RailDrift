using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CoinsWay : MonoBehaviour
{
	[SerializeField] private List<Transform> _points;

	public void GenerateCoins()
	{
		for (int i = 0; i < _points.Count; i++)
		{
			GameObject coin = CoinPool.Instance.GetCoin();
			coin.transform.position = _points[i].transform.position;
		}
	}
}
