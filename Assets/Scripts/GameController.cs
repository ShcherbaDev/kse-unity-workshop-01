using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField] private Player _player;
	[SerializeField] private PipesGenerator _pipesGenerator;
	[SerializeField] private ScoreManager _scoreManager;
	[SerializeField] private UIManager _uiManager;

	private bool _isGameOver;

	private void Start()
	{
		_player.OnDeath.AddListener(GameLose);
		_player.OnJumpPressed.AddListener(RestartOnJump);
		_pipesGenerator.OnPipePassed.AddListener(_scoreManager.AddPoint);
		_scoreManager.OnScoreChanged.AddListener(_uiManager.SetScore);

		_scoreManager.Init();
		_player.Init();
		_pipesGenerator.Init(_player.transform.position.x);
	}

	private void GameLose()
	{
		_isGameOver = true;
		_pipesGenerator.Stop();
		foreach (Pipe pipe in FindObjectsByType<Pipe>(FindObjectsSortMode.None))
			pipe.Stop();
		foreach (ParallaxLayer layer in FindObjectsByType<ParallaxLayer>(FindObjectsSortMode.None))
			layer.Stop();
		Debug.Log($"Game Over. Score: {_scoreManager.Score}");
	}

	private void RestartOnJump()
	{
		if (!_isGameOver)
			return;

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
