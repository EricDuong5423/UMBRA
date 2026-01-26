using UnityEngine;
using UnityEngine.UI;

// Class này sẽ làm móng cho cả Health và Stamina
public abstract class ResourceBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] protected Image barFill;

    // Hàm này được các con gọi khi chỉ số thay đổi
    protected void UpdateView(float current, float max)
    {
        // 1. Tính toán Ratio
        float ratio = 0f;
        if (max > 0)
        {
            ratio = Mathf.Clamp01(current / max);
        }
        
        barFill.fillAmount = ratio;
    }
}