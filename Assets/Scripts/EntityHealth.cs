using System;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    // Events
    public event Action<float, float> OnHealthChanged; // Cho UI
    public event Action<Vector2> OnHit;                // Cho Animation (Hurt)
    public event Action OnDeath;                       // Cho Logic chết

    // Variables
    protected StatsManager stats;
    protected float currentEmbers;
    public bool isDead { get; private set; }

    // Properties
    public float CurrentEmbers => currentEmbers;
    public float MaxEmbers => stats != null ? stats.MaxEmbers : 100f;

    protected virtual void Awake()
    {
        stats = GetComponent<StatsManager>();
        isDead = false;
        if (stats) currentEmbers = stats.MaxEmbers;
    }

    protected virtual void Start()
    {
        if (stats) stats.OnLevelChanged += HandleLevelUp;
        BroadcastHealth();
    }

    protected virtual void OnDestroy()
    {
        if (stats) stats.OnLevelChanged -= HandleLevelUp;
    }

    // --- LOGIC CHÍNH (VIRTUAL ĐỂ CON GHI ĐÈ) ---

    public virtual void TakeDamage(float amount, Transform source)
    {
        if (isDead) return;

        currentEmbers -= amount;
        currentEmbers = Mathf.Clamp(currentEmbers, 0, MaxEmbers);
        
        BroadcastHealth();

        // Tính hướng bị đẩy (Knockback Direction)
        Vector2 direction = Vector2.zero;
        if (source != null)
        {
            // Vector từ Nguồn -> Nạn nhân
            direction = (transform.position - source.position).normalized;
        }

        // Bắn sự kiện bị đánh kèm hướng
        OnHit?.Invoke(direction);

        if (currentEmbers <= 0)
        {
            Die();
        }
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