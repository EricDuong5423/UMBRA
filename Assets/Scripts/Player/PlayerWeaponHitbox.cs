using UnityEngine;

public class PlayerWeaponHitbox : MonoBehaviour
{
    private Transform owner;
    private StatsManager stats;

    public void Initialize(StatsManager playerStats, Transform playerTransform)
    {
        stats = playerStats;
        owner = playerTransform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.TryGetComponent(out IDamageable target))
        {
            if (!stats) return;
            float finalDamage = stats.GetCalculatedHitDamage();
            target.TakeDamage(finalDamage, owner);
        }
    }
}