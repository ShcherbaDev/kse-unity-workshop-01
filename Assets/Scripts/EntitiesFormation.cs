using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * AI Usage here:
 *
 * Prompt: I'm trying to create an entities formation script
 *         that has to spawn enemies in a grid.
 *         But the gaps are too huge. How to fix?
 *
 * Answer: Found it: entities were spaced by localScale (5 units),
 *         but the sprite's on-screen size is sprite size * scale,
 *         much smaller than that, leaving large gaps.
 *         I've fixed the step to use the sprite's actual size
 *         plus an adjustable gap instead.
 */

public class EntitiesFormation : MonoBehaviour
{
	[SerializeField] private BaseMortalEntity _entityPrefab;

	[Header("Grid")]
	[SerializeField, Min(1)] private int _columns = 1;
	[SerializeField, Min(1)] private int _rows = 1;
	[SerializeField, Min(0)] private float _gap = 0.2f;

	[Header("Movement")]
	[SerializeField, Min(0f)] private float _movementSpeed = 1f;
	[SerializeField, Min(0f)] private float _edgeMargin = 0.2f;
	[SerializeField, Min(0f)] private float _stepDownDistance = 0.5f;

	[Header("Shooting")]
	[SerializeField, Min(0f)] private float _minFireInterval = 0.5f;
	[SerializeField, Min(0f)] private float _maxFireInterval = 2f;

	private BaseMortalEntity[,] _entities; // [column, row], destroyed entities become null
	private float _entityHalfWidth;
	private float _leftEdgeX;
	private float _rightEdgeX;
	private float _direction = 1f; // 1 - right, -1 - left

	private void Start()
	{
		// World size of one entity = sprite size * prefab scale
		Vector2 entitySize = Vector2.Scale(
			_entityPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size,
			_entityPrefab.transform.localScale
		);
		_entityHalfWidth = entitySize.x / 2;
		Vector2 step = entitySize + new Vector2(_gap, _gap);

		// Center horizontally (in local space)
		float originX = -step.x * (_columns - 1) / 2;

		_entities = new BaseMortalEntity[_columns, _rows];
		for (int col = 0; col < _columns; col++)
		{
			for (int row = 0; row < _rows; row++)
			{
				// Rows grow along the formation's local up, so the Z rotation sets the direction
				Vector2 localOffset = new Vector2(originX + step.x * col, step.y * row);
				Vector2 spawnPosition = transform.position + transform.rotation * localOffset;
				_entities[col, row] = SpawnSingleEntity(spawnPosition);
			}
		}

		GameManager.Instance.SetEnemiesCount(_columns * _rows);

		(_leftEdgeX, _rightEdgeX) = Utils.GetHorizontalEdges(_edgeMargin);
		StartCoroutine(ShootingLoop());
	}

	// One shared timer, so the fire rate doesn't grow with the number of entities
	private IEnumerator ShootingLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(_minFireInterval, _maxFireInterval));

			List<BaseMortalEntity> shooters = GetFrontEntities();
			if (shooters.Count > 0)
				shooters[Random.Range(0, shooters.Count)].Fire();
		}
	}

	// AI usage:
	// Only the lowest alive entity of each column can fire,
	// so shots never pass through the formation's own front rows
	private List<BaseMortalEntity> GetFrontEntities()
	{
		List<BaseMortalEntity> frontEntities = new List<BaseMortalEntity>();
		for (int col = 0; col < _columns; col++)
		{
			BaseMortalEntity front = null;
			for (int row = 0; row < _rows; row++)
			{
				BaseMortalEntity entity = _entities[col, row];
				bool isAlive = entity;
				if (!isAlive)
					continue;

				bool hasFront = front;
				bool isLowerThanFront = hasFront && entity.transform.position.y < front.transform.position.y;
				if (!hasFront || isLowerThanFront)
					front = entity;
			}

			if (front)
				frontEntities.Add(front);
		}
		return frontEntities;
	}

	private void Update()
	{
		if (transform.childCount == 0)
			return;

		if (IsHittingEdge())
			TurnAroundAndStepDown();

		MoveHorizontally();
	}

	private bool IsHittingEdge()
	{
		(float minX, float maxX) = GetEntitiesHorizontalExtents();
		return _direction > 0
			? maxX + _entityHalfWidth >= _rightEdgeX
			: minX - _entityHalfWidth <= _leftEdgeX;
	}

	// Use the alive entities' extents, so the formation
	// still reaches the edge after its outer columns die
	private (float, float) GetEntitiesHorizontalExtents()
	{
		float minX = float.MaxValue;
		float maxX = float.MinValue;
		foreach (Transform child in transform)
		{
			minX = Mathf.Min(minX, child.position.x);
			maxX = Mathf.Max(maxX, child.position.x);
		}
		return (minX, maxX);
	}

	private void TurnAroundAndStepDown()
	{
		_direction = -_direction;
		transform.position += Vector3.down * _stepDownDistance;
	}

	private void MoveHorizontally()
	{
		transform.position += Vector3.right * (_direction * _movementSpeed * Time.deltaTime);
	}

	private BaseMortalEntity SpawnSingleEntity(Vector2 spawnPosition)
	{
		return Instantiate(_entityPrefab, spawnPosition, transform.rotation, transform);
	}
}
