using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private EnemyBase enemyBase;
    [SerializeField] private DamageText damageTextPrefab;
    [SerializeField] private Transform damageTextParent;

    public void Initialize(EnemyBase manager)
    {
        enemyBase = manager;
        InitializeHealth(enemyBase.StatsManager.MaxEmbers);
        enemyBase.StatsManager.OnStatsChange -= HandleStatsChanged;
        enemyBase.StatsManager.OnStatsChange += HandleStatsChanged;
    }

    private void OnDestroy()
    {
        if (enemyBase != null && enemyBase.StatsManager != null)
            enemyBase.StatsManager.OnStatsChange -= HandleStatsChanged;
    }

    public override void TakeDamage(float amount, Transform source, bool isCrit, float knockbackForce)
    {
        float finalDamage = enemyBase.StatsManager.GetDamageTaken(amount);
        DamageTextManager.Instance.SpawnDamageText($"{Mathf.RoundToInt(finalDamage)}", isCrit ? Color.red: Color.white, transform, isCrit);
        base.TakeDamage(finalDamage, source, isCrit, knockbackForce);
    }

    public override void TakeDoTDamage(float amount)
    {
        base.TakeDoTDamage(amount);
        DamageTextManager.Instance.SpawnDamageText($"{amount}", Color.green, transform, false);
    }

    private void HandleStatsChanged()
    {
        UpdateMaxHealth(enemyBase.StatsManager.MaxEmbers);
    }
}