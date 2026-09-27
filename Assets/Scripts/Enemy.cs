using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : BaseMortalEntity
{
	[Header("Score")]
	[SerializeField, Min(0)] private int _points = 10;

	[Header("Death")]
	[SerializeField] private ParticleSystem _explosionPrefab;
	[SerializeField] private AudioClip _explosionSound;
	[SerializeField, Min(0f)] private float _deathAnimationSeconds = 0.2f;

	protected override void Die()
	{
		GameManager.Instance.OnEnemyKilled(_points);

		// The explosion prefab destroys itself when the particles are gone
		Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
		AudioSource.PlayClipAtPoint(_explosionSound, Vector2.zero);
		PlayDeathAnimation();
	}

	// Shrink, then destroy
	private void PlayDeathAnimation()
	{
		GetComponent<Collider2D>().enabled = false;

		DOTween.Sequence()
			.Join(transform.DOScale(0f, _deathAnimationSeconds).SetEase(Ease.InBack))
			.SetUpdate(true)
			.SetLink(gameObject)
			.OnComplete(() => base.Die());
	}
}
