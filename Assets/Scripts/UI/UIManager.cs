using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private MainMenu _mainMenu;
	[SerializeField] private GameOverMenu _gameOverMenu;
	[SerializeField] private TextMeshProUGUI _scoreText;

	private void Start()
	{
		_mainMenu.gameObject.SetActive(true);
		_gameOverMenu.gameObject.SetActive(false);
	}

	public void SetScore(int score)
	{
		_scoreText.text = score.ToString();
	}

	public void ShowGameOverMenu(int score)
	{
		_gameOverMenu.gameObject.SetActive(true);
		_gameOverMenu.Show(score);
	}
}
