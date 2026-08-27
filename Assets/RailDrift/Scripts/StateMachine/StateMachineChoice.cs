using UnityEngine;
using System.Collections.Generic;

public class StateMachineChoice : StateMachineState
{
	private List<TurnDirection> _playerChoices;
	private TurnDirection _frontDirection;
	private bool IsFrontChoiceTime = false;
	private bool IsBackChoiceTime = false;
	private bool IsDoFrontChoice = false;

	public StateMachineChoice(int id, StateMachineManager stateManager, Track track, GameObject train, Transform frontWheels, Transform backWheels, SpriteRenderer frontLeftLight, SpriteRenderer frontRightLight, SpriteRenderer backLeftLight, SpriteRenderer backRightLight, ScoreCounter scoreCounter) 
		: base(id, stateManager, track, train, frontWheels, backWheels, frontLeftLight, frontRightLight, backLeftLight, backRightLight, scoreCounter)
	{
		_playerChoices = new List<TurnDirection>();
	}

	public override void Enter()
	{
		_stateManager.Speed += 0.04f;

		IsFrontChoiceTime = true;
		IsDoFrontChoice = false;
		_stateManager.IsDrift = false;

		_playerChoices.Clear();

		GameEvents.IsPressing = false;

		GameEvents.OnLeftButtonClick += OnLeftClick;
		GameEvents.OnRightButtonClick += OnRightClick;

		GameEvents.OnBackWheelStartChoice += BackChoiceStart;

		GameEvents.OnFrontWheelEndChoice += DoFrontChoice;
		GameEvents.OnBackWheelEndChoice += DoBackChoice;

		FrontStartLight();
	}

	public override void Exit()
	{
		if (!GameEvents.IsPressing) BackEndLight();

		GameEvents.OnLeftButtonClick -= OnLeftClick;
		GameEvents.OnRightButtonClick -= OnRightClick;

		GameEvents.OnBackWheelStartChoice -= BackChoiceStart;

		GameEvents.OnFrontWheelEndChoice -= DoFrontChoice;
		GameEvents.OnBackWheelEndChoice -= DoBackChoice;
	}

	public override void Destroy()
	{
		GameEvents.OnLeftButtonClick -= OnLeftClick;
		GameEvents.OnRightButtonClick -= OnRightClick;

		GameEvents.OnBackWheelStartChoice -= BackChoiceStart;

		GameEvents.OnFrontWheelEndChoice -= DoFrontChoice;
		GameEvents.OnBackWheelEndChoice -= DoBackChoice;
	}

