using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
	[SerializeField] private GameObject Train;

	private void Awake()
	{
		SceneManager.sceneLoaded += SceneLoad;
	}

	private void SceneLoad(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "GamePlayScene")
		{
			Train = FindAnyObjectByType<Train>().gameObject;
		}
	}

	private void LateUpdate()
	{
		if (Train == null) return;

		transform.position = Train.transform.position + new Vector3(0, 0, -10);
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= SceneLoad;
	}
}
