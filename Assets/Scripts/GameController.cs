using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] private Player _player;
	[SerializeField] private PipesGenerator _pipesGenerator;
	[SerializeField] private ScoreManager _scoreManager;
	[SerializeField] private UIManager _uiManager;

	public void Init()
	{
		_player.OnDeath.AddListener(GameLose);
		_pipesGenerator.OnPipePassed.AddListener(_scoreManager.AddPoint);
		_scoreManager.OnScoreChanged.AddListener(_uiManager.SetScore);

		_scoreManager.Init();
		_player.Init();
		_pipesGenerator.Init(_player.transform.position.x);
		
		_uiManager.ShowGameplayScreen();
	}

	private void GameLose()
	{
		_pipesGenerator.Stop();
		_uiManager.ShowGameOverMenu(_scoreManager.Score);

		foreach (Pipe pipe in FindObjectsByType<Pipe>(FindObjectsSortMode.None))
			pipe.Stop();
		foreach (ParallaxLayer layer in FindObjectsByType<ParallaxLayer>(FindObjectsSortMode.None))
			layer.Stop();
	}
}
