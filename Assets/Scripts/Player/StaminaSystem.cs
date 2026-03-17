using System;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    public event Action<float, float> OnStaminaChanged;

    public float CurrentStamina => currentStamina;
    public float MaxStamina => playerStatsManager != null ? playerStatsManager.MaxStamina : 100f;

    private PlayerStatsManager playerStatsManager;
    private float currentStamina;

    public void Initialize(PlayerStatsManager statsManager)
    {
        playerStatsManager = statsManager;
        currentStamina = playerStatsManager.MaxStamina;
        playerStatsManager.OnStatsChange += HandleStatsChanged;
        BroadcastStamina();
    }

    private void Start()
    {
        // fix: bỏ double-subscribe, chỉ broadcast
        BroadcastStamina();
    }

    private void OnDestroy()
    {
        if (playerStatsManager != null)
            playerStatsManager.OnStatsChange -= HandleStatsChanged;
    }

    private void Update()
    {
        if (playerStatsManager == null || currentStamina >= MaxStamina) return;
        currentStamina += playerStatsManager.StaminaRegen * Time.deltaTime;
        if (currentStamina > MaxStamina) currentStamina = MaxStamina;
        BroadcastStamina();
    }

    public bool TryConsumeStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            BroadcastStamina();
            return true;
        }
        return false;
    }

    private void HandleStatsChanged()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, playerStatsManager.MaxStamina);
        BroadcastStamina();
    }

    private void BroadcastStamina()
    {
        OnStaminaChanged?.Invoke(currentStamina, MaxStamina);
    }
}