using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : BaseMortalEntity
{
	[Header("Score")]
	[SerializeField, Min(0)] private int _points = 10;

	protected override void Die()
	{
		GameManager.Instance.OnEnemyKilled(_points);
		base.Die();
	}
}
