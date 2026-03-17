using UnityEngine;

public class EnemyWeaponHitbox : MonoBehaviour
{
    private EnemyStatsManager statsManager;
    private Transform owner;

    public void Initialize(EnemyStatsManager stats, Transform enemyTransform)
    {
        statsManager = stats;
        owner = enemyTransform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable target))
        {
            float damage = statsManager != null ? statsManager.GetCalculatedHitDamage() : 0f;
            target.TakeDamage(damage, owner);
        }
    }
}