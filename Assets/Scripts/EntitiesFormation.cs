using UnityEngine;

/**
 * AI Usage here:
 *
 * Prompt: I'm trying to create an entities formation script
 *         that has to spawn enemies in a grid.
 *         But the gaps are too huge. How to fix?

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

	private void Start()
	{
		// World size of one entity = sprite size * prefab scale
		Vector2 entitySize = Vector2.Scale(
			_entityPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size,
			_entityPrefab.transform.localScale
		);
		Vector2 step = entitySize + Vector2.one * _gap;

		for (int col = 0; col < _columns; col++)
		{
			for (int row = 0; row < _rows; row++)
			{
				Vector2 spawnPosition = (Vector2)transform.position + new Vector2(step.x * col, step.y * row);
				SpawnSingleEntity(spawnPosition);
			}
		}
	}

	private BaseMortalEntity SpawnSingleEntity(Vector2 spawnPosition)
	{
		return Instantiate(_entityPrefab, spawnPosition, transform.rotation, transform);
	}
}
