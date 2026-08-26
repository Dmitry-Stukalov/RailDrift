using UnityEngine;

public class StateMachineState
{
	public int ID { get; set; }
	protected StateMachineManager _stateManager;
	protected Track _track;
	protected GameObject _train;
	protected Transform _frontWheels;
	protected Transform _backWheels;
	protected SpriteRenderer _frontLeftLight;
	protected SpriteRenderer _frontRightLight;
	protected SpriteRenderer _backLeftLight;
	protected SpriteRenderer _backRightLight;
	protected ScoreCounter _scoreCounter;

	public StateMachineState(int id, StateMachineManager stateManager, Track track, GameObject train, Transform frontWheels, Transform backWheels, SpriteRenderer frontLeftLight, SpriteRenderer frontRightLight, SpriteRenderer backLeftLight, SpriteRenderer backRightLight, ScoreCounter scoreCounter)
	{
		ID = id;
		_stateManager = stateManager;
		_track = track;
		_train = train;
		_frontWheels = frontWheels;
		_backWheels = backWheels;
		_frontLeftLight = frontLeftLight;
		_frontRightLight = frontRightLight;
		_backLeftLight = backLeftLight;
		_backRightLight = backRightLight;
		_scoreCounter = scoreCounter;
	}

	public virtual void Enter() { }

	public virtual void Exit() { }

	public virtual void Update() { }

	public virtual void Destroy() { }

	protected virtual void FrontStartLight()
	{
		if (_stateManager.IsLeftRailWay) FrontLeftStartLight();
		if (_stateManager.IsRightRailWay) FrontRightStartLight();
	}

	protected virtual void BackStartLight()
	{
		if (_stateManager.IsLeftRailWay) BackLeftStartLight();
		if (_stateManager.IsRightRailWay) BackRightStartLight();
	}

	protected void FrontEndLight()
	{
		FrontLeftEndLight();
		FrontRightEndLight();
	}

	protected void BackEndLight()
	{
		BackLeftEndLight();
		BackRightEndLight();
	}

	protected void FrontLeftStartLight() => ChangeColor(_frontLeftLight, 0.5f);
	protected void FrontRightStartLight() => ChangeColor(_frontRightLight, 0.5f);

	protected void BackLeftStartLight() => ChangeColor(_backLeftLight, 0.5f);
	protected void BackRightStartLight() => ChangeColor(_backRightLight, 0.5f);

	protected void FrontLeftEndLight() => ChangeColor(_frontLeftLight, 0f);
	protected void FrontRightEndLight() => ChangeColor(_frontRightLight, 0f);

	protected void BackLeftEndLight() => ChangeColor(_backLeftLight, 0f);
	protected void BackRightEndLight() => ChangeColor(_backRightLight, 0f);

	protected void FrontLeftLight() => ChangeColor(_frontLeftLight, 1f);
	protected void FrontRightLight() => ChangeColor(_frontRightLight, 1f);

	protected void BackLeftLight() => ChangeColor(_backLeftLight, 1f);
	protected void BackRightLight() => ChangeColor(_backRightLight, 1f);

	protected void ChangeColor(SpriteRenderer spriteRenderer, float alpha)
	{
		Color color = spriteRenderer.color;
		color.a = alpha;
		spriteRenderer.color = color;
	}
}
