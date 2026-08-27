using UnityEngine;
using UnityEngine.UIElements;

public class MenuUIManager : MonoBehaviour
{
	private VisualElement _mainElement;
	private Button _startButton;
	private Label _coinCount;


	public MenuUIManager(VisualElement mainElement)
	{
		_mainElement = mainElement;

		_startButton = _mainElement.Q<Button>("StartButton");
		_coinCount = _mainElement.Q<Label>("CoinCount");

		_startButton.RegisterCallback<ClickEvent>(StartGame);

		GameEvents.OnCoinsChange += ChangeCoins;

		ChangeCoins(PlayerPrefs.GetInt("Coins", 0));
	}

	private void StartGame(ClickEvent evt)
	{
		GameEvents.OnGameStart?.Invoke();
	}

	private void ChangeCoins(int coinCount)
	{
		_coinCount.text = coinCount.ToString();
	}

	private void OnDestroy()
	{
		_startButton.UnregisterCallback<ClickEvent>(StartGame);

		GameEvents.OnCoinsChange -= ChangeCoins;
	}
}
