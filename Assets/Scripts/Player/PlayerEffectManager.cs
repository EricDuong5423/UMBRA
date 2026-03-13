using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectManager : BaseEffectManager
{
    private PlayerStatsManager playerStatsManager;

    public void Initialize(PlayerStatsManager statsManager)
    {
        playerStatsManager = statsManager;
    }
}
