using UnityEngine;

public class HealthBarUI : ResourceBarUI
{
    [Header("Data Connection")]
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        if (healthSystem != null)
        {
            // Đăng ký sự kiện
            healthSystem.OnHealthChanged += UpdateView;
            
            // Cập nhật ngay lập tức (để tránh lúc đầu game thanh máu bị rỗng/sai màu)
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