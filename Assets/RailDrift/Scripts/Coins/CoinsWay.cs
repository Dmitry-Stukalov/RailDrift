using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CoinsWay : MonoBehaviour
{
	[SerializeField] private List<Transform> _points;
	private List<Coin> _coins = new List<Coin>();

	public void GenerateCoins()
	{
		for (int i = 0; i < _points.Count; i++)
		{
			GameObject coin = CoinPool.Instance.GetCoin();
			_coins.Add(coin.GetComponent<Coin>());
			coin.transform.position = _points[i].transform.position;
		}
	}

	public void ReleaseCoins()
	{
		for (int i = 0; i < _coins.Count; i++)
		{
			if (!_coins[i].InPool) _coins[i].ReturnToPool();
		}
	}
}
