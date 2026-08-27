using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter
{
	private StateMachineManager _manager;
	private float _bestScore;
	private float _currentScore;
	private float _multiplyScore = 1;
	private float _lastCount;
	private float _difference;

	public ScoreCounter(StateMachineManager manager)
	{
		_manager = manager;
		_manager.OnChange += UpdateScore;

		UpdateBestScore();
		ChangeMultiply(1);

		GameEvents.OnGameOver += GameOver;
	}

	public void ChangeMultiply(float newMultiply)
	{
		_multiplyScore = newMultiply;

		GameEvents.OnScoreMultiplyChange?.Invoke(_multiplyScore);
	}

	private void UpdateScore(float currentCount)
	{
		_difference += currentCount - _lastCount;
		_lastCount = currentCount;


		float value = Mathf.Round(_difference * _multiplyScore);

        if (value >= 1)
        {
			if (!GameEvents.IsInMenu) _currentScore += value;
			_difference = 0;
		}
		

		GameEvents.OnScoreChange?.Invoke(_currentScore);
	}

	private void UpdateBestScore()
	{
		_bestScore = PlayerPrefs.GetFloat("BestScore", 0);
		GameEvents.OnBestScoreChange?.Invoke(_bestScore);
	}

	private void GameOver(string text)
	{
		if (_currentScore > _bestScore)
		{
			_bestScore = _currentScore;

			PlayerPrefs.SetFloat("BestScore", _bestScore);
			PlayerPrefs.Save();

			GameEvents.OnBestScoreChange?.Invoke(_bestScore);
		}
	}

	public void OnDestroy()
	{
		_manager.OnChange -= UpdateScore;
		GameEvents.OnGameOver -= GameOver;
	}
}
