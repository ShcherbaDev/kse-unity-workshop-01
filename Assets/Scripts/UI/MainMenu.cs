using UnityEngine;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameController _gameController;
	
	public void GameStart()
	{
		_gameController.Init();
		Destroy(gameObject);
	}

	public void GameExit()
	{
		Application.Quit();
	}
}
