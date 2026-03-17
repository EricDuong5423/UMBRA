using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffectManager : MonoBehaviour
{
    protected List<ActiveEffect> activeEffects = new List<ActiveEffect>();
    public IReadOnlyList<ActiveEffect> ActiveEffects => activeEffects;
    protected EntityHealth healthSystem;
    
    public event Action<ActiveEffect, Transform> OnEffectStarted;
    public event Action<ActiveEffect> OnEffectEnded;

    protected virtual void Awake()
    {
        healthSystem = GetComponent<EntityHealth>();
    }

    public virtual void AddEffect(ItemEffect effectData, int stacks, float duration)
    {
        ActiveEffect existingEffect = activeEffects.Find(effect => effect.effectData == effectData);
        if (existingEffect != null)
        {
            existingEffect.durationTimer = duration;
            existingEffect.stackCount =  stacks;
        }
        else
        {
            ActiveEffect newEffect = new ActiveEffect(effectData, stacks, duration);
            activeEffects.Add(newEffect);

            effectData.OnEffectApplied(gameObject, stacks);
            
            OnEffectStarted?.Invoke(newEffect, transform);
        }
    }

    protected void Update()
    {
        if (activeEffects.Count == 0 || (healthSystem != null && healthSystem.IsDead)) return;
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            ActiveEffect current = activeEffects[i];
            current.durationTimer -= Time.deltaTime;
            current.effectData.OnEffectTick(gameObject, current);
            if (current.durationTimer <= 0)
            {
                current.effectData.OnEffectRemoved(gameObject, current.stackCount);
                OnEffectEnded?.Invoke(current);
                activeEffects.RemoveAt(i);
            }
        }
    }
}
