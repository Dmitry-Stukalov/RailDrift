using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using Unity.VisualScripting.FullSerializer.Internal;

public class StateMachineManager
{
	private Dictionary<int, StateMachineState> _states = new Dictionary<int, StateMachineState>();
	private StateMachineState _currentState;
	public ExitPoint LastExitPoint { get; private set; }
	public string LoseText { get; set; }
	public float CurrentDistance 
	{ 
		get
		{
			return _currentDistance;
		}

		set
		{
			_currentDistance = value;
			OnChange?.Invoke(_currentDistance);
		}
	}
	public float BackDistance { get; set; }
	public float WheelBase { get; set; }
	public float Speed { get; set; }
	//public float BoostedSpeed { get; set; }
	public float SlowedSpeed { get; set; }
	//public bool IsBoostedSpeed { get; set; }
	public bool IsSlowedSpeed { get; set; }
	public bool IsDrift { get; set; }
	public bool IsLeftRailWay { get; set; }
	public bool IsRightRailWay { get; set; }
	public bool IsLeftButtonPress { get; set; }
	public bool IsRightButtonPress { get; set; }
	private float _currentDistance;

	public event Action<float> OnChange;

	public StateMachineManager(float wheelBase, float speed)
	{
		WheelBase = wheelBase;
		BackDistance -= wheelBase;
		Speed = speed;
	}

	public void AddState(int id, StateMachineState state) => _states[id] = state;

	public void SetState(int id)
	{
		if (_currentState != null && _currentState.ID == id) return;

		_currentState?.Exit();

		_currentState = _states[id];

		_currentState?.Enter();
	}

	public void Update()
	{
		_currentState?.Update();
	}

	public void Destroy()
	{
		_currentState?.Destroy();
	}

	public void SetExitPoint(ExitPoint exitPoint) => LastExitPoint = exitPoint;
}
