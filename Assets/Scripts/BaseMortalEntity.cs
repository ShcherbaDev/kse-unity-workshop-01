using System.Collections;
using UnityEngine;

public abstract class BaseMortalEntity : MonoBehaviour
{
	[Header("Fighting")]
	[SerializeField, Min(0)] protected int _health = 3; // 3 lives by default
	[SerializeField, Min(0f)] protected float _cooldownSeconds = 0.1f;

	[Header("Projectile")]
	[SerializeField] protected Projectile _projectilePrefab;
	[SerializeField] protected GameObject _projectileSpawnPoint;
	[SerializeField] protected string _enemiesTag;

	private bool _isCooldownPassed = true;

	public virtual void Damage()
	{
		// AI usage:
		// Two hits in the same frame must not kill (and score) the entity twice
		if (_health <= 0)
			return;

		_health--;
		if (_health <= 0)
			Die();
	}

	protected virtual void Die()
	{
		Destroy(gameObject);
	}

	public void Fire()
	{
		// Dead entities can still be around while their death animation plays
		if (!_isCooldownPassed || _health <= 0)
			return;

		Projectile projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.transform.position, _projectileSpawnPoint.transform.rotation);
		projectile.SetTagToKill(_enemiesTag);
		StartCoroutine(UpdateCooldown());
	}

	private IEnumerator UpdateCooldown()
	{
		_isCooldownPassed = false;
		yield return new WaitForSeconds(_cooldownSeconds);
		_isCooldownPassed = true;
	}
}
