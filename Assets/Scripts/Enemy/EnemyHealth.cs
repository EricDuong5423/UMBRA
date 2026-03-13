using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private EnemyStatsManager enemyStats;
    [SerializeField] private DamageText damageTextPrefab;
    [SerializeField] private Transform damageTextParent;

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

    public override void TakeDamage(float amount, Transform source)
    {
        base.TakeDamage(amount, source);
        DamageTextManager.Instance.SpawnDamageText($"{amount}", Color.red, transform);
    }

    public override void TakeDoTDamage(float amount)
    {
        base.TakeDoTDamage(amount);
        DamageTextManager.Instance.SpawnDamageText($"{amount}", Color.green, transform);
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