using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectManager : BaseEffectManager
{
    private EnemyStatsManager enemyStats;
    [SerializeField] private GameObject effectVFXContainer;
    private Dictionary<ActiveEffect, GameObject> spawnedEffectsVFX = new Dictionary<ActiveEffect, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        enemyStats = GetComponent<EnemyStatsManager>();
    }

    protected override void OnEffectAdded(ActiveEffect effect)
    {
        //TODO: Hiệu ứng hoặc UI làm ở đây
        if (effect == null || effect.effectData.EffectVFXPrefab == null) return;
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
        //TODO: Hiệu ứng hoặc UI làm ở đây
        if (effect == null || effect.effectData.EffectVFXPrefab == null) return;
        if (spawnedEffectsVFX.TryGetValue(effect, out var effectVFXPrefabToRemove))
        {
            Destroy(effectVFXPrefabToRemove);
            spawnedEffectsVFX.Remove(effect);
        }
    }
}
