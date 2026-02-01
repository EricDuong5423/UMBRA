using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerVisuals : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Light2D playerLight;
    [SerializeField] private Animator animator;
    
    private PlayerStats playerStats; 
    private PlayerHealth playerHealth;

    // SỬA 1: Đổi int thành float (Mặc định nhìn phải = 1f)
    private float lastX = 1f; 

    public void Initialize(PlayerStats stats, PlayerHealth health)
    {
        playerStats = stats;
        playerHealth = health;

        // Đăng ký các sự kiện
        playerHealth.OnHealthChanged += UpdateVisuals;
        playerHealth.OnHit += TriggerHurtAnim;
        playerHealth.OnDeath += TriggerDeathAnim;

        // Cập nhật hiển thị lần đầu
        UpdateVisuals(playerHealth.CurrentEmbers, playerHealth.MaxEmbers);
    }

    private void OnDestroy()
    {
        if(playerHealth) 
        {
            playerHealth.OnHealthChanged -= UpdateVisuals;
            playerHealth.OnHit -= TriggerHurtAnim;
            playerHealth.OnDeath -= TriggerDeathAnim;
        }
    }

    // --- LOGIC ANIMATION ---

    public void UpdateMovementAnim(Vector2 direction, bool isMoving)
    {
        if (animator == null) return;
    
        // Gửi trạng thái di chuyển
        animator.SetBool("IsMoving", isMoving);

        // Chỉ cập nhật hướng khi thực sự di chuyển (để tránh quay về 0 khi đứng yên)
        if (isMoving && direction.x != 0)
        {
            // SỬA 2: Ép kiểu sang float (1f hoặc -1f)
            lastX = direction.x > 0 ? 1f : -1f;
            
            // SỬA 3: Dùng SetFloat thay vì SetInteger cho Blend Tree
            animator.SetFloat("X", lastX);
        }
    }

    private void TriggerHurtAnim(Vector2 knockbackDir)
    {
        
        if (animator == null) return;
        
        // Kích hoạt Trigger bị thương
        animator.SetTrigger("Hurt");
        
        // Animator sẽ tự dùng giá trị "X" (lastX) hiện tại để biết diễn cảnh ngã sang trái hay phải
        // (Nếu muốn nhân vật bị đánh bay ngược hướng nhìn, logic đó đã nằm ở Animation Clip hoặc Blend Tree setup)
    }

    public void TriggerRoll(Vector2 direction)
    {
        if (animator == null) return;
        
        // Nếu có input di chuyển thì cập nhật hướng lộn mới
        if (direction.x != 0)
        {
             lastX = direction.x > 0 ? 1f : -1f;
             
             // SỬA 4: SetFloat cập nhật hướng cho Blend Tree Roll
             animator.SetFloat("X", lastX);
        }
        
        animator.SetTrigger("Roll");
    }

    public void TriggerAttack()
    {
        if (animator == null) return;
        
        // SỬA 5: SetFloat để đảm bảo Blend Tree Attack đánh đúng hướng mặt đang nhìn
        animator.SetFloat("X", lastX);
        animator.SetTrigger("Attack");
    }

    private void TriggerDeathAnim()
    {
        if (animator) animator.SetTrigger("Death");
    }

    // --- LOGIC VISUALS (Màu sắc & Ánh sáng) ---

    private void UpdateVisuals(float current, float max)
    {
        if (playerStats == null) return;

        // Tính tỷ lệ % máu còn lại
        float ratio = max > 0 ? Mathf.Clamp01(current / max) : 0f;

        // Cập nhật đèn (Máu ít -> Đèn tối/nhỏ)
        if (playerLight != null)
        {
            playerLight.intensity = Mathf.Lerp(playerStats.minLightIntensity, playerStats.maxLightIntensity, ratio);
            playerLight.pointLightOuterRadius = Mathf.Lerp(playerStats.minLightRadius, playerStats.maxLightRadius, ratio);
        }

        // Cập nhật màu nhân vật (Máu ít -> Đen dần)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(playerStats.silhouetteColor, playerStats.lightColor, ratio);
        }
    }
}