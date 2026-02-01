using UnityEngine;

public class EnemyWeaponHitbox : MonoBehaviour
{
    private float damage;
    private Transform owner;

    public void Initialize(float damageAmount, Transform enemyTransform)
    {
        damage = damageAmount;
        owner = enemyTransform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damage, owner);
        }
    }
}