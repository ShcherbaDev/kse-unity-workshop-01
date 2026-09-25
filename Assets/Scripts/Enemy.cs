using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : BaseMortalEntity
{
	[Header("Shooting")]
	[SerializeField, Min(0f)] private float _minFireInterval = 1f;
	[SerializeField, Min(0f)] private float _maxFireInterval = 4f;

	// Runs as a coroutine, stops automatically when the enemy is destroyed
	private IEnumerator Start()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(_minFireInterval, _maxFireInterval));
			Fire();
		}
	}
}