	public override void Update()
	{
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

	private void BackChoiceStart()
	{
		if (!IsDoFrontChoice) return;

		IsBackChoiceTime = true;
		BackStartLight();
	}

	private void DoFrontChoice()
	{
		if (GameEvents.IsInMenu)
		{
			List<TrackChoice> autoChoices = new List<TrackChoice>(_stateManager.LastExitPoint.GetChoices());

			ResultFrontDirection(autoChoices[0].ResultDirections);
			ResultBackDirection(autoChoices[0].ResultDirections);

			_stateManager.SetState(4);
			return;
		}

		FrontEndLight();
		IsFrontChoiceTime = false;

		List<TrackChoice> choices = new List<TrackChoice>(_stateManager.LastExitPoint.GetChoices());

		//Если игрок не делал выбор и самый первый путь - движение прямо и передними, и задними колесами
		if (!IsDoFrontChoice && choices[0].Directions.Count == 0)
		{
			ResultFrontDirection(choices[0].ResultDirections);
			ResultBackDirection(choices[0].ResultDirections);
			_stateManager.SetState(0);
			return;
		}

		for (int i = 0; i < choices.Count; i++)
		{
			if (SequenceComparison(_playerChoices, choices[i].Directions))
			{
				if (SimpleRailWay(choices))
				{
					ResultFrontDirection(choices[i].ResultDirections);
					ResultBackDirection(choices[i].ResultDirections);
					_stateManager.SetState(0);
					return;
				}

				ResultFrontDirection(choices[i].ResultDirections);
				IsDoFrontChoice = true;
				return;
			}
		}

		_stateManager.SetState(3);
	}

	private void DoBackChoice()
	{
		if (!IsBackChoiceTime) return;

		IsBackChoiceTime = false;

		List<TrackChoice> choices = new List<TrackChoice>(_stateManager.LastExitPoint.GetChoices());

		if (_playerChoices.Count == 2 && _playerChoices[0] == _playerChoices[1]) _playerChoices.RemoveAt(0);

		for (int i = 0; i < choices.Count; i++)
		{
			if (SequenceComparison(_playerChoices, choices[i].Directions))
			{
				ResultBackDirection(choices[i].ResultDirections);

				if (!_stateManager.IsDrift) BackEndLight();

				if (GameEvents.IsPressing/* && IsDoFrontChoice*/) _stateManager.SetState(2);
				else _stateManager.SetState(0);

				return;
			}
		}

		_stateManager.LoseText = "Хуй знает что это";
		_stateManager.SetState(3);
	}

	private bool SequenceComparison(List<TurnDirection> player, List<TurnDirection> track)
	{
		if (player.Count != track.Count)
		{
			_stateManager.LoseText = "Неверное количество нажатий";
			return false;
		}

		for (int i = 0; i < player.Count; i++)
		{
			if (player[i] != track[i])
			{
				_stateManager.LoseText = "Неверное направление";
				return false;
			}
		}

			return true;
	}

	private bool SimpleRailWay(List<TrackChoice> track)
	{
		int t = 0;

		for (int i = 0; i < track.Count; i++)
		{
			if (track[i].Directions.Count == 1) t++;
		}

		if (t == track.Count) return true;
		else return false;
	}

	private void ResultFrontDirection(List<TurnDirection> result)
	{
		if (result.Count == 0)
		{
			_stateManager.LastExitPoint.NextExitPoint.AddFrontStraightRailWay();
			_frontDirection = TurnDirection.None;
		}
		
		if (result.Count == 1)
		{
			if (result[0] == TurnDirection.Left) _stateManager.LastExitPoint.NextExitPoint.AddFrontLeftRailWay();

			if (result[0] == TurnDirection.Right) _stateManager.LastExitPoint.NextExitPoint.AddFrontRightRailWay();

			_frontDirection = result[0];
			CalculateSlowedSpeed(result[0]);
		}
	}

	private void ResultBackDirection(List<TurnDirection> result)
	{
		if (result.Count == 0) _stateManager.LastExitPoint.NextExitPoint.AddBackStraightRailWay();

		if (result.Count == 1 && _frontDirection != TurnDirection.None)
		{
			if (result[0] == TurnDirection.Left) _stateManager.LastExitPoint.NextExitPoint.AddBackLeftRailWay();
			if (result[0] == TurnDirection.Right) _stateManager.LastExitPoint.NextExitPoint.AddBackRightRailWay();
		}

		if (result.Count == 1 && _frontDirection == TurnDirection.None)
		{
			_stateManager.LastExitPoint.NextExitPoint.AddBackStraightRailWay();
		}

		if (result.Count == 2)
		{
			if (result[0] == TurnDirection.Right && result[1] == TurnDirection.Left) _stateManager.LastExitPoint.NextExitPoint.AddBackStraightRailWay();

			if (result[0] == TurnDirection.Left && result[1] == TurnDirection.Right) _stateManager.LastExitPoint.NextExitPoint.AddBackStraightRailWay();

			_stateManager.IsSlowedSpeed = true;
			_stateManager.IsDrift = true;
		}
	}

	private void CalculateSlowedSpeed(TurnDirection direction)
	{
		float time = 0;
		float length = _stateManager.LastExitPoint.NextExitPoint.StraightWayLength();

		switch (direction)
		{
			case TurnDirection.None:
				_stateManager.SlowedSpeed = _stateManager.Speed;
				return;
			break;

			case TurnDirection.Left:
				time = _stateManager.LastExitPoint.NextExitPoint.LeftWayLength() / _stateManager.Speed;
			break;

			case TurnDirection.Right:
				time = _stateManager.LastExitPoint.NextExitPoint.RightWayLength() / _stateManager.Speed;
			break;
		}

		_stateManager.SlowedSpeed = length / time;
	}

	protected override void FrontStartLight()
	{
		if (_stateManager.LastExitPoint.GetChoices()[0].Directions.Count == 0 && _stateManager.LastExitPoint.GetChoices().Count == 1) return;

		base.FrontStartLight();
	}

	protected override void BackStartLight()
	{
		if (!IsDoFrontChoice) return;

		base.BackStartLight();
	}

	private void OnLeftClick()
	{
		_playerChoices.Add(TurnDirection.Left);

		if (IsFrontChoiceTime)
		{
			FrontLeftLight();
			IsDoFrontChoice = true;
		}
		else BackLeftLight();
	}

	private void OnRightClick()
	{
		_playerChoices.Add(TurnDirection.Right);

		if (IsFrontChoiceTime)
		{
			FrontRightLight();
			IsDoFrontChoice = true;
		}
		else BackRightLight();
    }
}
