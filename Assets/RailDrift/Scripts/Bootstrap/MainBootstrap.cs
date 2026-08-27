using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainBootstrap : MonoBehaviour
{
	[SerializeField] private AllUIManager _allUIManager;

	private void Start()
	{
		_allUIManager.Initializing();

		StartCoroutine(LoadGamePlayScene());

		GameEvents.OnGameRestart += RestartGame;
		GameEvents.OnMainMenu += RestartGame;
	}

	private IEnumerator LoadGamePlayScene()
	{
		AsyncOperation loadOperation = SceneManager.LoadSceneAsync("GamePlayScene", LoadSceneMode.Additive);

		while (!loadOperation.isDone)
			yield return null;

		Scene loadedScene = SceneManager.GetSceneByName("GamePlayScene");
		
		if (loadedScene.IsValid())
			SceneManager.SetActiveScene(loadedScene);
	}

	private void RestartGame() => StartCoroutine(Restart());

	private IEnumerator Restart()
	{
		AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync("GamePlayScene");

		while (!unloadOperation.isDone)
			yield return null;

		AsyncOperation loadOperation = SceneManager.LoadSceneAsync("GamePlayScene", LoadSceneMode.Additive);

		while (!loadOperation.isDone)
			yield return null;

		Scene loadedScene = SceneManager.GetSceneByName("GamePlayScene");

		if (loadedScene.IsValid())
			SceneManager.SetActiveScene(loadedScene);
	}

	private void OnDestroy()
	{
		GameEvents.OnGameRestart -= RestartGame;
		GameEvents.OnMainMenu -= RestartGame;
	}
}
