using UnityEngine;

// Паралакс не працює на екзотичних відношеннях сторін -
// якщо екран надто широкий, то видно область без фону
[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxLayer : MonoBehaviour
{
	[SerializeField] private float _speed = 1f;

	private SpriteRenderer _renderer;
	private float _tileWidth;
	private float _startPositionX;
	private float _offset;

	public void Stop()
	{
		_speed = 0f;
	}

	private void Awake()
	{
		_renderer = GetComponent<SpriteRenderer>();
		_tileWidth = _renderer.sprite.bounds.size.x * transform.lossyScale.x;
		_startPositionX = transform.position.x;
	}

	private void Update()
	{
		_offset = Mathf.Repeat(_offset + _speed * Time.deltaTime, _tileWidth);

		Vector3 position = transform.position;
		position.x = _startPositionX - _offset;
		transform.position = position;
	}
}
