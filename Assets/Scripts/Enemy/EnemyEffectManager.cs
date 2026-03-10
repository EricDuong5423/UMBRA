using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectManager : BaseEffectManager
{
    private EnemyStatsManager enemyStats;

    protected override void Awake()
    {
        base.Awake();
        enemyStats = GetComponent<EnemyStatsManager>();
    }
}
