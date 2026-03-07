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

    public void Initialize(PlayerStats stats, PlayerHealth health)
    {
        playerStats = stats;
        playerHealth = health;
        
        playerHealth.OnHealthChanged += UpdateVisuals;
        playerHealth.OnHit += TriggerHurtAnim;
        playerHealth.OnDeath += TriggerDeathAnim;
        
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

    public void UpdateMovementAnim(Vector2 direction, bool isMoving)
    {
        if (animator == null) return;
        animator.SetBool("IsMoving", isMoving);
    }

    private void TriggerHurtAnim(Vector2 knockbackDir)
    {
        
        if (animator == null) return;
        animator.SetTrigger("Hurt");
    }

    public void TriggerRoll(Vector2 direction)
    {
        if (animator == null) return;
        animator.SetTrigger("Roll");
    }

    public void TriggerAttack()
    {
        if (animator == null) return;
        animator.SetTrigger("Attack");
    }

    private void TriggerDeathAnim()
    {
        if (animator) animator.SetTrigger("Death");
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