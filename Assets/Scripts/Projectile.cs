using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
	[SerializeField, Min(0f)] private float _speed = 1f;

	private Rigidbody2D _rigidbody;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		// Move forward
		_rigidbody.MovePosition(transform.position + transform.right * _speed * Time.fixedDeltaTime);
	}

	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
