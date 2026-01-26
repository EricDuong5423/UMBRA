using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerVisuals : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Light2D playerLight;
    [SerializeField] private Animator animator;
    
    private PlayerStats playerStats; 
    private HealthSystem healthSystem;

    public void Initialize(PlayerStats stats, HealthSystem health)
    {
        playerStats = stats;
        healthSystem = health;
        healthSystem.OnHealthChanged += UpdateVisuals;
        UpdateVisuals(healthSystem.CurrentEmbers, healthSystem.MaxEmbers);UpdateVisuals(healthSystem.CurrentEmbers, healthSystem.MaxEmbers);
    }

    private void OnDestroy()
    {
        if(healthSystem) healthSystem.OnHealthChanged -= UpdateVisuals;
    }

    // --- SỬA LẠI HÀM NÀY ---
    public void UpdateMovementAnim(Vector2 direction, bool isMoving)
    {
        if (animator == null) return;
    
        int x = Mathf.RoundToInt(direction.x);
        int y = Mathf.RoundToInt(direction.y);

        animator.SetInteger("X", x);
        animator.SetInteger("Y", y);

        animator.SetBool("IsMoving", isMoving);
    }

    // ... (Giữ nguyên các hàm TriggerRoll, TriggerAttack, UpdateVisuals cũ)
    public void TriggerRoll(Vector2 direction)
    {
        if (animator == null) return;
        animator.SetInteger("X", Mathf.RoundToInt(direction.x));
        animator.SetTrigger("Roll");
    }

    public void TriggerAttack()
    {
        if (animator == null) return;
        animator.SetTrigger("Attack");
    }
    
    private void UpdateVisuals(float current, float max)
    {
        // 1. Kiểm tra an toàn (Null Check)
        if (playerStats == null) return;

        // 2. Tính tỷ lệ máu (Ratio)
        // Kẹp giá trị từ 0 đến 1 để tránh lỗi khi máu < 0 hoặc > max
        float ratio = 0f;
        if (max > 0)
        {
            ratio = Mathf.Clamp01(current / max);
        }

        // 3. Xử lý Đèn (Light 2D)
        // Máu càng ít -> Đèn càng tối và bán kính càng nhỏ
        if (playerLight != null)
        {
            playerLight.intensity = Mathf.Lerp(playerStats.minLightIntensity, playerStats.maxLightIntensity, ratio);
            playerLight.pointLightOuterRadius = Mathf.Lerp(playerStats.minLightRadius, playerStats.maxLightRadius, ratio);
        }

        // 4. Xử lý Màu Sắc (Sprite Renderer)
        // Máu càng ít -> Màu chuyển dần về SilhouetteColor (đen/tối)
        // Máu đầy -> Màu là LightColor (thường là trắng/gốc)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(playerStats.silhouetteColor, playerStats.lightColor, ratio);
        }

        // 5. Xử lý Animator
        // Cập nhật biến HEALTH để Animator chuyển state (VD: chuyển sang animation thở dốc khi máu thấp)
        if (animator != null)
        {
            animator.SetInteger("HEALTH", Mathf.RoundToInt(current));
        }
    }
}