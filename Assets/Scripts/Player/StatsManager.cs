using System;
using UnityEngine;
using Random = System.Random;

public class StatsManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerStats baseStats; 

    [Header("Equipment Bonuses")] 
    public float bonusMaxEmbers = 0;
    public float bonusMoveSpeed = 0;
    public float bonusAttackDamage = 0;
    public float bonusMaxStamina = 0;
    public float bonusStaminaRegen = 0;
    public float bonusCritRate = 0;
    public float bonusCritDamage = 0;
    
    private float _maxEmbers;
    private float _moveSpeed;
    private float _attackDamage;
    private float _maxStamina;
    private float _staminaRegen;
    private float _critRate;
    private float _critDamage;

    public float MaxEmbers => _maxEmbers;
    public float MoveSpeed => _moveSpeed;
    public float AttackDamage => _attackDamage;
    
    public float CritRate => _critRate;
    
    public float CritDamage => _critDamage;

    public float MaxStamina => _maxStamina;
    public float StaminaRegen => _staminaRegen;
    
    public PlayerStats BaseStats => baseStats;

    public float GetCalculatedHitDamage()
    {
        float randomChance = UnityEngine.Random.Range(0f, 100f);
        if (randomChance <= _critRate)
        {
            return _attackDamage * (1f + _critDamage);
        }
        return _attackDamage;
    }

    public event Action OnStatsChange;

    private void Awake()
    {
        RecalculateStats();
    }
    public void RecalculateStats()
    {
        if (baseStats == null) return;
        _maxEmbers = baseStats.baseMaxEmbers + bonusMaxEmbers;
        _moveSpeed = baseStats.baseMoveSpeed + bonusMoveSpeed;
        _attackDamage = baseStats.baseAtkDamage + bonusAttackDamage;
        _maxStamina = baseStats.baseMaxStamina + bonusMaxStamina;
        _staminaRegen = baseStats.baseStaminaRegen + bonusStaminaRegen;
        _critRate = baseStats.critRate + bonusCritRate;
        _critDamage = baseStats.critDamage + bonusCritDamage;
        OnStatsChange?.Invoke();
    }
    
    // Bonus stats add functions

    public void AddDamageModifier(float amount)
    {
        bonusAttackDamage += amount;
        RecalculateStats();
    }

    public void AddCritRateModifier(float amount)
    {
        bonusCritRate += amount;
        RecalculateStats();
    }

    public void AddCritDamageModifier(float amount)
    {
        bonusCritDamage += amount;
        RecalculateStats();
    }

    public void AddStaminaModifier(float amount)
    {
        bonusMaxStamina += amount;
        RecalculateStats();
    }

    public void AddMaxEmbersModifier(float amount)
    {
        bonusMaxEmbers += amount;
        RecalculateStats();
    }

    public void AddMoveSpeedModifier(float amount)
    {
        bonusMoveSpeed += amount;
        RecalculateStats();
    }

    public void AddStaminaRegenModifier(float amount)
    {
        bonusStaminaRegen += amount;
        RecalculateStats();
    }
}