using UnityEngine;

public class EnemyWeaponHitbox : MonoBehaviour
{
    private float damage;
    private Transform owner; // Transform của Enemy

    public void Initialize(float damageAmount, Transform enemyTransform)
    {
        damage = damageAmount;
        owner = enemyTransform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nhờ Matrix, nó chỉ va chạm với Player (Layer 6)
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            // Gây damage và truyền 'owner' vào để Player biết hướng bị đánh
            playerHealth.TakeDamage(damage, owner);
        }
    }
}