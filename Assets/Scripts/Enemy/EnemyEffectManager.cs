using UnityEngine;

public class EnemyEffectManager : BaseEffectManager
{
    private EnemyStatsManager enemyStats;

    protected override void Awake()
    {
        base.Awake();
        enemyStats = GetComponent<EnemyStatsManager>();
    }

    protected override void OnEffectAdded(ActiveEffect effect)
    {
        base.OnEffectAdded(effect);
        
        //TODO: Hiệu ứng hoặc UI làm ở đây
    }

    protected override void OnEffectRemoved(ActiveEffect effect)
    {
        base.OnEffectRemoved(effect);
        
        //TODO: Hiệu ứng hoặc UI làm ở đây
    }
}
