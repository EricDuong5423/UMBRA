using UnityEngine;
[CreateAssetMenu(fileName = "PoisonEffect", menuName = "Roguelike/Effects/PoisonEffect")]
public class PoisonEffect : ItemEffect
{
    [Header("Settings")] 
    public float DamagePerTick = 1f;
    public float TickInterval = 0.5f;

    public override void OnHitEnemy(GameObject player, IDamageable enemy, float damage, int stackCount)
    {
        MonoBehaviour enemyObj = enemy as MonoBehaviour;
        if (enemyObj != null)
        {
            EnemyEffectManager enemyEffectManager = enemyObj.GetComponent<EnemyEffectManager>();
            if (!enemyEffectManager) return;
            enemyEffectManager.AddEffect(this, stackCount, baseDuration);
        }
    }

    public override void OnEffectTick(GameObject target, ActiveEffect activeData)
    {
        if (Time.time > activeData.nextTickTime)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable == null) return;
            float totalDamage = DamagePerTick * activeData.stackCount;
            damageable.TakeDoTDamage(totalDamage);
            activeData.nextTickTime = Time.time + TickInterval;
        }
    }
}