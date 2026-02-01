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
    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => expToNextLevel;
    
    public event Action OnLevelChanged;
    
    public event Action<int, int, int> OnExpChanged;
    
    public float MaxEmbers => GetStat(baseStats.baseMaxEmbers, baseStats.healthGrowth);
    public float MoveSpeed => GetStat(baseStats.baseMoveSpeed, baseStats.speedGrowth);
    public float AttackDamage => GetStat(baseStats.baseAtkDamage, baseStats.damageGrowth);
    public float MaxStamina => GetStat(baseStats.baseMaxStamina, baseStats.staminaGrowth);
    public float StaminaRegen => GetStat(baseStats.baseStaminaRegen, baseStats.staminaRegenGrowth);
    
    public EntitesStats BaseStats => baseStats;

    private void Start()
    {
        BroadcastExp();
    }

    private float GetStat(float baseVal, float growth)
    {
        return baseVal * Mathf.Pow(growth, currentLevel - 1);
    }

    private void BroadcastExp()
    {
        OnExpChanged?.Invoke(currentLevel, currentExp, expToNextLevel);
    }
    
    public void AddExperience(int amount)
    {
        currentExp += amount;
        if (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }
        BroadcastExp();
    }
    
    public void LevelUp()
    {
        currentLevel++;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f);
        OnLevelChanged?.Invoke();
        OnExpChanged?.Invoke(currentLevel, currentExp, expToNextLevel);
    }
}