using TMPro;
using UnityEngine;

public class GameplayScreen : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _scoreText;

	public void SetScoreText(int score)
	{
		_scoreText.text = score.ToString();
	}
}
