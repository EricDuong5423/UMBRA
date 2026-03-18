using UnityEngine;
using System;

public class PlayerWeaponHitbox : MonoBehaviour
{
    public event Action<float, Vector2, bool> OnHitLanded;

    private Transform owner;
    private PlayerStatsManager stats;

    public void Initialize(PlayerStatsManager playerStats, Transform playerTransform)
    {
        stats = playerStats;
        owner = playerTransform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable target))
        {
            if (!stats) return;
            float finalDamage = stats.GetCalculatedHitDamage(out bool isCrit);
            float knockback = stats.KnockbackForce; 
            target.TakeDamage(finalDamage, owner, isCrit, knockback);
            Vector2 hitDirection = owner != null
                ? ((Vector2)(other.transform.position - owner.position)).normalized
                : Vector2.zero;
            OnHitLanded?.Invoke(finalDamage, hitDirection, isCrit);
            ItemManager inventory = owner?.GetComponent<ItemManager>();
            inventory?.TriggerOnHitEffects(target, finalDamage);
        }
    }
}