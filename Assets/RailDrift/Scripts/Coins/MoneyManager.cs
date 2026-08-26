using UnityEngine;

public class MoneyManager : MonoBehaviour
{
	private int _currentMoney;

	private void Start()
	{
		_currentMoney = PlayerPrefs.GetInt("Coins", 0);

		GameEvents.OnAddCoins += AddCoins;
	}

	private void AddCoins(int coins)
	{
		_currentMoney += coins;

		PlayerPrefs.SetInt("Coins", _currentMoney);
		PlayerPrefs.Save();

		GameEvents.OnCoinsChange?.Invoke(_currentMoney);
	}

	private void OnDestroy()
	{
		GameEvents.OnAddCoins -= AddCoins;
	}
}
