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
    private bool pendingHealTrigger = false;

    public void Initialize(PlayerStats stats, PlayerHealth health)
    {
        playerStats = stats;
        playerHealth = health;
        
        playerHealth.OnHealthChanged += UpdateVisuals;
        playerHealth.OnHit += TriggerHurtAnim;
        playerHealth.OnDeath += TriggerDeathAnim;
        playerHealth.OnHeal += TriggerHeal;
        
        UpdateVisuals(playerHealth.CurrentEmbers, playerHealth.MaxEmbers);
    }

    public void ResetDeathAnimation()
    {
        if (animator == null) return;
        animator.ResetTrigger("Death"); 
        animator.Play("Idle");
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

    private void TriggerHurtAnim(Vector2 knockbackDir, float _)
    {
        
        if (animator == null) return;
        animator.SetTrigger("Hurt");
    }

    private void TriggerHeal()
    {
        if (animator == null) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            pendingHealTrigger = true;
            return;
        }
        animator.SetTrigger("Heal");
    }

    public void OnAttackAnimationEnd()
    {
        if (pendingHealTrigger)
        {
            pendingHealTrigger = false;
            animator.SetTrigger("Heal");
        }
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
            playerLight.intensity = Mathf.Lerp(playerStats.MinLightIntensity, playerStats.MaxLightIntensity, ratio);
            playerLight.pointLightOuterRadius = Mathf.Lerp(playerStats.MinLightRadius, playerStats.MaxLightRadius, ratio);
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(playerStats.silhouetteColor, playerStats.lightColor, ratio);
        }
    }
}