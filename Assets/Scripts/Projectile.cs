using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
	[SerializeField, Min(0f)] private float _speed = 1f;
	[SerializeField] private string _tagToKill;

	private Rigidbody2D _rigidbody;
	private ParticleSystem _trail;
	private BaseMortalEntity _owner;

	public void SetTagToKill(string newTag)
	{
		_tagToKill = newTag;
	}

	public void SetOwner(BaseMortalEntity owner)
	{
		_owner = owner;
	}

	// Reuse after the pool: move first, then activate,
	// and restart the trail, so it doesn't streak from the previous flight
	public void Launch(Vector3 position, Quaternion rotation)
	{
		transform.SetPositionAndRotation(position, rotation);
		gameObject.SetActive(true);

		_trail.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		_trail.Play(true);
	}

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_trail = GetComponent<ParticleSystem>();
	}

	private void FixedUpdate()
	{
		_rigidbody.MovePosition(transform.position + transform.right * _speed * Time.fixedDeltaTime);
	}

	private void OnBecameInvisible()
	{
		Despawn();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!gameObject.activeSelf)
			return;

		if (!other.gameObject.TryGetComponent(out BaseMortalEntity entity))
			return;

		if (!entity.CompareTag(_tagToKill))
			return;

		entity.Damage();
		Despawn();
	}

	private void Despawn()
	{
		// Deactivating can fire OnBecameInvisible again - don't release twice
		if (!gameObject.activeSelf)
			return;

		// The owner (e.g. a killed enemy) and its pool can be gone while the projectile flies
		if (_owner)
			_owner.ReleaseProjectile(this);
		else
			Destroy(gameObject);
	}
}
