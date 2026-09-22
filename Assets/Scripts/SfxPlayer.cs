using UnityEngine;
using UnityEngine.UI;

// Одна точка відтворення звукових ефектів на всю гру
[RequireComponent(typeof(AudioSource))]
public class SfxPlayer : MonoBehaviour
{
	[SerializeField] private AudioClip _click;
	[SerializeField] private AudioClip _jump;
	[SerializeField] private AudioClip _hit;
	[SerializeField] private AudioClip _score;
	[SerializeField] private AudioClip _whoosh;

	private static SfxPlayer Instance;

	private AudioSource _source;

	public static void PlayJump() => Instance?.Play(Instance._jump);
	public static void PlayHit() => Instance?.Play(Instance._hit);
	public static void PlayScore() => Instance?.Play(Instance._score);
	public static void PlayWhoosh() => Instance?.Play(Instance._whoosh);

	private void Awake()
	{
		Instance = this;
		_source = GetComponent<AudioSource>();

		// Всі кнопки клацають однаково, тому не тягнемо цей виклик у кожен екран
		// А ще я лінивий додавати event на звук кліку через inspector вручну
		foreach (Button button in FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			button.onClick.AddListener(() => Play(_click));
	}

	private void Play(AudioClip clip)
	{
		_source.PlayOneShot(clip);
	}
}
