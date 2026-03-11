using System;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour, IDamageable
{
    public float CurrentEmbers { get; protected set; }
    public bool IsDead { get; protected set; }

    public event Action<float, Transform> OnHit;
    public event Action<float> OnDoTHit;
    public event Action OnDeath;

    public virtual void InitializeHealth(float maxHealth)
    {
        CurrentEmbers = maxHealth;
        IsDead = false;
    }

    public virtual void TakeDamage(float amount, Transform source)
    {
        if (IsDead) return;
        CurrentEmbers -= amount;
        OnHit?.Invoke(amount, source);
        if (CurrentEmbers <= 0) Die();
    }

    public virtual void TakeDoTDamage(float amount)
    {
        if (IsDead) return;
        CurrentEmbers -= amount;
        OnDoTHit?.Invoke(amount);
        if (CurrentEmbers <= 0) Die();
    }

    protected virtual void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }
}