using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Pipe : MonoBehaviour
{
	private float _speed;
	private float _despawnPositionX;
	private float _scorePositionX;
	private bool _isPassed;
	private Rigidbody2D _rigidbody;

	[HideInInspector] public UnityEvent OnPassed = new UnityEvent();

	public void Init(float speed, float despawnPositionX, float scorePositionX)
	{
		_speed = speed;
		_despawnPositionX = despawnPositionX;
		_scorePositionX = scorePositionX;
	}

	public void Stop()
	{
		_speed = 0f;
		_rigidbody.linearVelocity = Vector2.zero;
	}

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		if (_rigidbody.position.x < _despawnPositionX)
		{
			Destroy(gameObject);
			return;
		}

		_rigidbody.MovePosition(_rigidbody.position + Vector2.left * (_speed * Time.fixedDeltaTime));

		if (_isPassed || _rigidbody.position.x > _scorePositionX)
			return;

		_isPassed = true;
		OnPassed.Invoke();
	}
}
