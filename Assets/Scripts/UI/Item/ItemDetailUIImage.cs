using Coffee.UIEffects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUIImage : MonoBehaviour
{
        [SerializeField] private Image itemIcon;
    [SerializeField] private UIEffect itemIconEffect;
    [SerializeField] private Color defaultColor;
    [Header("Animation")]
    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float floatUpAmount = 20f;
    [SerializeField] private float bobAmount = 8f;
    [SerializeField] private float bobDuration = 1f;
    private Tween fadeTween;
    private Tween bobTween;
    private Vector3 originalPosition;
    private void Awake()
    {
        originalPosition = itemIcon.transform.localPosition;
    }
    public void Setup(ItemData item, int amount)
    {
        if (item == null)
        {
            Clear();
            return;
        }
        Clear();
        itemIcon.sprite = item.Icon;
        itemIconEffect.shadowMode = ShadowMode.Outline8;
        itemIconEffect.shadowColor = InventoryUIManager.Instance.GetRarityColor(item.Rarity);
        itemIconEffect.shadowIteration = 5;
        itemIcon.color = new Color(1, 1, 1, 0);
        itemIcon.transform.localPosition = originalPosition + Vector3.down * floatUpAmount;
        Sequence entrance = DOTween.Sequence();
        entrance.Join(itemIcon.DOFade(1f, fadeInDuration).SetEase(Ease.OutCubic));
        entrance.Join(itemIcon.transform
            .DOLocalMoveY(originalPosition.y, fadeInDuration)
            .SetEase(Ease.OutCubic));
        entrance.OnComplete(() =>
        {
            bobTween = itemIcon.transform
                .DOLocalMoveY(originalPosition.y + bobAmount, bobDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        });
        fadeTween = entrance;
    }
    public void Clear()
    {
        fadeTween?.Kill();
        bobTween?.Kill();
        itemIcon.transform.localPosition = originalPosition;
        itemIcon.sprite = null;
        itemIcon.color = defaultColor;
        itemIconEffect.shadowMode = ShadowMode.None;
    }
}
