using UnityEngine;

public class Coin : MonoBehaviour
{
	private CoinPool _parent;

	public void SetParent(CoinPool parent) => _parent = parent;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("FrontWheel")) _parent.Release(gameObject);
	}
}
