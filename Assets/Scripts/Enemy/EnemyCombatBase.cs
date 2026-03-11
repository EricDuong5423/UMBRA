using System;
using UnityEngine;

public abstract class EnemyCombatBase : MonoBehaviour, IEnemyComponent
{
    protected EnemyBase brain;
    protected float nextAttackTime;
    public event Action OnAttackPerformed; 

    public virtual void Initialize(EnemyBase brain)
    {
        this.brain = brain;
        nextAttackTime = 0f;
    }

    public bool CanAttack() => Time.time >= nextAttackTime;

    public void HandleCombat()
    {
        if (brain.CurrentState == EnemyState.Attack && CanAttack())
        {
            PerformAttack();
        }
    }

    protected abstract void PerformAttack();
    
    protected void TriggerAttackEvent()
    {
        OnAttackPerformed?.Invoke();
        nextAttackTime = Time.time + brain.Stats.BaseStats.attackCooldown;
        brain.ChangeState(EnemyState.Idle);
    }
}