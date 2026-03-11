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
        var dmgText = Instantiate(damageTextPrefab
                                , damageTextParent.position
                                , Quaternion.identity);
        dmgText.transform.SetParent(damageTextParent);
        dmgText.SetData($"{amount}", Color.red);
    }

    public override void TakeDoTDamage(float amount)
    {
        base.TakeDoTDamage(amount);
        
        var dmgText = Instantiate(damageTextPrefab
            , damageTextParent.position
            , Quaternion.identity);
        dmgText.transform.SetParent(damageTextParent);
        dmgText.SetData($"{amount}", Color.red);
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