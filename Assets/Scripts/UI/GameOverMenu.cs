using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : AnimatedScreen
{
	[SerializeField] private TextMeshProUGUI _scoreText;

	public void SetScoreText(int score)
	{
		_scoreText.text = score.ToString();
	}

	public void Restart()
	{
		Hide(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
	}
}
