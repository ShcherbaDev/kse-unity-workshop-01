using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

	[SerializeField, Min(0f)] private float _movementSpeed = 1f;

	private Rigidbody2D _rigidbody;
	private InputSystem_Actions _input;
	private float _horizontalMovementDirection; // Value from -1 to 1. 0 - standing still
	private float _leftEdgeX;
	private float _rightEdgeX;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_input = new InputSystem_Actions();
	}

	private void Start()
	{
		(_leftEdgeX, _rightEdgeX) = GetHorizontalEdges();
		Debug.Log(_leftEdgeX + " " + _rightEdgeX);
	}

	private void OnEnable()
	{
		_input.Enable();
		_input.Player.Fire.performed += Fire;
	}

	private void OnDisable()
	{
		_input.Disable();
		_input.Player.Fire.performed -= Fire;
	}

	private (float, float) GetHorizontalEdges()
	{
		Camera cam = Camera.main;
		float cameraLeftEdge = cam.ViewportToWorldPoint(Vector2.zero).x;
		float cameraRightEdge = cam.ViewportToWorldPoint(Vector2.one).x;
		return (cameraLeftEdge, cameraRightEdge);
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

	private void Fire(InputAction.CallbackContext _)
	{
		Debug.Log("Fire");
	}
}
