using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class TabSection : MonoBehaviour
{
    [SerializeField] private float showDuration = 0.2f;
    [SerializeField] private float hideDuration = 0.15f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, showDuration).SetEase(Ease.OutCubic);
    }

    public void Hide()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.DOKill();
        canvasGroup.DOFade(0f, hideDuration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
                canvasGroup.alpha = 0f;
            });
    }

    public void HideImmediate()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.DOKill();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}