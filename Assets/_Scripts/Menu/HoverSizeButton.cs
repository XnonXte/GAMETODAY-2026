using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
public class HoverSizeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform target;
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float duration = 0.15f;
    private Vector3 originalScale;
    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (target == null) return;

        target.DOKill();
        target.DOScale(originalScale * hoverScale, duration).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (target == null) return;

        target.DOKill();
        target.DOScale(originalScale, duration).SetEase(Ease.OutBack).SetUpdate(true);
    }
}