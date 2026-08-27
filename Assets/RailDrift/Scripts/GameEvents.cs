using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public static class GameEvents
{
	public static bool IsPressing { get; set; } = false;
	public static bool IsRestartGame { get; set; } = false;
	public static bool IsInMenu {  get; set; } = false;

	public static Action OnGameStart;
	public static Action OnGameRestart;

	public static Action<List<Transform>> OnAddFrontRailWay;
	public static Action<List<Transform>> OnAddBackRailWay;

	public static Action<ExitPoint> OnChoiceTime;
	public static Action<ExitPoint> OnAddExitPoint;
	public static Action OnFrontWheelChoice;
	public static Action OnBackWheelChoice;

	public static Action OnLeftButtonClick;
	public static Action OnRightButtonClick;

	public static Action OnFrontWheelStartChoice;
	public static Action OnBackWheelStartChoice;
	public static Action OnFrontWheelEndChoice;
	public static Action OnBackWheelEndChoice;

	public static Action OnReleaseRailWay;

	public static Action<float> OnScoreChange;
	public static Action<float> OnBestScoreChange;
	public static Action<float> OnScoreMultiplyChange;
	public static Action<int> OnCoinsChange;

	public static Action<int> OnAddCoins { get; set; }

	public static Action OnMainMenu;
	public static Action<string> OnGameOver;
}
