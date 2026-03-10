using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image borderImage;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackCountText;

    public void Setup(ItemData item, int count)
    {
        if (item.Icon != null) itemIcon.sprite = item.Icon;
        if (borderImage != null) borderImage.color = GetColorByRarity(item.Rarity);

        if (count > 1)
        {
            stackCountText.text = "x" + count;
            stackCountText.gameObject.SetActive(true);
        }
        else
        {
            stackCountText.gameObject.SetActive(false);
        }
    }

    private Color GetColorByRarity(ItemRarity itemRarity)
    {
        switch (itemRarity)
        {
            case ItemRarity.Common: return Color.gray;
            case ItemRarity.Uncommon: return Color.green;
            case ItemRarity.Rare: return new Color(0f, 0.5f, 1f);
            case ItemRarity.Epic: return new  Color(0.6f, 0.1f, 0.9f);
            case ItemRarity.Legendary: return Color.red;
            default: return Color.white;
        }
    }
}
