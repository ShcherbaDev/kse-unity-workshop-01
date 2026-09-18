using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
	public int Score { get; private set; }

	public UnityEvent<int> OnScoreChanged = new UnityEvent<int>();

	public void Init()
	{
		Score = 0;
		OnScoreChanged.Invoke(Score);
	}

	public void AddPoint()
	{
		Score++;
		OnScoreChanged.Invoke(Score);
	}
}
