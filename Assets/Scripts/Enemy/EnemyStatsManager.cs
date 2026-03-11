using System;
using UnityEngine;

public class EnemyStatsManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private EnemyStats baseStats; 

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
        
        OnStatsChange?.Invoke();
    }
    
    // --- CÁC HÀM NHẬN DEBUFF TỪ PLAYER ---

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
}