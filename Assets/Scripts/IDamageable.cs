using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount, Transform source, bool isCrit, float knockbackForce = 0f);
    
    void TakeDoTDamage(float amount);
}