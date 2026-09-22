using System;
using DG.Tweening;
using UnityEngine;

// Базовий екран, що виїжджає знизу і здувається вітром вліво-вниз
[RequireComponent(typeof(Canvas))]
public class AnimatedScreen : MonoBehaviour
{
	[SerializeField] private float Duration = 2f;
	[SerializeField] private float WindRotation = 70f;

	[SerializeField] private RectTransform _panel;

	private Vector2 _homePosition;

	private Vector2 CanvasSize => ((RectTransform)transform).rect.size;

	protected virtual void Awake()
	{
		_homePosition = _panel.anchoredPosition;
	}

	public void Show()
	{
		gameObject.SetActive(true);
		Canvas.ForceUpdateCanvases(); // щойно увімкнений Canvas ще не має розміру
		_panel.DOKill();
		_panel.localRotation = Quaternion.identity;
		_panel.anchoredPosition = _homePosition + Vector2.down * CanvasSize.y;
		_panel.DOAnchorPos(_homePosition, Duration).SetEase(Ease.OutBack);
	}

	public void Hide(Action onComplete = null)
	{
		_panel.DOKill();
		DOTween.Sequence().SetTarget(_panel)
			.Join(_panel.DOAnchorPos(_homePosition - CanvasSize, Duration))
			.Join(_panel.DOLocalRotate(new Vector3(0f, 0f, WindRotation), Duration))
			.SetEase(Ease.InQuad)
			.OnComplete(() =>
			{
				gameObject.SetActive(false);
				onComplete?.Invoke();
			});
	}
}
