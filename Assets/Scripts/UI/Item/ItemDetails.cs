using UnityEngine;

public class ItemDetails : MonoBehaviour
{
    [SerializeField] private ItemDetailUIImage itemDetailUIImage;
    [SerializeField] private ItemGeneralInfo  itemGeneralInfo;
    [SerializeField] private ItemBonusStatsUI itemBonusStatsUI;
    [SerializeField] private TypewriterTextUI itemDescriptionUI;
    [SerializeField] private TypewriterTextUI itemEffectUI;

    public void Setup(ItemData item, int amount)
    {
        itemDetailUIImage.Setup(item, amount);
        itemGeneralInfo.Setup(item, amount);
        itemBonusStatsUI.Setup(item, amount);
        itemDescriptionUI.Play(item.Description);
        itemEffectUI.Play(item.effects?.Description);
    }
}
