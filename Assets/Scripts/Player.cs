using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : BaseMortalEntity
{
	[Header("Movement")]
	[SerializeField, Min(0f)] private float _movementSpeed = 1f;

	private Rigidbody2D _rigidbody;
	private InputSystem_Actions _input;

	private float _horizontalMovementDirection; // Value from -1 to 1. 0 - standing still
	private float _leftEdgeX;
	private float _rightEdgeX;

	protected override void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_input = new InputSystem_Actions();

		base.Awake();
	}

	private void Start()
	{
		(_leftEdgeX, _rightEdgeX) = Utils.GetHorizontalEdges();
		GameManager.Instance.SetLives(_health);
	}

	private void OnEnable()
	{
		_input.Enable();
		_input.Player.Fire.performed += HandleFire;
	}

	private void OnDisable()
	{
		_input.Disable();
		_input.Player.Fire.performed -= HandleFire;
	}

	private void Update()
	{
		_horizontalMovementDirection = _input.Player.Move.ReadValue<float>();
	}

	private void FixedUpdate()
	{
		HandleMovement();
	}

	private void HandleMovement()
	{
		Vector2 deltaMove = new Vector2(_horizontalMovementDirection, 0) * _movementSpeed * Time.fixedDeltaTime;
		Vector2 deltaPosition = _rigidbody.position + deltaMove;

		// Clamp the position
		// so the player won't go out of bounds
		deltaPosition.x = Mathf.Clamp(deltaPosition.x, _leftEdgeX, _rightEdgeX);

		_rigidbody.MovePosition(deltaPosition);
	}

	public override void Damage()
	{
		base.Damage();
		GameManager.Instance.SetLives(_health);
	}

	protected override void Die()
	{
		GameManager.Instance.Lose();
		base.Die();
	}

	private void HandleFire(InputAction.CallbackContext _)
	{
		// No shots on the Start/Result screens (e.g. clicking the Play button)
		if (!GameManager.Instance.IsPlaying)
			return;

		Fire();
	}
}
