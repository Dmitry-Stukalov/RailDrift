using TMPro;
using UnityEngine;

public class TrainCoins : MonoBehaviour
{
	[SerializeField] private TextMeshPro _coinsCount;
	private int _currentCoins = 0;

	private void Start()
	{
		CoinPool.OnGetCoin += AddCoin;
		GameEvents.OnGameOver += GameOver;

		UpdateText();
	}

	private void UpdateText()
	{
		_coinsCount.text = $"X{_currentCoins}";
	}

	private void AddCoin()
	{
		_currentCoins++;
		UpdateText();
	}

	private void GameOver(string text)
	{
		GameEvents.OnAddCoins?.Invoke(_currentCoins);
		_currentCoins = 0;
	}

	private void OnDestroy()
	{
		CoinPool.OnGetCoin -= AddCoin;
		GameEvents.OnGameOver -= GameOver;
	}
}
