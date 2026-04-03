using System.Collections;
using UnityEngine;

public class VampireBoss : EnemyBase
{
    [SerializeField] private float decisionInterval = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float aggroChance = 0.6f;
    private float nextDecisionTime = 0f;
    public static VampireBoss Instance { get; private set; }
    [SerializeField] private GameObject _projectilePrefab;
    private SpawnData _spawnData;
    private EncounterSpawner _spawner;
    [SerializeField] private GameObject _minionPrefab;
    [SerializeField] private float _projectileDamage = 10f;
    [SerializeField] private float _knockBackForce = 10f;
    protected override void PerformAttack(int attackIndex)
    {
        if (currentState == EnemyState.Hurt) return;
        switch (attackIndex)
        {
            case 1: SpawnProjectile(1); Anim.SetTrigger("Attack"); break;
            case 2: SpawnProjectile(10); Anim.SetTrigger("Attack"); break;
            case 3: SpawnMinion(3); break;
            default: break;
        }
    }

    private void DecideChaseOrWander()
    {
        if (Time.time < nextDecisionTime) return;
        nextDecisionTime = Time.time + decisionInterval;

        if (Random.value <= aggroChance)
        {
            ChangeState(EnemyState.Chase);
        }
        else
        {
            ChangeState(EnemyState.Wander);
        }
    }
    
    protected override void LogicChase()
    {
        if (distanceToTarget > stats.lookRadius * 1.5f)
        {
            ChangeState(stats.wandersWhenIdle ? EnemyState.Wander : EnemyState.Idle);
            return;
        }
        
        if (distanceToTarget <= stats.attackRangeMax) { ChangeState(EnemyState.Attack); return; }
        DecideChaseOrWander();
        if (currentState != EnemyState.Chase) return;

        FaceTarget();
        MoveToTarget();
    }

    protected override int ChooseAttackType()
    {
        float dist = Vector2.Distance(transform.position, Target.position);
        if (dist < stats.attackRangeMax && dist >= stats.attackRangeMin)
        {
            return Random.Range(1, 2);
        }
        else
        {
            return 3;
        }
        
    }

    protected void Start()
    {
        _spawner = FindAnyObjectByType<EncounterSpawner>(); 
        
        if (_spawner == null)
        {
            Debug.LogError("Boss không tìm thấy EncounterSpawner trên Scene!");
        }
    }

    private void SpawnMinion(float radius)
    {
        float ratio = HealthSystem.CurrentEmbers / HealthSystem.MaxEmbers;
        int count = 0;
        if (ratio == 1) count = 1;
        else if (ratio < 1 && ratio >= 0.5) count = 2;
        else count = Random.Range(3, 5);
        _spawnData = new SpawnData(); 
        
        _spawnData.enemyPrefab = _minionPrefab;
        _spawnData.radius = radius;
        _spawnData.count = count;
        _spawnData.spawnPoint = transform;
        
        _spawner.SpawnEnemies(_spawnData);
    }

    protected override void HandleHit(Vector2 dir, float knockbackForce)
    {
        ChangeState(EnemyState.Hurt);
        DisableEnemyHitBox();
        StopMovement();

        if (Anim)
        {
            Anim.ResetTrigger("Attack");
            Anim.SetTrigger("Hurt");
        }
        
        if (knockbackForce > 0f)
        {
            RB.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        }

        hurtEndTime = Time.time + hurtDuration;
    }

    protected override void HandleDeath()
    {
        ChangeState(EnemyState.Dead);
        StopMovement();
        DisableEnemyHitBox();
        RB.simulated = false;

        if (Anim)
        {
            Anim.ResetTrigger("Hurt");
            Anim.ResetTrigger("Attack");
            Anim.SetTrigger("isDead");
        }
    }

    protected override void MoveToTarget()
    {
        if (Anim) Anim.SetBool("IsMoving", true);
        Vector2 direction = (Target.position - transform.position).normalized;
        Anim.SetFloat("X", direction.x);
        Anim.SetFloat("Y", direction.y);
        float speed = StatsManager != null ? StatsManager.MoveSpeed : stats.BaseMoveSpeed;
        RB.linearVelocity = direction * speed;
    }

    protected override void FaceDirection(Vector2 dir)
    {
        return;
    }

    protected override void FaceTarget()
    {
        return;
    }

    protected override void StopMovement()
    {
        if (Anim) Anim.SetBool("IsMoving", false);
        RB.linearVelocity = Vector2.zero;
    }

    private void SpawnProjectile(int count)
    {
        StartCoroutine(SpawnProjectileRoutine(count, 0.5f));
    }
    
    private IEnumerator SpawnProjectileRoutine(int count, float delayTime)
    {
        for (int i = 0; i < count; i++)
        {
            ProjectileManager.Instance.SpawnProjectile(_projectilePrefab, Target, this.transform, _projectileDamage, _knockBackForce);
            yield return new WaitForSeconds(delayTime);
        }
    }
}
