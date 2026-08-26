using UnityEngine;

public class StateMachineDrive : StateMachineState
{
	public StateMachineDrive(int id, StateMachineManager stateManager, Track track, GameObject train, Transform frontWheels, Transform backWheels, SpriteRenderer frontLeftLight, SpriteRenderer frontRightLight, SpriteRenderer backLeftLight, SpriteRenderer backRightLight, ScoreCounter scoreCounter) 
		: base(id, stateManager, track, train, frontWheels, backWheels, frontLeftLight, frontRightLight, backLeftLight, backRightLight, scoreCounter)
	{ }

	public override void Enter()
	{
		GameEvents.OnChoiceTime += StartChoice;

		_stateManager.IsLeftRailWay = false;
		_stateManager.IsRightRailWay = false;

		FrontEndLight();
		BackEndLight();
	}

	public override void Exit()
	{
		GameEvents.OnChoiceTime -= StartChoice;

		if (_stateManager.IsSlowedSpeed) _stateManager.IsSlowedSpeed = false;
	}

	public override void Destroy()
	{
		GameEvents.OnChoiceTime -= StartChoice;
	}

	public override void Update()
	{
		if (_track.GetPointsCount() == 0) return;

		_stateManager.CurrentDistance += _stateManager.Speed * Time.deltaTime;

		if (_stateManager.IsSlowedSpeed) _stateManager.BackDistance += _stateManager.SlowedSpeed * Time.deltaTime;
		else _stateManager.BackDistance += _stateManager.Speed * Time.deltaTime;

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

	private void StartChoice(ExitPoint exitPoint)
	{
		if (exitPoint.NextExitPoint.LeftWayLength() != 0) _stateManager.IsLeftRailWay = true;
		if (exitPoint.NextExitPoint.RightWayLength() != 0) _stateManager.IsRightRailWay = true;

		_stateManager.SetExitPoint(exitPoint);
		_stateManager.SetState(1);
	}
}
