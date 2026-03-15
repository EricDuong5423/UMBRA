using System;
using Coffee.UIEffects;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    public ItemData currentItem { get; private set; }
    public int currentItemAmount {get; private set;}
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackCountText;
    [SerializeField] private UIEffect glowEffect;
    [SerializeField] private Image selectedFrame;
    
    [Header("Animation")]
    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float floatUpAmount = 30f;

    private Tween fadeTween;
    private Vector3 defaultTransform;

    private void Awake()
    {
        defaultTransform = itemIcon.transform.localPosition;
    }

    public void SetHighLight(bool isSelected)
    {
        if (selectedFrame != null)
        {
            selectedFrame.gameObject.SetActive(isSelected);
        }
    }
    public void ClearSlot()
    {
        fadeTween?.Kill();
        itemIcon.enabled = false;
        itemIcon.sprite = null;
        itemIcon.color = new Color(255f / 255f, 215f / 255f, 168f / 255f, 1f);
        glowEffect.shadowMode = ShadowMode.None;
        stackCountText.text = "";
        currentItem = null;
        currentItemAmount = 0;
        SetHighLight(false);
    }

    public void UpdateSlot(ItemData item, int stack, float delay = 0f)
    {
        if (item == null)
        {
            ClearSlot();
            return;
        }
        ClearSlot();
        currentItem = item;
        currentItemAmount = stack;
        itemIcon.enabled = true;
        itemIcon.sprite = item.Icon;
        glowEffect.shadowMode = ShadowMode.Outline8;
        glowEffect.shadowColor = InventoryUIManager.Instance.GetRarityColor(item.Rarity);
        glowEffect.shadowIteration = 5;
        itemIcon.color = new Color(1f, 1f, 1f, 0f);
        itemIcon.transform.localPosition = defaultTransform + Vector3.down * floatUpAmount;
        Sequence entrance = DOTween.Sequence();
        entrance.PrependInterval(delay);
        entrance.Join(itemIcon.DOFade(1f, fadeInDuration).SetEase(Ease.OutCubic));
        entrance.Join(itemIcon.transform
            .DOLocalMoveY(defaultTransform.y, fadeInDuration)
            .SetEase(Ease.OutCubic));
        fadeTween = entrance;
        stackCountText.text = stack > 0 ? $"{stack:N0}" : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (currentItem == null) return;
            InventoryUIManager.Instance.OnClicked(this);
        }
    }
}
