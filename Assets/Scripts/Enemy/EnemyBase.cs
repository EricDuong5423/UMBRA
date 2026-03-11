using UnityEngine;

public enum EnemyState { Idle, Chase, Attack, Hurt, Dead, Frozen }
public class EnemyBase : MonoBehaviour
{
    [HideInInspector] public GameObject originalPrefab; // Phục vụ Object Pool
    public Transform Target { get; private set; }
    public EnemyState CurrentState { get; private set; } = EnemyState.Idle;
    public float DistanceToTarget { get; private set; }

    // --- References tới đàn em ---
    public EnemyStatsManager Stats { get; private set; }
    public EnemyHealth Health { get; private set; }
    public EnemyPhysics Physics { get; private set; }
    public EnemyCombatBase Combat { get; private set; }
    public EnemyAnimator Anim { get; private set; }
    public EnemySound Sound { get; private set; }
    public EnemyVFX VFX { get; private set; }
    public EnemyReward Reward { get; private set; }

    public void InjectDependencies(Transform playerTarget)
    {
        Target = playerTarget;

        Stats = GetComponent<EnemyStatsManager>();
        Health = GetComponent<EnemyHealth>();
        Physics = GetComponent<EnemyPhysics>();
        Combat = GetComponent<EnemyCombatBase>();
        Anim = GetComponent<EnemyAnimator>();
        Sound = GetComponent<EnemySound>();
        VFX = GetComponent<EnemyVFX>();
        Reward = GetComponent<EnemyReward>();
        foreach (var component in GetComponents<IEnemyComponent>())
        {
            component.Initialize(this);
        }
    }

    private void Update()
    {
        if (CurrentState == EnemyState.Dead || CurrentState == EnemyState.Frozen) return;

        if (Target != null)
            DistanceToTarget = Vector2.Distance(transform.position, Target.position);

        if (CurrentState == EnemyState.Hurt) return;
        if (DistanceToTarget <= Stats.BaseStats.attackRangeMax && Combat.CanAttack())
            ChangeState(EnemyState.Attack);
        else if (DistanceToTarget <= Stats.BaseStats.lookRadius)
            ChangeState(EnemyState.Chase);
        else
            ChangeState(EnemyState.Idle);
        Combat.HandleCombat();
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentState = newState;
    }
}