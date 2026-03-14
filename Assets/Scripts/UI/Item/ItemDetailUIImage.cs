using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUIImage : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private UIEffect itemIconEffect;
    [SerializeField] private Color defaultColor;

    public void Setup(ItemData item, int amount)
    {
        if (item == null)
        {
            Clear();
            return;
        }
        Clear();
        itemIcon.sprite = item.Icon;
        itemIcon.color = Color.white;
        itemIconEffect.shadowMode = ShadowMode.Outline8;
        itemIconEffect.shadowColor = InventoryUIManager.Instance.GetRarityColor(item.Rarity);
        itemIconEffect.shadowIteration = 5;
    }

    public void Clear()
    {
        itemIcon.sprite = null;
        itemIcon.color = defaultColor;
        itemIconEffect.shadowMode = ShadowMode.None;
    }
}
