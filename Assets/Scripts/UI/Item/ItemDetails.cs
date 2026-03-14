using UnityEngine;

public class ItemDetails : MonoBehaviour
{
    [SerializeField] private ItemDetailUIImage itemDetailUIImage;
    [SerializeField] private ItemGeneralInfo  itemGeneralInfo;
    [SerializeField] private ItemDescriptionUI itemDescriptionUI;
    [SerializeField] private ItemBonusStatsUI itemBonusStatsUI;
    [SerializeField] private ItemEffectUI itemEffectUI;

    public void Setup(ItemData item, int amount)
    {
        itemDetailUIImage.Setup(item, amount);
        itemGeneralInfo.Setup(item, amount);
        itemDescriptionUI.Setup(item, amount);
        itemBonusStatsUI.Setup(item, amount);
        itemEffectUI.Setup(item, amount);
    }
}
