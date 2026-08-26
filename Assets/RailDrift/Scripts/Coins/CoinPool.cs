using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class CoinPool : MonoBehaviour
{
	[SerializeField] private GameObject Coin;
	private static CoinPool _instance;
	public static CoinPool Instance => _instance;
	private ObjectPool<GameObject> _pool;

	public static event Action OnGetCoin;

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			_instance = this;
			return;
		}

		_instance = this;
	}

	private void Start()
	{
		_pool = new ObjectPool<GameObject>(
		createFunc: () =>
		{
			GameObject obj = Instantiate(Coin);
			SceneManager.MoveGameObjectToScene(obj, gameObject.scene);

			return obj;
		},
		actionOnGet: (obj) => obj.SetActive(true),
		actionOnRelease: (obj) => obj.SetActive(false),
		actionOnDestroy: (obj) => Destroy(obj),
		defaultCapacity: 20,
		maxSize: 40
		);
	}

	public GameObject GetCoin()
	{
		GameObject coin = _pool.Get();

		coin.GetComponent<Coin>().SetParent(this);

		return coin;
	}

	public void Release(GameObject obj)
	{
		_pool.Release(obj);
		OnGetCoin?.Invoke();
		Debug.Log("Z");
	}
}
