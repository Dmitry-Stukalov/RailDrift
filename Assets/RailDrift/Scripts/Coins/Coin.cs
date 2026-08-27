using UnityEngine;

public class Coin : MonoBehaviour
{
	private CoinPool _parent;
	public bool InPool { get; private set; }

	public void SetParent(CoinPool parent)
	{
		_parent = parent;
		InPool = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("FrontWheel") && !GameEvents.IsInMenu) ReleaseCoin();
	}

	private void ReleaseCoin()
	{
		_parent.Release(gameObject);
		InPool = true;
	}
	
	public void ReturnToPool()
	{
		_parent.ReturnToPoll(gameObject);
		InPool = true;
	}
}
