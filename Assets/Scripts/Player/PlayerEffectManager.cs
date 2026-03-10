using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectManager : BaseEffectManager
{
    private StatsManager playerStats;
    [SerializeField] private GameObject effectVFXContainer;
    private Dictionary<ActiveEffect, GameObject> spawnedEffectsVFX = new Dictionary<ActiveEffect, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        playerStats = GetComponent<StatsManager>();
    }

    protected override void OnEffectAdded(ActiveEffect effect)
    {
        // TODO: Dành cho UI
        if(effect == null || effect.effectData.EffectVFXPrefab == null) return;
        if (spawnedEffectsVFX.TryGetValue(effect, out GameObject effectVFX))
        {
            Destroy(effectVFX);
            spawnedEffectsVFX.Remove(effect);
        }
        var newEffect = Instantiate(effect.effectData.EffectVFXPrefab
                                            , effectVFXContainer.transform.position
                                            , Quaternion.identity);
        newEffect.transform.SetParent(effectVFXContainer.transform, true);
        spawnedEffectsVFX.Add(effect, newEffect);
    }

    protected override void OnEffectRemoved(ActiveEffect effect)
    {
        // TODO: Dành cho UI
        if(effect == null || effect.effectData.EffectVFXPrefab == null) return;
        if (spawnedEffectsVFX.TryGetValue(effect, out GameObject effectVFX))
        {
            Destroy(effectVFX);
            spawnedEffectsVFX.Remove(effect);
        }
    }
}
