using System;
using UnityEngine;

public class EnemyStatsManager : MonoBehaviour
{
    private EnemyBase enemyBase;
    private EnemyStats baseStats; // không còn [SerializeField]

    public float bonusMaxEmbers = 0;
    public float bonusMoveSpeed = 0;
    public float bonusAttackDamage = 0;
    public float bonusMaxStamina = 0;
    public float bonusStaminaRegen = 0;
    public float bonusCritRate = 0;
    public float bonusCritDamage = 0;
    public float bonusArmor = 0;

    private float _maxEmbers;
    private float _moveSpeed;
    private float _attackDamage;
    private float _maxStamina;
    private float _staminaRegen;
    private float _critRate;
    private float _critDamage;
    private float _armor;

    public float MaxEmbers => _maxEmbers;
    public float MoveSpeed => _moveSpeed;
    public float AttackDamage => _attackDamage;
    public float CritRate => _critRate;
    public float CritDamage => _critDamage;
    public float MaxStamina => _maxStamina;
    public float StaminaRegen => _staminaRegen;
    public float Armor => _armor;
    public EnemyStats BaseStats => baseStats;

    public event Action OnStatsChange;

    public void Initialize(EnemyBase manager)
    {
        enemyBase = manager;
        baseStats = manager.Stats;
        bonusMaxEmbers = bonusMoveSpeed = bonusAttackDamage = 0;
        bonusMaxStamina = bonusStaminaRegen = bonusCritRate = 0;
        bonusCritDamage = bonusArmor = 0;
        RecalculateStats();
    }

    // Dùng cho procedural spawn — scale stats theo floor/room
    public void SetBaseStats(EnemyStats newStats)
    {
        baseStats = newStats;
        RecalculateStats();
    }

    public void RecalculateStats()
    {
        if (baseStats == null) return;
        _maxEmbers    = baseStats.BaseMaxEmbers    + bonusMaxEmbers;
        _moveSpeed    = baseStats.BaseMoveSpeed    + bonusMoveSpeed;
        _attackDamage = baseStats.BaseAtkDamage    + bonusAttackDamage;
        _maxStamina   = baseStats.BaseMaxStamina   + bonusMaxStamina;
        _staminaRegen = baseStats.BaseStaminaRegen + bonusStaminaRegen;
        _critRate     = baseStats.BaseCritRate     + bonusCritRate;
        _critDamage   = baseStats.BaseCritDamage   + bonusCritDamage;
        _armor        = baseStats.BaseArmor        + bonusArmor;
        OnStatsChange?.Invoke();
    }

    public float GetDamageTaken(float rawDamage)
    {
        if (_armor < 0)
        {
            float amplifyMultiplier = 2f - (100f / (100f - _armor));
            return rawDamage * amplifyMultiplier;
        }
        return rawDamage * (100f / (100f + _armor));
    }

    public float GetCalculatedHitDamage()
    {
        float randomChance = UnityEngine.Random.Range(0f, 100f);
        return randomChance <= _critRate ? _attackDamage * (1f + _critDamage) : _attackDamage;
    }

    public void AddDamageModifier(float amount)      { bonusAttackDamage += amount;  RecalculateStats(); }
    public void AddCritRateModifier(float amount)    { bonusCritRate += amount;       RecalculateStats(); }
    public void AddCritDamageModifier(float amount)  { bonusCritDamage += amount;     RecalculateStats(); }
    public void AddStaminaModifier(float amount)     { bonusMaxStamina += amount;     RecalculateStats(); }
    public void AddMaxEmbersModifier(float amount)   { bonusMaxEmbers += amount;      RecalculateStats(); }
    public void AddMoveSpeedModifier(float amount)   { bonusMoveSpeed += amount;      RecalculateStats(); }
    public void AddStaminaRegenModifier(float amount){ bonusStaminaRegen += amount;   RecalculateStats(); }
    public void AddArmorModifier(float amount)       { bonusArmor += amount;          RecalculateStats(); }
}