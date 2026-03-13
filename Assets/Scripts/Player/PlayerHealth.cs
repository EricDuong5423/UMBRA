using UnityEngine;
using System.Collections;

public class PlayerHealth : EntityHealth
{
    [Header("Player Settings")]
    public static bool isInvincible = false;

    private PlayerStatsManager playerStatsManager;

    public void Initialize(PlayerStatsManager statsManager)
    {
        playerStatsManager = statsManager;
        InitializeHealth(playerStatsManager.MaxEmbers);
        playerStatsManager.OnStatsChange += HandleStatsChanged;
    }

    private void OnDestroy()
    {
        if (playerStatsManager != null) playerStatsManager.OnStatsChange -= HandleStatsChanged;
    }

    private void HandleStatsChanged()
    {
        UpdateMaxHealth(playerStatsManager.MaxEmbers);
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
        yield return new WaitForSeconds(playerStatsManager.BaseStats.InvicibleDuration);
        isInvincible = false;
    }
}