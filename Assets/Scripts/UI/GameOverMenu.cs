using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _scoreText;

	public void Show(int score)
	{
		_scoreText.text = score.ToString();
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
