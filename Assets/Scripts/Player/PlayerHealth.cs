using System;
using UnityEngine;
using System.Collections;

public class PlayerHealth : EntityHealth
{
    [Header("Player Settings")]
    public bool isInvincible = false;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) TakeDamage(MaxEmbers, transform, false, 0f);
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

    public void GameOver()
    {
        GameManager.Instance.SetState(GameManager.GameState.GameOver);
    }
    
    public void Ressurect()
    {
        IsDead = false;
        isInvincible = false;
        PlayerManager.Instance.PlayerStamina.ResetStamina();
        PlayerManager.Instance.PlayerItemManager.RemoveAllItems();
        PlayerManager.Instance.PlayerCoinSystem.ResetCoins();
        Heal(PlayerManager.Instance.PlayerStatsManager.BaseStats.BaseMaxEmbers);
        PlayerManager.Instance.PlayerVisuals.ResetDeathAnimation();
        PlayerManager.Instance.PlayerController.ResetState();
    }
}