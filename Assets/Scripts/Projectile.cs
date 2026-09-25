using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
	[SerializeField, Min(0f)] private float _speed = 1f;
	[SerializeField] private string _tagToKill;

	private Rigidbody2D _rigidbody;

	public void SetTagToKill(string newTag)
	{
		_tagToKill = newTag;
	}

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
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
		if (!other.gameObject.TryGetComponent(out BaseMortalEntity entity))
			return;

		if (!entity.CompareTag(_tagToKill))
			return;

		entity.Damage();
		Despawn();
	}

	private void Despawn()
	{
		Destroy(gameObject);
	}
}
