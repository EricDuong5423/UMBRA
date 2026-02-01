using UnityEngine;

public class HealthBarUI : ResourceBarUI
{
    [Header("Data Connection")]
    // Dùng EntityHealth để support cả PlayerHealth và EnemyHealth
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