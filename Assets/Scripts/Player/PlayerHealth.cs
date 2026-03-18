using UnityEngine;
using System.Collections;

public class PlayerHealth : EntityHealth
{
    [Header("Player Settings")]
    public bool isInvincible = false; // fix: instance thay vì static

    private PlayerStatsManager playerStatsManager;

    public void Initialize(PlayerStatsManager statsManager)
    {
        playerStatsManager = statsManager;
        isInvincible = false;
        InitializeHealth(playerStatsManager.MaxEmbers);
        playerStatsManager.OnStatsChange += HandleStatsChanged;
    }

    private void OnDestroy()
    {
        if (playerStatsManager != null)
            playerStatsManager.OnStatsChange -= HandleStatsChanged;
    }

    private void HandleStatsChanged()
    {
        UpdateMaxHealth(playerStatsManager.MaxEmbers);
    }

    public override void TakeDamage(float amount, Transform source, bool _, float knockbackForce)
    {
        if (isInvincible || IsDead) return;
        float finalDamage = playerStatsManager.GetDamageTaken(amount);
        base.TakeDamage(finalDamage, source, _);
        ItemManager inventory = GetComponent<ItemManager>();
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
    
    public void EnableIFrame()
    {
        isInvincible = true;
    }
    
    public void DisableIFrame()
    {
        isInvincible = false;
    }
}