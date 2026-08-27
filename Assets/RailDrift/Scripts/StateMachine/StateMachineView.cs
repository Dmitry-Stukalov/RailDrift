using UnityEngine;

public class StateMachineView : StateMachineState
{
	public StateMachineView(int id, StateMachineManager stateManager, Track track, GameObject train, Transform frontWheels, Transform backWheels, SpriteRenderer frontLeftLight, SpriteRenderer frontRightLight, SpriteRenderer backLeftLight, SpriteRenderer backRightLight, ScoreCounter scoreCounter)
		: base(id, stateManager, track, train, frontWheels, backWheels, frontLeftLight, frontRightLight, backLeftLight, backRightLight, scoreCounter)
	{ }

	public override void Enter()
	{
		GameEvents.OnGameStart += StartGame;
		GameEvents.OnChoiceTime += StartChoice;

		GameEvents.IsInMenu = true;

		_stateManager.IsLeftRailWay = false;
		_stateManager.IsRightRailWay = false;

		FrontEndLight();
		BackEndLight();
	}

	public override void Exit()
	{
		GameEvents.OnGameStart -= StartGame;
		GameEvents.OnChoiceTime -= StartChoice;
	}

	public override void Destroy()
	{
		GameEvents.OnGameStart -= StartGame;
		GameEvents.OnChoiceTime -= StartChoice;
	}

	public override void Update()
	{
		if (_track.GetPointsCount() == 0) return;

		_stateManager.CurrentDistance += _stateManager.Speed * Time.deltaTime;
		_stateManager.BackDistance += _stateManager.Speed * Time.deltaTime;

		Vector2 posFront = _track.GetFrontWheelPosition(_stateManager.CurrentDistance);
		Vector2 posBack = _track.GetBackWheelPosition(_stateManager.BackDistance);

		if (posFront != null) _frontWheels.position = posFront;
		if (posBack != null) _backWheels.position = posBack;

		_train.transform.position = (posFront + posBack) / 2f;

		Vector2 direction = posFront - posBack;
		if (direction != Vector2.zero)
		{
			float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
			_train.transform.rotation = Quaternion.Euler(0, 0, -angle);
		}

		float currentDistance = Vector2.Distance(posFront, posBack);
		float error = currentDistance - _stateManager.WheelBase;
		_stateManager.BackDistance += error * Time.deltaTime;
	}

	private void StartGame()
	{
		_stateManager.SetState(0);
	}

	private void StartChoice(ExitPoint exitPoint)
	{
		if (exitPoint.NextExitPoint.LeftWayLength() != 0) _stateManager.IsLeftRailWay = true;
		if (exitPoint.NextExitPoint.RightWayLength() != 0) _stateManager.IsRightRailWay = true;

		_stateManager.SetExitPoint(exitPoint);
		_stateManager.SetState(1);
	}
}
