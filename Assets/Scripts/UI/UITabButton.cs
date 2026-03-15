using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITabButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Press Animation")]
    [SerializeField] private float pressedScale = 0.88f;
    [SerializeField] private float pressDuration = 0.08f;
    [SerializeField] private float releaseDuration = 0.15f;

    [Header("Selected Offset")]
    [SerializeField] private float selectedOffsetY = -20f;   // chỉnh giá trị này trong Inspector
    [SerializeField] private float selectMoveDuration = 0.2f;

    public System.Action OnClick;

    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void SetSelected(bool selected)
    {
        transform.DOKill();
        float targetY = selected
            ? originalPosition.y + selectedOffsetY
            : originalPosition.y;

        transform.DOLocalMoveY(targetY, selectMoveDuration).SetEase(Ease.OutCubic);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(pressedScale, pressDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1f, releaseDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnClick?.Invoke();
    }
}
