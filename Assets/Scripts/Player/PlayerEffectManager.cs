using UnityEngine;

public class PlayerEffectManager : BaseEffectManager
{
    private StatsManager playerStats;

    protected override void Awake()
    {
        base.Awake();
        playerStats = GetComponent<StatsManager>();
    }

    protected override void OnEffectAdded(ActiveEffect effect)
    {
        base.OnEffectAdded(effect);
        
        // TODO: Dành cho UI
    }

    protected override void OnEffectRemoved(ActiveEffect effect)
    {
        base.OnEffectRemoved(effect);
        
        // TODO: Dành cho UI
    }
}
