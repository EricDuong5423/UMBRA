using UnityEngine;

public class HealthBarUI : ResourceBarUI
{
    [Header("Data Connection")]
    // Đổi từ HealthSystem sang EntityHealth
    [SerializeField] private EntityHealth healthSystem;

    private void Start()
    {
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