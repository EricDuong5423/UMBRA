using UnityEngine;

public class PlayerWeaponHitbox : MonoBehaviour
{
    private float damage;
    private Transform owner; 

    public void Initialize(float damageAmount, Transform playerTransform)
    {
        damage = damageAmount;
        owner = playerTransform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // QUAN TRỌNG: Tìm Interface thay vì tìm class cụ thể
        if (other.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damage, owner);
        }
    }
}