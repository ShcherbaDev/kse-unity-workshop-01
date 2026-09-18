using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PipesGenerator : MonoBehaviour
{
	[SerializeField] private Pipe _pipePrefab;

	[Header("Spawn rules")]
	[SerializeField] private float _spawnInterval = 1f;
	[SerializeField, Range(0, 1)] private float _pipeMinVerticalOffset = -0.5f;
	[SerializeField, Range(0, 1)] private float _pipeMaxVerticalOffset = 0.5f;

	[Header("Speed")]
	[SerializeField] private float _pipeSpeed = 3f;
	[SerializeField] private float _pipeMaxSpeed = 8f;
	[SerializeField] private float _pipeSpeedStep = 0.1f;

	private float _currentPipeSpeed;
	private float _despawnPositionX;
	private float _scorePositionX;
	private Coroutine _spawnCycleCoroutine;

	[HideInInspector] public UnityEvent OnPipePassed = new UnityEvent();

	public void Init(float scorePositionX)
	{
		_despawnPositionX = GetDespawnHorizontalPosition();
		_scorePositionX = scorePositionX;
		_currentPipeSpeed = _pipeSpeed;
		if (_spawnCycleCoroutine == null)
			_spawnCycleCoroutine = StartCoroutine(SpawnCycle());
	}

	public void Stop()
	{
		if (_spawnCycleCoroutine != null)
			StopCoroutine(_spawnCycleCoroutine);
		_spawnCycleCoroutine = null;
	}

	private Vector2 GetSpawnPosition()
	{
		float verticalOffset = Random.Range(_pipeMinVerticalOffset, _pipeMaxVerticalOffset);
		return Camera.main.ViewportToWorldPoint(new Vector2(1, verticalOffset));
	}

	private float GetDespawnHorizontalPosition()
	{
		return Camera.main.ViewportToWorldPoint(Vector2.zero).x;
	}

	private IEnumerator SpawnCycle()
	{
		while (true)
		{
			Vector2 spawnPosition = GetSpawnPosition();
			Pipe pipe = Instantiate(_pipePrefab, spawnPosition, transform.rotation);
			pipe.Init(_currentPipeSpeed, _despawnPositionX, _scorePositionX);
			pipe.OnPassed.AddListener(OnPipePassed.Invoke);
			_currentPipeSpeed = Mathf.Min(_currentPipeSpeed + _pipeSpeedStep, _pipeMaxSpeed);

			yield return new WaitForSeconds(_spawnInterval);
		}
	}
}
