using UnityEngine;

public class Train : MonoBehaviour
{
	[SerializeField] private Track _track;
	[SerializeField] private SpriteRenderer _train;
	[SerializeField] private Transform _frontWheels;
	[SerializeField] private Transform _backWheels;
	[SerializeField] private SpriteRenderer _frontLeftLight;
	[SerializeField] private SpriteRenderer _frontRightLight;
	[SerializeField] private SpriteRenderer _backLeftLight;
	[SerializeField] private SpriteRenderer _backRightLight;
	[SerializeField] private float _startDistance;
	[SerializeField] private float _wheelBase;
	[SerializeField] private float _speed;
	private StateMachineManager _stateManager;
	private ScoreCounter _scoreCounter;
	private bool IsStart = false;
	

	public void Initializing()
	{
		_frontWheels.position = new Vector2(0, _train.bounds.size.y / 2 - _startDistance);
		_backWheels.position = new Vector2(0, _frontWheels.position.y - _wheelBase);

		_stateManager = new StateMachineManager(_wheelBase, _speed);

		_scoreCounter = new ScoreCounter(_stateManager);

		_stateManager.AddState(0, new StateMachineDrive(0, _stateManager, _track, gameObject, _frontWheels, _backWheels, _frontLeftLight, _frontRightLight, _backLeftLight, _backRightLight, _scoreCounter));
		_stateManager.AddState(1, new StateMachineChoice(1, _stateManager, _track, gameObject, _frontWheels, _backWheels, _frontLeftLight, _frontRightLight, _backLeftLight, _backRightLight, _scoreCounter));
		_stateManager.AddState(2, new StateMachineDrift(2, _stateManager, _track, gameObject, _frontWheels, _backWheels, _frontLeftLight, _frontRightLight, _backLeftLight, _backRightLight, _scoreCounter));
		_stateManager.AddState(3, new StateMachineGameOver(3, _stateManager, _track, gameObject, _frontWheels, _backWheels, _frontLeftLight, _frontRightLight, _backLeftLight, _backRightLight, _scoreCounter));
		_stateManager.AddState(4, new StateMachineView(4, _stateManager, _track, gameObject, _frontWheels, _backWheels, _frontLeftLight, _frontRightLight, _backLeftLight, _backRightLight, _scoreCounter));

		if (GameEvents.IsRestartGame)
		{
			GameEvents.IsRestartGame = false;
			_stateManager.SetState(0);
			StartGame();
		}
		else _stateManager.SetState(4);

		GameEvents.OnGameStart += StartGame;
		GameEvents.OnGameRestart += StartGame;
		GameEvents.OnGameOver += EndGame;
	}

	private void StartGame() => IsStart = true;

	private void EndGame(string text) => IsStart = false;

	public StateMachineManager GetStateManager() => _stateManager;

	private void Update()
	{
		_stateManager?.Update();
	}

	private void OnDestroy()
	{
		_stateManager?.Destroy();

		GameEvents.OnGameStart -= StartGame;
		GameEvents.OnGameRestart -= StartGame;
		GameEvents.OnGameOver -= EndGame;
	}
}
