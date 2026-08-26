using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainBootstrap : MonoBehaviour
{
	[SerializeField] private PlayerUIManager _playerUIManager;

	private void Awake()
	{
		_playerUIManager.Initializing();

		StartCoroutine(LoadGamePlayScene());
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
}
