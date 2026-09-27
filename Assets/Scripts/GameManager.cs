using DG.Tweening;
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

	// Survives the scene reload, so Restart goes straight into gameplay
	private static bool _skipStartScreen;

	[Header("HUD")]
	[SerializeField] private TMP_Text _scoreText;
	[SerializeField] private TMP_Text _livesText;
	[SerializeField] private TMP_Text _enemiesText;

	[Header("Start Screen")]
	[SerializeField] private GameObject _startPanel;

	[Header("Result Screen")]
	[SerializeField] private CanvasGroup _resultPanel;
	[SerializeField] private Transform _resultContent;
	[SerializeField] private TMP_Text _resultText;
	[SerializeField] private TMP_Text _finalScoreText;
	[SerializeField, Min(0f)] private float _resultAnimationSeconds = 0.4f;

	public bool IsPlaying { get; private set; }

	private int _score;
	private int _enemiesLeft;

	private void Awake()
	{
		Instance = this;
		Time.timeScale = 0f; // Frozen until Play

		if (_skipStartScreen)
		{
			_skipStartScreen = false;
			Play();
		}
	}

	// Called by the Play button
	public void Play()
	{
		_startPanel.SetActive(false);
		IsPlaying = true;
		Time.timeScale = 1f;
	}

	public void SetLives(int lives)
	{
		_livesText.text = $"Lives: {lives}";
	}

	public void SetEnemiesCount(int count)
	{
		_enemiesLeft = count;
		_enemiesText.text = $"Enemies: {_enemiesLeft}";
	}

	public void OnEnemyKilled(int points)
	{
		_score += points;
		_scoreText.text = $"Score: {_score}";
		SetEnemiesCount(_enemiesLeft - 1);

		if (_enemiesLeft == 0)
			EndGame("YOU WIN");
	}

	public void Lose()
	{
		EndGame("GAME OVER");
	}

	private void EndGame(string resultText)
	{
		// The first outcome wins, e.g. the player can't lose after clearing the formation
		if (!IsPlaying)
			return;

		IsPlaying = false;
		Time.timeScale = 0f;

		_resultText.text = resultText;
		_finalScoreText.text = $"Final score: {_score}";
		ShowResultPanel();
	}

	// SetUpdate(true) - tweens run on unscaled time, since the game is frozen
	private void ShowResultPanel()
	{
		_resultPanel.gameObject.SetActive(true);

		_resultPanel.alpha = 0f;
		_resultPanel.DOFade(1f, _resultAnimationSeconds).SetUpdate(true).SetLink(_resultPanel.gameObject);

		_resultContent.localScale = Vector3.one * 0.5f;
		_resultContent.DOScale(1f, _resultAnimationSeconds).SetEase(Ease.OutBack).SetUpdate(true).SetLink(_resultContent.gameObject);
	}

	// Called by the Restart button
	public void Restart()
	{
		_skipStartScreen = true;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
