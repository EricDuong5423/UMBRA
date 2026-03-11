using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BaseEffectManager))]
public class EntityVFXHandler : MonoBehaviour
{
    [SerializeField] private Transform effectVFXContainer;
    private BaseEffectManager effectManager;
    private Dictionary<ActiveEffect, GameObject> spawnedVFXs = new Dictionary<ActiveEffect, GameObject>();

    private void Awake()
    {
        effectManager = GetComponent<BaseEffectManager>();
        if (effectVFXContainer == null) 
        {
            effectVFXContainer = transform;
        }
    }

    private void OnEnable()
    {
        effectManager.OnEffectStarted += HandleEffectStarted;
        effectManager.OnEffectEnded += HandleEffectEnded;
    }

    private void OnDisable()
    {
        effectManager.OnEffectStarted -= HandleEffectStarted;
        effectManager.OnEffectEnded -= HandleEffectEnded;
    }

    private void HandleEffectStarted(ActiveEffect effect, Transform targetTransform)
    {
        if (effect.effectData.vfxFrames == null || effect.effectData.vfxFrames.Length == 0) return;
        
        GameObject vfx = EffectVFXManager.Instance.SpawnVFX(effectVFXContainer.position, effectVFXContainer);
        
        vfx.GetComponent<SimpleVFXPlayer>()?.PlayAnimation(effect.effectData.vfxFrames, effect.effectData.vfxFramerate);
        
        vfx.transform.localPosition = Vector3.zero;
        
        spawnedVFXs.Add(effect, vfx);
    }

    private void HandleEffectEnded(ActiveEffect effect)
    {
        if (spawnedVFXs.TryGetValue(effect, out GameObject vfxToRemove))
        {
            EffectVFXManager.Instance.ReturnToPool(vfxToRemove);

            spawnedVFXs.Remove(effect);
        }
    }
}