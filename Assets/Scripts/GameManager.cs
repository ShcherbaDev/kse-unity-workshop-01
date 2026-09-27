using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * AI Usage here:
 *
 * Prompt: Implement task 6 from PDF (Galaga): Gameplay loop
 *
 * Answer: Added this GameManager: it keeps the score, ends the game
 *         with Win (formation cleared) or Game Over (player died),
 *         freezes the game via Time.timeScale and restarts it by
 *         reloading the scene. Enemy, Player and EntitiesFormation
 *         report their events to it through the Instance.
 *
 * Prompt: Remake the GUI with proper Canvas
 *
 * Answer: Replaced the temporary IMGUI with a Canvas in the Gameplay
 *         scene: Score text and a hidden Result panel (result text,
 *         final score, Restart button wired to Restart() in the Inspector).
 */

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[Header("UI")]
	[SerializeField] private TMP_Text _scoreText;
	[SerializeField] private GameObject _resultPanel;
	[SerializeField] private TMP_Text _resultText;
	[SerializeField] private TMP_Text _finalScoreText;

	private int _score;
	private bool _isGameOver;

	private void Awake()
	{
		Instance = this;
		Time.timeScale = 1f; // Undo the freeze from the previous run
	}

	public void AddScore(int points)
	{
		_score += points;
		_scoreText.text = $"Score: {_score}";
	}

	public void Win()
	{
		EndGame("YOU WIN");
	}

	public void Lose()
	{
		EndGame("GAME OVER");
	}

	private void EndGame(string resultText)
	{
		if (_isGameOver)
			return;

		_isGameOver = true;
		_resultText.text = resultText;
		_finalScoreText.text = $"Final score: {_score}";
		_resultPanel.SetActive(true);
		Time.timeScale = 0f;
	}

	// Called by the Restart button
	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
