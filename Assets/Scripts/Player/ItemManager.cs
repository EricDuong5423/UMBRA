using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    private StatsManager statsManager;
    public event Action<ItemData, int> OnInvetoryChanged;
    
    [field:SerializeField] public List<ItemData> randomItemsDebug = new List<ItemData>();

    private void Awake()
    {
        statsManager = GetComponent<StatsManager>();
    }

    public void AddItem(ItemData item)
    {
        if(item == null) return;
        if (inventory.ContainsKey(item))
        {
            inventory[item]++;
        }
        else
        {
            inventory.Add(item, 1);
        }
        
        ApplyAllItemStats();
        OnInvetoryChanged?.Invoke(item, inventory[item]);
    }

    private void ApplyAllItemStats()
    {
        statsManager.bonusAttackDamage = 0;
        statsManager.bonusCritDamage = 0;
        statsManager.bonusCritRate = 0;
        statsManager.bonusStaminaRegen = 0;
        statsManager.bonusMaxEmbers = 0;
        statsManager.bonusMaxStamina = 0;
        statsManager.bonusMoveSpeed = 0;
        foreach (var kvp in inventory)
        {
            ItemData item = kvp.Key;
            int count =  kvp.Value;
            
            statsManager.bonusAttackDamage += item.bonusAttackDmg * count;
            statsManager.bonusCritDamage += item.bonusCritDamage * count;
            statsManager.bonusCritRate += item.bonusCritRate * count;
            statsManager.bonusStaminaRegen += item.bonusStaminaRegen * count;
            statsManager.bonusMaxEmbers += item.bonusMaxEmbers * count;
            statsManager.bonusMaxStamina += item.bonusStamina * count;
            statsManager.bonusMoveSpeed += item.bonusMoveSpeed * count;
        }
        statsManager.RecalculateStats();
    }
}
