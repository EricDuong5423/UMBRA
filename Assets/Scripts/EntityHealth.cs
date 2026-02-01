using System;
using UnityEngine;

// Kế thừa IDamageable để vũ khí nhận diện được
public class EntityHealth : MonoBehaviour, IDamageable
{
    public event Action<float, float> OnHealthChanged;
    public event Action<Vector2> OnHit;
    public event Action OnDeath;

    protected StatsManager stats;
    protected float currentEmbers;
    protected bool isDead = false;

    // Public Getter
    public float CurrentEmbers => currentEmbers;
    public float MaxEmbers => stats != null ? stats.MaxEmbers : 100f;
    public bool IsDead => isDead; // Để Controller check

    protected virtual void Awake()
    {
        stats = GetComponent<StatsManager>();
    }

    protected virtual void Start()
    {
        if (stats) currentEmbers = stats.MaxEmbers;
        
        if (stats) stats.OnLevelChanged += HandleLevelUp;
    
        BroadcastHealth();
    }

    protected virtual void OnDestroy()
    {
        if (stats) stats.OnLevelChanged -= HandleLevelUp;
    }

    // Implement từ Interface IDamageable
    public virtual void TakeDamage(float amount, Transform source)
    {
        if (isDead) return;

        currentEmbers -= amount;
        currentEmbers = Mathf.Clamp(currentEmbers, 0, MaxEmbers);
        
        BroadcastHealth();

        Vector2 direction = Vector2.zero;
        if (source != null)
        {
            direction = (transform.position - source.position).normalized;
        }
        OnHit?.Invoke(direction);

        if (currentEmbers <= 0) Die();
    }

    public virtual void Heal(float amount)
    {
        if (isDead) return;
        currentEmbers += amount;
        currentEmbers = Mathf.Clamp(currentEmbers, 0, MaxEmbers);
        BroadcastHealth();
    }

    protected virtual void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
    }

    protected virtual void HandleLevelUp()
    {
        currentEmbers = MaxEmbers;
        BroadcastHealth();
    }

    protected void BroadcastHealth()
    {
        OnHealthChanged?.Invoke(currentEmbers, MaxEmbers);
    }
}