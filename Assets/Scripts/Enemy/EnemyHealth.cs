using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private EnemyStatsManager enemyStats;

    private void Awake()
    {
        // Tìm StatsManager của riêng Quái
        enemyStats = GetComponent<EnemyStatsManager>();
    }

    private void Start()
    {
        if (enemyStats != null)
        {
            // Bơm máu gốc dựa theo EnemyStatsManager
            InitializeHealth(enemyStats.MaxEmbers);
            enemyStats.OnStatsChange += HandleStatsChanged;
        }
    }

    private void OnDestroy()
    {
        if (enemyStats != null) enemyStats.OnStatsChange -= HandleStatsChanged;
    }

    private void HandleStatsChanged()
    {
        UpdateMaxHealth(enemyStats.MaxEmbers);
    }

    protected override void Die()
    {
        base.Die();
        
        Destroy(gameObject); 
    }
}