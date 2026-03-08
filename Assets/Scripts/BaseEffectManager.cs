using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffectManager : MonoBehaviour
{
    public List<ActiveEffect> activeEffects = new List<ActiveEffect>();
    protected EntityHealth healthSystem;

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
            
            OnEffectAdded(newEffect);
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
                OnEffectRemoved(current);
                activeEffects.RemoveAt(i);
            }
        }
    }
    
    protected virtual void OnEffectAdded(ActiveEffect effect){}
    protected virtual void OnEffectRemoved(ActiveEffect effect){}
}
