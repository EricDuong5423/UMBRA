using System;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    public event Action<float, float> OnStaminaChanged;

    public float CurrentStamina => currentStamina;
    public float MaxStamina => stats != null ? stats.MaxStamina : 100f;

    private StatsManager stats;
    private float currentStamina;

    private void Awake()
    {
        stats = GetComponent<StatsManager>();
        
        // XÓA đoạn gán currentStamina ở đây đi
        // Vì lúc này stats có thể chưa tính toán xong
    }

    private void Start()
    {
        // CHUYỂN VIỆC LẤY CHỈ SỐ XUỐNG ĐÂY
        // Lý do: Hàm Start() luôn chạy sau khi TẤT CẢ hàm Awake() đã chạy xong.
        // Đảm bảo StatsManager đã tính toán xong MaxStamina.
        if (stats != null)
        {
            currentStamina = stats.MaxStamina; 
            stats.OnLevelChanged += HandleLevelUp;
        }

        BroadcastStamina();
    }
    
    // ... (Các phần còn lại giữ nguyên) ...
    
    private void OnDestroy() 
    {
        if(stats) stats.OnLevelChanged -= HandleLevelUp;
    }

    private void Update()
    {
        if (stats != null && currentStamina < MaxStamina)
        {
            currentStamina += stats.StaminaRegen * Time.deltaTime;
            if (currentStamina > MaxStamina) currentStamina = MaxStamina;
            BroadcastStamina();
        }
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

    private void HandleLevelUp()
    {
        currentStamina = MaxStamina;
        BroadcastStamina();
    }

    private void BroadcastStamina()
    {
        OnStaminaChanged?.Invoke(currentStamina, MaxStamina);
    }
}