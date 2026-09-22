using UnityEngine;

public class MainMenu : AnimatedScreen
{
	[SerializeField] private GameController _gameController;

	public void GameStart()
	{
		Hide(_gameController.Init);
	}

	public void GameExit()
	{
		Hide(Application.Quit);
	}
}
