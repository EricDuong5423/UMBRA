using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount, Transform source);
    
    void TakeDoTDamage(float amount);
}