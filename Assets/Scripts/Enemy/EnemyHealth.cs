using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private EnemyStatsManager enemyStats;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStatsManager>();
    }

    private void Start()
    {
        if (enemyStats != null)
        {
            InitializeHealth(enemyStats.MaxEmbers);
            enemyStats.OnStatsChange += HandleStatsChanged;
        }
    }

    private void OnDestroy()
    {
        if (enemyStats != null) enemyStats.OnStatsChange -= HandleStatsChanged;
    }

    protected override void Die()
    {
        base.Die();
    }

    private void HandleStatsChanged()
    {
        UpdateMaxHealth(enemyStats.MaxEmbers);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}