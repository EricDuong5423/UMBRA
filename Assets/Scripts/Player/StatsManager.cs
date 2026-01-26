using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private EntitesStats baseStats; 

    [Header("Runtime")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentExp = 0;
    [SerializeField] private int expToNextLevel = 100;
    
    public event Action OnLevelChanged;
    
    public float MaxEmbers => GetStat(baseStats.baseMaxEmbers, baseStats.healthGrowth);
    public float MoveSpeed => GetStat(baseStats.baseMoveSpeed, baseStats.speedGrowth);
    public float AttackDamage => GetStat(baseStats.baseAtkDamage, baseStats.damageGrowth);
    public float MaxStamina => GetStat(baseStats.baseMaxStamina, baseStats.staminaGrowth);
    public float StaminaRegen => GetStat(baseStats.baseStaminaRegen, baseStats.staminaRegenGrowth);
    
    public EntitesStats BaseStats => baseStats; 

    private float GetStat(float baseVal, float growth)
    {
        return baseVal * Mathf.Pow(growth, currentLevel - 1);
    }
    
    public void AddExperience(int amount)
    {
        currentExp += amount;
        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    [ContextMenu("Test Level Up")]
    public void LevelUp()
    {
        currentLevel++;
        currentExp -= expToNextLevel;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f); // Tăng độ khó lên cấp
        OnLevelChanged?.Invoke(); // Báo cho Health/Stamina biết
    }
}