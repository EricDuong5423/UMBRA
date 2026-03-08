using UnityEngine;
using System.Collections;

public class PlayerHealth : EntityHealth
{
    [Header("Player Settings")]
    [SerializeField] private float iFrameDuration = 0.5f;
    public static bool isInvincible = false;

    private StatsManager stats;

    private void Awake()
    {
        stats = GetComponent<StatsManager>();
    }

    private void Start()
    {
        if (stats != null)
        {
            InitializeHealth(stats.MaxEmbers);
            stats.OnStatsChange += HandleStatsChanged;
        }
    }

    private void OnDestroy()
    {
        if (stats != null) stats.OnStatsChange -= HandleStatsChanged;
    }

    private void HandleStatsChanged()
    {
        UpdateMaxHealth(stats.MaxEmbers);
    }

    public override void TakeDamage(float amount, Transform source)
    {
        if (isInvincible || IsDead) return;
        base.TakeDamage(amount, source);
        ItemManager inventory =  GetComponent<ItemManager>();
        if (!inventory) return;
        inventory.TriggerOnPlayerTakeDamageEffect(amount);
        
        if (!IsDead) StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(iFrameDuration);
        isInvincible = false;
    }
}