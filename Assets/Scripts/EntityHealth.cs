using System;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour, IDamageable
{
    public event Action<float, float> OnHealthChanged;
    public event Action<Vector2> OnHit;
    public event Action OnDeath;
    public float CurrentEmbers { get; protected set; }
    public float MaxEmbers { get; protected set; }
    public bool IsDead { get; protected set; } 
    protected void InitializeHealth(float maxHealth)
    {
        MaxEmbers = maxHealth;
        CurrentEmbers = maxHealth;
        BroadcastHealth();
    }

    protected void UpdateMaxHealth(float newMaxHealth)
    {
        MaxEmbers = newMaxHealth;
        CurrentEmbers = Mathf.Clamp(CurrentEmbers, 0, MaxEmbers);
        BroadcastHealth();
    }

    // --- LOGIC CHUNG CHO MỌI THỰC THỂ ---
    public virtual void TakeDamage(float amount, Transform source)
    {
        if (IsDead) return;

        CurrentEmbers -= amount;
        CurrentEmbers = Mathf.Clamp(CurrentEmbers, 0, MaxEmbers);
        BroadcastHealth();

        if (CurrentEmbers <= 0) 
        {
            Die();
        }
        else 
        {
            Vector2 direction = source != null ? (Vector2)(transform.position - source.position).normalized : Vector2.zero;
            OnHit?.Invoke(direction);
        }
    }

    public virtual void Heal(float amount)
    {
        if (IsDead) return;
        CurrentEmbers += amount;
        CurrentEmbers = Mathf.Clamp(CurrentEmbers, 0, MaxEmbers);
        BroadcastHealth();
    }

    protected virtual void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }

    protected void BroadcastHealth()
    {
        OnHealthChanged?.Invoke(CurrentEmbers, MaxEmbers);
    }
}