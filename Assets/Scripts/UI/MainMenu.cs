using UnityEngine;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameController _gameController;
	
	public void GameStart()
	{
		_gameController.Init();
	}

	public void GameExit()
	{
		Application.Quit();
	}
}
