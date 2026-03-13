using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectManager : BaseEffectManager
{
    private EnemyBase enemyBase;
    private EnemyStatsManager enemyStats;
    protected override void Awake() { }

    public void Initialize(EnemyBase manager)
    {
        enemyBase = manager;
        enemyStats = enemyBase.StatsManager;
        
        healthSystem = enemyBase.HealthSystem;
        
        if (activeEffects != null) activeEffects.Clear();
    }
}