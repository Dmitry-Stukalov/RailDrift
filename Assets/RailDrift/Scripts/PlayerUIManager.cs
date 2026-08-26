using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerUIManager : MonoBehaviour
{
	[SerializeField] private UIDocument _playerUI;
	private Button _leftActionButton, _rightActionButton;
	private Button _restartButton;
	private Label _loseText;
	private Label _coinCount;
	private ScoreManager _scoreManager;
	private bool IsInitializing = false;

	public void Initializing()
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

		IsInitializing = true;
	}

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
		StartCoroutine(Restart());

		_restartButton.style.display = DisplayStyle.None;
		_loseText.style.display = DisplayStyle.None;
	}

	private IEnumerator Restart()
	{
		AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync("GamePlayScene");

		while (!unloadOperation.isDone)
			yield return null;

		AsyncOperation loadOperation = SceneManager.LoadSceneAsync("GamePlayScene", LoadSceneMode.Additive);

		while (!loadOperation.isDone)
			yield return null;

		Scene loadedScene = SceneManager.GetSceneByName("GamePlayScene");

		if (loadedScene.IsValid())
			SceneManager.SetActiveScene(loadedScene);
	}

	private void ChangeCoins(int coinCount)
	{
		_coinCount.text = coinCount.ToString();
	}

	private void GameOver(string loseText)
	{
		_restartButton.style.display = DisplayStyle.Flex;
		_loseText.text = loseText;
		_loseText.style.display = DisplayStyle.Flex;
	}

	private void OnDestroy()
	{
		_leftActionButton.RegisterCallback<PointerDownEvent>(OnLeftButtonPress);
		_leftActionButton.RegisterCallback<PointerUpEvent>(OnLeftButtonUnPress);
		_rightActionButton.RegisterCallback<PointerDownEvent>(OnRightButtonPress);
		_rightActionButton.RegisterCallback<PointerUpEvent>(OnRightButtonUnPress);
		_restartButton.UnregisterCallback<ClickEvent>(OnRestartGame);

		GameEvents.OnCoinsChange -= ChangeCoins;
		GameEvents.OnGameOver -= GameOver;

		_scoreManager.OnDestroy();
	}
}
