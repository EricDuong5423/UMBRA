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

    // --- CACHED VALUES (Biến lưu đệm) ---
    private float _maxEmbers;
    private float _moveSpeed;
    private float _attackDamage;
    private float _maxStamina;
    private float _staminaRegen;

    // Public Getters lấy giá trị từ Cache (Siêu nhanh)
    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => expToNextLevel;
    
    public float MaxEmbers => _maxEmbers;
    public float MoveSpeed => _moveSpeed;
    public float AttackDamage => _attackDamage;
    public float MaxStamina => _maxStamina;
    public float StaminaRegen => _staminaRegen;
    
    public EntitesStats BaseStats => baseStats;

    // Events
    public event Action OnLevelChanged;
    public event Action<int, int, int> OnExpChanged;

    private void Awake()
    {
        RecalculateStats();
    }
    private void Start()
    {
        BroadcastExp();
    }

    // Hàm tính toán chỉ số (Chỉ chạy 1 lần khi Start hoặc LevelUp)
    private void RecalculateStats()
    {
        if (baseStats == null) return;
        
        _maxEmbers = CalculateStat(baseStats.baseMaxEmbers, baseStats.healthGrowth);
        _moveSpeed = CalculateStat(baseStats.baseMoveSpeed, baseStats.speedGrowth);
        _attackDamage = CalculateStat(baseStats.baseAtkDamage, baseStats.damageGrowth);
        _maxStamina = CalculateStat(baseStats.baseMaxStamina, baseStats.staminaGrowth);
        _staminaRegen = CalculateStat(baseStats.baseStaminaRegen, baseStats.staminaRegenGrowth);
    }

    private float CalculateStat(float baseVal, float growth)
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
        while (currentExp >= expToNextLevel)
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
        
        RecalculateStats();
        
        OnLevelChanged?.Invoke();
        OnExpChanged?.Invoke(currentLevel, currentExp, expToNextLevel);
    }

    [ContextMenu("Test Level Up")]
    public void TestLevelUp() => AddExperience(expToNextLevel);
}