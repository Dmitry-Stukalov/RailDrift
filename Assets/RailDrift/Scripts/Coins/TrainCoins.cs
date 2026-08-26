using TMPro;
using UnityEngine;

public class TrainCoins : MonoBehaviour
{
	[SerializeField] private TextMeshPro _coinsCount;
	private int _currentCoins = 0;

	private void Start()
	{
		CoinPool.OnGetCoin += UpdateText;
		GameEvents.OnGameOver += GameOver;
	}

	private void UpdateText()
	{
		_currentCoins++;
		_coinsCount.text = $"X{_currentCoins}";
	}

	private void GameOver(string text)
	{
		GameEvents.OnAddCoins?.Invoke(_currentCoins);
		_currentCoins = 0;
	}

	private void OnDestroy()
	{
		CoinPool.OnGetCoin -= UpdateText;
		GameEvents.OnGameOver -= GameOver;
	}
}
