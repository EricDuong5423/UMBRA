using UnityEngine;

public class PlayerWeaponHitbox : MonoBehaviour
{
    private float damage;
    private Transform owner; // Transform của Player (để tính hướng đẩy lùi)

    // Hàm này được PlayerCombat gọi để cài đặt thông số
    public void Initialize(float damageAmount, Transform playerTransform)
    {
        damage = damageAmount;
        owner = playerTransform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nhờ Matrix, nó chỉ va chạm với Enemy (Layer 8)
        // Nhưng check component cho chắc ăn
        if (other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            // Gây damage và truyền 'owner' vào để Enemy biết hướng bị đánh
            enemyHealth.TakeDamage(damage, owner);
        }
    }
}