using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance;
    
    [Header("Settings")]
    [SerializeField] private ItemPickUp itemPickUpPrefab;
    private void Awake()
    {
        if (Instance != null || Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void TryDropItem(Vector3 dropPosition, LootTable specificLootTable)
    {
        if (Random.Range(0, 100f) > specificLootTable.dropChance) return;
        ItemRarity rarity = GetRandomRarity(specificLootTable);
        
        List<ItemData> availableItems = specificLootTable.itemsToDrop.Where(item => item.Rarity == rarity).ToList();

        if (availableItems.Count == 0)
        {
            availableItems = specificLootTable.itemsToDrop;
        }
        ItemData itemToDrop = availableItems[Random.Range(0, availableItems.Count)];
        ItemPickUp droppedItem = Instantiate(itemPickUpPrefab, dropPosition, Quaternion.identity);
        droppedItem.Setup(itemToDrop);
    }

    private ItemRarity GetRandomRarity(LootTable specificLootTable)
    {
        float totalWeight = specificLootTable.commonWeight 
                            + specificLootTable.uncommonWeight 
                            + specificLootTable.rareWeight 
                            + specificLootTable.epicWeight 
                            + specificLootTable.legendaryWeight;
        float randomVal =  Random.Range(0, totalWeight);
        if(randomVal <= specificLootTable.commonWeight) return ItemRarity.Common;
        randomVal -= specificLootTable.commonWeight;
        if(randomVal <= specificLootTable.uncommonWeight) return ItemRarity.Uncommon;
        randomVal -= specificLootTable.uncommonWeight;
        if(randomVal <= specificLootTable.rareWeight) return ItemRarity.Rare;
        randomVal -= specificLootTable.rareWeight;
        if(randomVal <= specificLootTable.epicWeight) return ItemRarity.Epic;
        return ItemRarity.Legendary;
    }
}
