using UnityEngine;

// Контроль екранів правильніше було б зробити через
// Finite State Machine, але в рамках цієї роботи
// це є трохи overkill
public class UIManager : MonoBehaviour
{
	[SerializeField] private MainMenu _mainMenu;
	[SerializeField] private GameplayScreen _gameplayScreen;
	[SerializeField] private GameOverMenu _gameOverMenu;

	private void Start()
	{
		_gameplayScreen.gameObject.SetActive(false);
		_gameOverMenu.gameObject.SetActive(false);
		_mainMenu.Show();
	}

	public void SetScore(int score)
	{
		_gameplayScreen.SetScoreText(score);
	}

	public void ShowGameplayScreen()
	{
		_gameOverMenu.gameObject.SetActive(false);
		_gameplayScreen.gameObject.SetActive(true);
	}

	public void ShowGameOverMenu(int score)
	{
		_gameplayScreen.gameObject.SetActive(false);
		_gameOverMenu.SetScoreText(score);
		_gameOverMenu.Show();
	}
}
