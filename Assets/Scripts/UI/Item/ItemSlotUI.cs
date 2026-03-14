using Coffee.UIEffects;
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

    public void SetHighLight(bool isSelected)
    {
        if (selectedFrame != null)
        {
            selectedFrame.gameObject.SetActive(isSelected);
        }
    }
    public void ClearSlot()
    {
        itemIcon.enabled = false;
        itemIcon.sprite = null;
        itemIcon.color = new Color(255f / 255f, 215f / 255f, 168f / 255f, 1f);
        glowEffect.shadowMode = ShadowMode.None;
        stackCountText.text = "";
        currentItem = null;
        currentItemAmount = 0;
        SetHighLight(false);
    }

    public void UpdateSlot(ItemData item, int stack)
    {
        currentItem = item;
        currentItemAmount = stack;
        if (item == null)
        {
            ClearSlot();
            return;
        }
        itemIcon.enabled = true;
        itemIcon.sprite = item.Icon;
        glowEffect.shadowMode = ShadowMode.Outline8;
        glowEffect.shadowColor = InventoryUIManager.Instance.GetRarityColor(item.Rarity);
        glowEffect.shadowIteration = 5;
        itemIcon.color = Color.white;
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
