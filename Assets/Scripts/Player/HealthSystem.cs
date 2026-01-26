using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // Sự kiện
    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;
    
    // Getters
    public float CurrentEmbers => currentEmbers;
    public float MaxEmbers => stats != null ? stats.MaxEmbers : 100f;

    private StatsManager stats;
    private float currentEmbers;

    private void Awake()
    {
        stats = GetComponent<StatsManager>();
        
        // 1. KHỞI TẠO DỮ LIỆU (Chạy sớm nhất)
        if (stats != null)
        {
            currentEmbers = stats.MaxEmbers;
        }
    }

    private void Start()
    {
        // 2. ĐĂNG KÝ SỰ KIỆN
        if (stats != null)
        {
            stats.OnLevelChanged += HandleLevelUp;
        }
        
        // 3. THÔNG BÁO RA UI (Broadcast)
        // Dù UI chạy trước hay sau, việc gọi lại ở đây giúp đồng bộ lần cuối
        BroadcastHealth();
    }

    private void OnDestroy()
    {
        if (stats) stats.OnLevelChanged -= HandleLevelUp;
    }

    // --- LOGIC ---

    public void TakeDamage(float amount)
    {
        currentEmbers -= amount;
        currentEmbers = Mathf.Clamp(currentEmbers, 0, MaxEmbers);
        
        BroadcastHealth(); // Báo cập nhật

        if (currentEmbers <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        currentEmbers += amount;
        currentEmbers = Mathf.Clamp(currentEmbers, 0, MaxEmbers);
        BroadcastHealth(); // Báo cập nhật
    }

    private void HandleLevelUp()
    {
        currentEmbers = MaxEmbers; // Hồi đầy máu
        BroadcastHealth();
    }

    // Hàm Broadcast tập trung
    private void BroadcastHealth()
    {
        OnHealthChanged?.Invoke(currentEmbers, MaxEmbers);
    }
}