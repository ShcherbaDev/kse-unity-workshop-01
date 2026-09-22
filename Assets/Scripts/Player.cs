using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player : MonoBehaviour
{
	[SerializeField] private float _jumpForce = 10f;

	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private InputSystem_Actions _input;
	private bool _canControl;

	// Used to check if user is out of bounds (vertically)
	private float _topEdgeY;
	private float _bottomEdgeY;

	[HideInInspector] public UnityEvent OnDeath = new UnityEvent();

	public void Init()
	{
		_rigidbody.bodyType = RigidbodyType2D.Dynamic;
		_canControl = true;
		_animator.enabled = true;
	}

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_input = new InputSystem_Actions();
	}

	private void Start()
	{
		(_topEdgeY, _bottomEdgeY) = GetVerticalEdges();
	}

	private void OnEnable()
	{
		_input.Player.Jump.performed += HandleJump;
		_input.Enable();
	}

	private void OnDisable()
	{
		_input.Player.Jump.performed -= HandleJump;
		_input.Disable();
	}

	private void OnDestroy()
	{
		_input.Dispose();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		bool isPipe = other.gameObject.TryGetComponent(out Pipe _);
		if (!isPipe)
			return;

		Die();
	}

	private void Update()
	{
		HandleOutOfBounds();
	}

	private void HandleJump(InputAction.CallbackContext ctx)
	{
		if (!_canControl)
			return;

		_rigidbody.linearVelocity = Vector2.zero;
		_rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);

		SfxPlayer.PlayJump();
	}
	
	private void HandleOutOfBounds()
	{
		bool isInBounds = transform.position.y < _topEdgeY && transform.position.y > _bottomEdgeY; 
		if (isInBounds)
			return;
		Die();
	}

	private (float, float) GetVerticalEdges()
	{
		Camera cam = Camera.main;
		float cameraTopEdge = cam.ViewportToWorldPoint(Vector2.up).y;
		float cameraBottomEdge = cam.ViewportToWorldPoint(Vector2.zero).y;
		return (cameraTopEdge, cameraBottomEdge);
	}

	private void Die()
	{
		if (!_canControl)
			return;

		_canControl = false;
		_rigidbody.bodyType = RigidbodyType2D.Static;
		_animator.enabled = false;

		SfxPlayer.PlayHit();
		OnDeath?.Invoke();
	}
}
