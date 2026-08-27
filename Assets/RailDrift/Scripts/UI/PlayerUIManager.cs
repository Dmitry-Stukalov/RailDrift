using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerUIManager : MonoBehaviour
{
	private VisualElement _mainElement;
	private Button _leftActionButton, _rightActionButton;
	private Button _restartButton;
	private Button _mainMenuButton;
	private Label _loseText;
	private Label _coinCount;
	private ScoreManager _scoreManager;
	private bool IsInitializing = false;

	public PlayerUIManager(VisualElement mainElement)
	{
		_mainElement = mainElement;

		_leftActionButton = _mainElement.Q<Button>("LeftActionButton");
		_rightActionButton = _mainElement.Q<Button>("RightActionButton");
		_restartButton = _mainElement.Q<Button>("RestartButton");
		_mainMenuButton = _mainElement.Q<Button>("MainMenuButton");
		_loseText = _mainElement.Q<Label>("LoseText");
		_coinCount = _mainElement.Q<Label>("CoinCount");

		_restartButton.style.display = DisplayStyle.None;
		_mainMenuButton.style.display = DisplayStyle.None;
		_loseText.style.display = DisplayStyle.None;

		_leftActionButton.RegisterCallback<PointerDownEvent>(OnLeftButtonPress, TrickleDown.TrickleDown);
		_leftActionButton.RegisterCallback<PointerUpEvent>(OnLeftButtonUnPress, TrickleDown.TrickleDown);
		_rightActionButton.RegisterCallback<PointerDownEvent>(OnRightButtonPress, TrickleDown.TrickleDown);
		_rightActionButton.RegisterCallback<PointerUpEvent>(OnRightButtonUnPress, TrickleDown.TrickleDown);
		_restartButton.RegisterCallback<ClickEvent>(OnRestartGame);
		_mainMenuButton.RegisterCallback<ClickEvent>(OnMainMenu);

		_scoreManager = new ScoreManager(_mainElement.Q<Label>("BestScore"), _mainElement.Q<Label>("Score"), _mainElement.Q<Label>("ScoreMultiply"));

		//GameEvents.OnCoinsChange += ChangeCoins;
		GameEvents.OnGameOver += GameOver;

		//ChangeCoins(PlayerPrefs.GetInt("Coins", 0));

	}

	/*public void Initializing()
	{
		if (IsInitializing) return;

		_leftActionButton = _playerUI.rootVisualElement.Q<Button>("LeftActionButton");
		_rightActionButton = _playerUI.rootVisualElement.Q<Button>("RightActionButton");
		_restartButton = _playerUI.rootVisualElement.Q<Button>("RestartButton");
		_restartButton.style.display = DisplayStyle.None;

		_loseText = _playerUI.rootVisualElement.Q<Label>("LoseText");
		_loseText.style.display = DisplayStyle.None;

		_coinCount = _playerUI.rootVisualElement.Q<Label>("CoinCount");

		_leftActionButton.RegisterCallback<PointerDownEvent>(OnLeftButtonPress, TrickleDown.TrickleDown);
		_leftActionButton.RegisterCallback<PointerUpEvent>(OnLeftButtonUnPress, TrickleDown.TrickleDown);
		_rightActionButton.RegisterCallback<PointerDownEvent>(OnRightButtonPress, TrickleDown.TrickleDown);
		_rightActionButton.RegisterCallback<PointerUpEvent>(OnRightButtonUnPress, TrickleDown.TrickleDown);
		_restartButton.RegisterCallback<ClickEvent>(OnRestartGame);

		_scoreManager = new ScoreManager(_playerUI.rootVisualElement.Q<Label>("BestScore"), _playerUI.rootVisualElement.Q<Label>("Score"), _playerUI.rootVisualElement.Q<Label>("ScoreMultiply"));

		GameEvents.OnCoinsChange += ChangeCoins;
		GameEvents.OnGameOver += GameOver;

		ChangeCoins(PlayerPrefs.GetInt("Coins", 0));

		IsInitializing = true;
	}*/

	private void OnLeftButtonPress(PointerDownEvent evt)
	{
		GameEvents.IsPressing = true;
		GameEvents.OnLeftButtonClick?.Invoke();
	}

	private void OnLeftButtonUnPress(PointerUpEvent evt)
	{
		GameEvents.IsPressing = false;
	}

	private void OnRightButtonPress(PointerDownEvent evt)
	{
		GameEvents.IsPressing = true;
		GameEvents.OnRightButtonClick?.Invoke();
	}

	private void OnRightButtonUnPress(PointerUpEvent evt)
	{
		GameEvents.IsPressing = false;
	}

	private void OnRestartGame(ClickEvent evt)
	{
		//StartCoroutine(Restart());

		_restartButton.style.display = DisplayStyle.None;
		_mainMenuButton.style.display = DisplayStyle.None;
		_loseText.style.display = DisplayStyle.None;

		GameEvents.IsRestartGame = true;

		GameEvents.OnGameRestart?.Invoke();
	}

	//private IEnumerator Restart()
	//{
	//	AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync("GamePlayScene");

	//	while (!unloadOperation.isDone)
	//		yield return null;

	//	AsyncOperation loadOperation = SceneManager.LoadSceneAsync("GamePlayScene", LoadSceneMode.Additive);

	//	while (!loadOperation.isDone)
	//		yield return null;

	//	Scene loadedScene = SceneManager.GetSceneByName("GamePlayScene");

	//	if (loadedScene.IsValid())
	//		SceneManager.SetActiveScene(loadedScene);
	//}

	//private void ChangeCoins(int coinCount)
	//{
	//	_coinCount.text = coinCount.ToString();
	//}

	private void OnMainMenu(ClickEvent evt)
	{
		_restartButton.style.display = DisplayStyle.None;
		_mainMenuButton.style.display = DisplayStyle.None;
		_loseText.style.display = DisplayStyle.None;
		GameEvents.OnMainMenu?.Invoke();
	}

	private void GameOver(string loseText)
	{
		_restartButton.style.display = DisplayStyle.Flex;
		_mainMenuButton.style.display = DisplayStyle.Flex;
		_loseText.text = loseText;
		_loseText.style.display = DisplayStyle.Flex;
	}

	private void OnDestroy()
	{
		_leftActionButton.UnregisterCallback<PointerDownEvent>(OnLeftButtonPress);
		_leftActionButton.UnregisterCallback<PointerUpEvent>(OnLeftButtonUnPress);
		_rightActionButton.UnregisterCallback<PointerDownEvent>(OnRightButtonPress);
		_rightActionButton.UnregisterCallback<PointerUpEvent>(OnRightButtonUnPress);
		_restartButton.UnregisterCallback<ClickEvent>(OnRestartGame);
		_mainMenuButton.UnregisterCallback<ClickEvent>(OnMainMenu);

		//GameEvents.OnCoinsChange -= ChangeCoins;
		GameEvents.OnGameOver -= GameOver;

		_scoreManager.OnDestroy();
	}
}
