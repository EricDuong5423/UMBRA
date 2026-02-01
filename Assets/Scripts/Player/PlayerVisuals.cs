using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerVisuals : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Light2D playerLight;
    [SerializeField] private Animator animator;
    
    private PlayerStats playerStats; 
    private PlayerHealth playerHealth; // Đổi thành PlayerHealth

    // Initialization
    public void Initialize(PlayerStats stats, PlayerHealth health)
    {
        playerStats = stats;
        playerHealth = health;

        // Đăng ký sự kiện
        playerHealth.OnHealthChanged += UpdateVisuals;
        playerHealth.OnHit += TriggerHurtAnim; // MỚI: Lắng nghe bị đánh
        playerHealth.OnDeath += TriggerDeathAnim;

        UpdateVisuals(playerHealth.CurrentEmbers, playerHealth.MaxEmbers);
    }

    private void TriggerDeathAnim()
    {
        animator.SetTrigger("Death");
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

    // --- XỬ LÝ ANIMATION HURT MỚI ---
    private void TriggerHurtAnim(Vector2 knockbackDir)
    {
        if (animator == null) return;

        // Trigger chung "Hurt"
        animator.SetTrigger("Hurt");
        
        animator.SetFloat("HurtX", -knockbackDir.x);
    }

    // ... (Giữ nguyên UpdateMovementAnim, TriggerRoll, TriggerAttack) ...
    public void UpdateMovementAnim(Vector2 direction, bool isMoving)
    {
        if (animator == null) return;
        animator.SetInteger("X", Mathf.RoundToInt(direction.x));
        animator.SetInteger("Y", Mathf.RoundToInt(direction.y));
    }
    
    public void TriggerRoll(Vector2 direction)
    {
        if (animator) 
        {
            animator.SetInteger("X", Mathf.RoundToInt(direction.x));
            animator.SetTrigger("Roll");
        }
    }

    public void TriggerAttack()
    {
        if (animator) animator.SetTrigger("Attack");
    }

    private void UpdateVisuals(float current, float max)
    {
        if (playerStats == null) return;

        float ratio = max > 0 ? Mathf.Clamp01(current / max) : 0f;

        if (playerLight != null)
        {
            playerLight.intensity = Mathf.Lerp(playerStats.minLightIntensity, playerStats.maxLightIntensity, ratio);
            playerLight.pointLightOuterRadius = Mathf.Lerp(playerStats.minLightRadius, playerStats.maxLightRadius, ratio);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(playerStats.silhouetteColor, playerStats.lightColor, ratio);
        }
    }
}