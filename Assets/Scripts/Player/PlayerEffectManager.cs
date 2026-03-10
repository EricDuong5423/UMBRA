using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectManager : BaseEffectManager
{
    private StatsManager playerStats;

    protected override void Awake()
    {
        base.Awake();
        playerStats = GetComponent<StatsManager>();
    }
}
