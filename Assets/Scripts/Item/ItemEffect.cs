using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public string Name;
    [TextArea]public string Description;
    public float baseDuration = 3f;
    public virtual void OnHitEnemy(GameObject player, IDamageable enemy, float damage, int stackCount){}
    public virtual void OnKillEnemy(GameObject player, int stackCount){}
    public virtual void OnPlayerTakeDamage(GameObject player, float damageTaken, int stackCount){}
    public virtual void OnEffectApplied(GameObject target, int stackCount){}
    public virtual void OnEffectRemoved(GameObject target, int stackCount){}
    public virtual void OnEffectTick(GameObject target, ActiveEffect activeData){}
}
