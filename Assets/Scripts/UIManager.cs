using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _scoreText;

	public void SetScore(int score)
	{
		_scoreText.text = score.ToString();
	}
}
