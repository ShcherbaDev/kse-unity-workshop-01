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

	public void Damage()
	{
		_health--;
		if (_health <= 0)
			Die();
	}

	protected virtual void Die()
	{
		Destroy(gameObject);
	}

	protected void Fire()
	{
		if (!_isCooldownPassed)
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
