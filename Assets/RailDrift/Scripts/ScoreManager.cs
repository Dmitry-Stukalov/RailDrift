using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreManager
{
	private Label _bestScore;
	private Label _currentScore;
	private Label _multiplyScore;
	private float _currentScoreCount = 0;

	public ScoreManager(Label bestScore, Label currentScore, Label multiplyScore)
	{
		_bestScore = bestScore;
		_currentScore = currentScore;
		_multiplyScore = multiplyScore;

		GameEvents.OnScoreChange += UpdateCurrentScore;
		GameEvents.OnBestScoreChange += UpdateBestScore;
		GameEvents.OnScoreMultiplyChange += UpdateMultiplyScore;
	}

	private void UpdateCurrentScore(float currentScore)
	{
		_currentScore.text = $"Пройдено: {currentScore} м";
	}

	private void UpdateBestScore(float bestScore)
	{
		_bestScore.text = $"Рекорд: {bestScore} м";
	}

	private void UpdateMultiplyScore(float multiplyScore)
	{
		_multiplyScore.text = $"Х{multiplyScore}";
	}

	public void OnDestroy()
	{
		GameEvents.OnScoreChange -= UpdateCurrentScore;
		GameEvents.OnBestScoreChange -= UpdateBestScore;
		GameEvents.OnScoreMultiplyChange -= UpdateMultiplyScore;
	}
}
