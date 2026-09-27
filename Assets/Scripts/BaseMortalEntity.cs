using System.Collections;
using DG.Tweening;
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

	[Header("Shooting Polish")]
	[SerializeField] private ParticleSystem _muzzleFlashPrefab;
	[SerializeField] private AudioClip _shootSound;

	[Header("Hit Feedback")]
	[SerializeField] private Material _hitFlashMaterial; // Draws the sprite in solid white
	[SerializeField, Min(0f)] private float _hitFlashSeconds = 0.08f;
	[SerializeField, Min(0f)] private float _hitPunchSeconds = 0.15f;
	[SerializeField] private AudioClip _hitSound;

	private bool _isCooldownPassed = true;

	private SpriteRenderer _spriteRenderer;
	private Material _defaultMaterial;
	private Tween _hitFlashTween;

	protected virtual void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_defaultMaterial = _spriteRenderer.sharedMaterial;
	}

	public virtual void Damage()
	{
		// AI usage:
		// Two hits in the same frame must not kill (and score) the entity twice
		if (_health <= 0)
			return;

		_health--;
		if (_health <= 0)
			Die();
		else
			PlayHitFeedback();
	}

	// White flash + scale punch + sound
	private void PlayHitFeedback()
	{
		AudioSource.PlayClipAtPoint(_hitSound, Camera.main.transform.position);

		// Tinting can't make a sprite whiter,
		// so swap to the white material for a moment
		_hitFlashTween?.Kill();
		_spriteRenderer.sharedMaterial = _hitFlashMaterial;
		_hitFlashTween = DOVirtual
			.DelayedCall(
				_hitFlashSeconds,
				() => _spriteRenderer.sharedMaterial = _defaultMaterial
			)
			.SetLink(gameObject);

		// Completing the previous punch keeps the original scale
		// when hits come in quick succession
		transform.DOComplete();
		transform.DOPunchScale(transform.localScale * 0.3f, _hitPunchSeconds).SetLink(gameObject);
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

		// The flash destroys itself when the particles are gone
		Instantiate(_muzzleFlashPrefab, _projectileSpawnPoint.transform.position, _projectileSpawnPoint.transform.rotation);
		AudioSource.PlayClipAtPoint(_shootSound, Camera.main.transform.position);

		StartCoroutine(UpdateCooldown());
	}

	private IEnumerator UpdateCooldown()
	{
		_isCooldownPassed = false;
		yield return new WaitForSeconds(_cooldownSeconds);
		_isCooldownPassed = true;
	}
}
