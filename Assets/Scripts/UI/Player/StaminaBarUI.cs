using UnityEngine;

public class StaminaBarUI : ResourceBarUI
{
    [Header("Data Connection")]
    [SerializeField] private StaminaSystem staminaSystem;

    private void Start()
    {
        staminaSystem = PlayerManager.Instance.PlayerStamina;
        if (staminaSystem != null)
        {
            staminaSystem.OnStaminaChanged += UpdateView;
            UpdateView(staminaSystem.CurrentStamina, staminaSystem.MaxStamina);
        }
    }

    private void OnDestroy()
    {
        if (staminaSystem != null)
        {
            staminaSystem.OnStaminaChanged -= UpdateView;
        }
    }
}