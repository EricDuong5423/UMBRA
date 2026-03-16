using UnityEngine;

public class PlayerWeaponHitbox : MonoBehaviour
{
    private Transform owner;
    private PlayerStatsManager stats;
    [SerializeField] private float shakeStrengthMultply = 3f;
    [SerializeField] private float effectDuration = 0.5f;

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
            float finalDamage = stats.GetCalculatedHitDamage();
            target.TakeDamage(finalDamage, owner);
            if (!owner) return;
            ItemManager inventory = owner.GetComponent<ItemManager>();
            if (inventory == null) return;
            inventory.TriggerOnHitEffects(target, finalDamage);
            CameraShake.Shake(effectDuration, finalDamage * shakeStrengthMultply);
        }
    }
}