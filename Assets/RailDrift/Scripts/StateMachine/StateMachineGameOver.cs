using UnityEngine;

public class StateMachineGameOver : StateMachineState
{
	public StateMachineGameOver(int id, StateMachineManager stateManager, Track track, GameObject train, Transform frontWheels, Transform backWheels, SpriteRenderer frontLeftLight, SpriteRenderer frontRightLight, SpriteRenderer backLeftLight, SpriteRenderer backRightLight, ScoreCounter scoreCounter) 
		: base(id, stateManager, track, train, frontWheels, backWheels, frontLeftLight, frontRightLight, backLeftLight, backRightLight, scoreCounter)
	{

	}

	public override void Enter()
	{
		FrontLeftEndLight();
		FrontRightEndLight();
		BackLeftEndLight();
		BackRightEndLight();
		GameEvents.OnGameOver?.Invoke(_stateManager.LoseText);
	}

	public override void Exit()
	{

	}

	public override void Update()
	{

	}
}
