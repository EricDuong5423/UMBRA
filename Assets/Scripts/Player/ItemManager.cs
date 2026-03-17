using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    private PlayerStatsManager playerStatsManager;
    public event Action<ItemData, int> OnInvetoryChanged;
    
    [field:SerializeField] public List<ItemData> randomItemsDebug = new List<ItemData>();

    public void Initialize(PlayerStatsManager statsManager)
    {
        playerStatsManager = statsManager;
    }

    public void AddItem(ItemData item)
    {
        if(item == null) return;
        if (inventory.ContainsKey(item))
            inventory[item]++;
        else
            inventory.Add(item, 1);
        
        ApplyAllItemStats();
        OnInvetoryChanged?.Invoke(item, inventory[item]);
    }

    private void ApplyAllItemStats()
    {
        playerStatsManager.bonusAttackDamage = 0;
        playerStatsManager.bonusCritDamage = 0;
        playerStatsManager.bonusCritRate = 0;
        playerStatsManager.bonusStaminaRegen = 0;
        playerStatsManager.bonusMaxEmbers = 0;
        playerStatsManager.bonusMaxStamina = 0;
        playerStatsManager.bonusMoveSpeed = 0;
        playerStatsManager.bonusArmor = 0;

        foreach (var kvp in inventory)
        {
            ItemData item = kvp.Key;
            int count = kvp.Value;
            playerStatsManager.bonusAttackDamage += item.bonusAttackDmg * count;
            playerStatsManager.bonusCritDamage += item.bonusCritDamage * count;
            playerStatsManager.bonusCritRate += item.bonusCritRate * count;
            playerStatsManager.bonusStaminaRegen += item.bonusStaminaRegen * count;
            playerStatsManager.bonusMaxEmbers += item.bonusMaxEmbers * count;
            playerStatsManager.bonusMaxStamina += item.bonusStamina * count;
            playerStatsManager.bonusMoveSpeed += item.bonusMoveSpeed * count;
            playerStatsManager.bonusArmor += item.bonusArmor * count;
        }
        playerStatsManager.RecalculateStats();
    }

    public void TriggerOnHitEffects(IDamageable enemy, float damage)
    {
        foreach (var kvp in inventory)
        {
            if (kvp.Key.effects != null)
                kvp.Key.effects.OnHitEnemy(gameObject, enemy, damage, kvp.Value);
        }
    }

    public void TriggerOnKillEnemyEffect()
    {
        foreach (var kvp in inventory)
        {
            if (kvp.Key.effects != null)
                kvp.Key.effects.OnKillEnemy(gameObject, kvp.Value);
        }
    }

    public void TriggerOnPlayerTakeDamageEffect(float damageTaken)
    {
        foreach (var kvp in inventory)
        {
            if (kvp.Key.effects != null)
                kvp.Key.effects.OnPlayerTakeDamage(gameObject, damageTaken, kvp.Value);
        }
    }
}