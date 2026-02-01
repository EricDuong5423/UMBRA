using UnityEngine;

public interface IDamageable
{
    // Mọi thứ bị đánh đều phải có hàm này
    void TakeDamage(float amount, Transform source);
}