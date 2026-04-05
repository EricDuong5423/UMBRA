using UnityEngine;

public class HealthBarUI : ResourceBarUI
{
    [Header("Data Connection")]
    // Đổi từ HealthSystem sang EntityHealth
    [SerializeField] private PlayerHealth healthSystem;

    private void Start()
    {
        healthSystem = PlayerManager.Instance.PlayerHealth;
        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged += UpdateView;
            UpdateView(healthSystem.CurrentEmbers, healthSystem.MaxEmbers);
        }
    }

    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged -= UpdateView;
        }
    }
}