using System;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    // Sự kiện
    public event Action<float, float> OnStaminaChanged;

    // Getters
    public float CurrentStamina => currentStamina;
    public float MaxStamina => stats != null ? stats.MaxStamina : 100f;

    private StatsManager stats;
    private float currentStamina;

    private void Awake()
    {
        stats = GetComponent<StatsManager>();

        // 1. KHỞI TẠO DỮ LIỆU (Để tránh lỗi = 0 lúc đầu)
        if (stats != null)
        {
            currentStamina = stats.MaxStamina;
        }
    }

    private void Start()
    {
        // 2. ĐĂNG KÝ SỰ KIỆN LEVEL UP
        if (stats != null)
        {
            stats.OnLevelChanged += HandleLevelUp;
        }

        // 3. THÔNG BÁO RA UI (Broadcast)
        BroadcastStamina();
    }
    
    private void OnDestroy() 
    {
        if(stats) stats.OnLevelChanged -= HandleLevelUp;
    }

    private void Update()
    {
        // LOGIC HỒI PHỤC TỰ ĐỘNG
        if (stats != null && currentStamina < MaxStamina)
        {
            currentStamina += stats.StaminaRegen * Time.deltaTime;
            
            // Đảm bảo không vượt quá Max
            if (currentStamina > MaxStamina) currentStamina = MaxStamina;
            
            BroadcastStamina(); // Cập nhật liên tục khi đang hồi
        }
    }

    public bool TryConsumeStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            BroadcastStamina(); // Cập nhật ngay khi dùng
            return true;
        }
        return false;
    }

    private void HandleLevelUp()
    {
        currentStamina = MaxStamina; // Hồi đầy thể lực
        BroadcastStamina();
    }

    // Hàm Broadcast tập trung
    private void BroadcastStamina()
    {
        OnStaminaChanged?.Invoke(currentStamina, MaxStamina);
    }
}