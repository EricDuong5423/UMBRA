using System;
using UnityEngine;

public class EnemyStatsManager : MonoBehaviour, IEnemyComponent
{
    private EnemyBase brain; // Bộ não trung tâm

    [Header("Data")]
    [SerializeField] private EnemyStats baseStats; 
    [SerializeField] private LootTable lootTable;
    
    public LootTable LootTable => lootTable;

    [Header("Buffs & Debuffs (Runtime)")] 
    public float bonusMaxEmbers = 0;
    public float bonusMoveSpeed = 0;
    public float bonusAttackDamage = 0;
    
    // --- CACHED VALUES ---
    private float _maxEmbers;
    private float _moveSpeed;
    private float _attackDamage;

    // --- PUBLIC GETTERS ---
    public float MaxEmbers => _maxEmbers;
    public float MoveSpeed => _moveSpeed;
    public float AttackDamage => _attackDamage;
    
    public EnemyStats BaseStats => baseStats;

    public event Action OnStatsChange;
    public void Initialize(EnemyBase brain)
    {
        this.brain = brain;
        bonusMaxEmbers = 0;
        bonusMoveSpeed = 0;
        bonusAttackDamage = 0;

        RecalculateStats();
    }

    public void RecalculateStats()
    {
        if (baseStats == null) return;
        _maxEmbers = baseStats.baseMaxEmbers + bonusMaxEmbers;
        _moveSpeed = baseStats.baseMoveSpeed + bonusMoveSpeed;
        _attackDamage = baseStats.baseAtkDamage + bonusAttackDamage;
        OnStatsChange?.Invoke();
    }

    public void AddDamageModifier(float amount)
    {
        bonusAttackDamage += amount;
        RecalculateStats();
    }

    public void AddMoveSpeedModifier(float amount)
    {
        bonusMoveSpeed += amount;
        RecalculateStats();
    }

    public void AddEmbersModifier(float amount)
    {
        bonusMaxEmbers += amount;
        RecalculateStats();
    }
}