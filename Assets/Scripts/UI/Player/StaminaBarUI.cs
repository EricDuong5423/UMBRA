using UnityEngine;

public class StaminaBarUI : ResourceBarUI
{
    [Header("Data Connection")]
    [SerializeField] private StaminaSystem staminaSystem;

    private void Start()
    {
        if (staminaSystem != null)
        {
            staminaSystem.OnStaminaChanged += UpdateView;
            
            // Nhờ bước 3 nên ta mới gọi được dòng này
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