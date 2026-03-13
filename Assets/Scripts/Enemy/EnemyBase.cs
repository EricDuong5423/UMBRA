using UnityEngine;

public enum EnemyState { Idle, Chase, Attack, Dead, Hurt }

[RequireComponent(typeof(EnemyHealth))] 
[RequireComponent(typeof(EnemyStatsManager))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyBase : MonoBehaviour
{
    // ==========================================
    // CẦU NỐI TRUNG GIAN (PUBLIC GETTERS)
    // ==========================================
    public EnemyHealth HealthSystem { get; private set; }
    public EnemyStatsManager StatsManager { get; private set; }
    public EnemyEffectManager EffectManager { get; private set; }
    public EnemyReward RewardSystem { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }

    [Header("Setup")]
    [SerializeField] protected EnemyStats stats;
    [SerializeField] protected Collider2D weaponCollider; 

    [Header("Combat Settings")]
    [SerializeField] protected float hurtDuration = 0.5f;
    public Transform Target { get; private set; } 

    protected EnemyState currentState = EnemyState.Idle;
    protected float nextAttackTime = 0f;
    protected float distanceToTarget;
    protected float hurtEndTime;

    [HideInInspector] public GameObject originalPrefab;

    protected virtual void Awake()
    {
        HealthSystem = GetComponent<EnemyHealth>();
        StatsManager = GetComponent<EnemyStatsManager>();
        EffectManager = GetComponent<EnemyEffectManager>();
        RewardSystem = GetComponent<EnemyReward>();
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) Target = playerObj.transform;
    }

    protected virtual void OnEnable()
    {
        if (StatsManager) StatsManager.Initialize(this);
        if (HealthSystem) HealthSystem.Initialize(this);
        if (EffectManager) EffectManager.Initialize(this);
        if (RewardSystem) RewardSystem.Initialize(this);
        if (HealthSystem)
        {
            HealthSystem.OnDeath += HandleDeath;
            HealthSystem.OnHit += HandleHit;
        }
        if (weaponCollider != null)
        {
            var hitbox = weaponCollider.GetComponent<EnemyWeaponHitbox>();
            if (hitbox == null) hitbox = weaponCollider.gameObject.AddComponent<EnemyWeaponHitbox>();
            
            float dmg = StatsManager != null ? StatsManager.AttackDamage : stats.BaseAtkDamage;
            hitbox.Initialize(dmg, transform);
            weaponCollider.enabled = false; 
        }
        
        currentState = EnemyState.Idle;
        RB.simulated = true;
        if (Anim)
        {
            Anim.Rebind();
            Anim.Update(0f);
        }
    }

    protected virtual void OnDisable()
    {
        if (HealthSystem)
        {
            HealthSystem.OnDeath -= HandleDeath;
            HealthSystem.OnHit -= HandleHit;
        }
    }
    
    protected virtual void HandleHit(Vector2 dir)
    {
        ChangeState(EnemyState.Hurt);
        
        DisableEnemyHitBox();
        StopMovement();
        
        if(Anim) 
        {
            Anim.ResetTrigger("Attack1");
            Anim.ResetTrigger("Attack2");
            Anim.SetTrigger("Hurt");
        }
        
        hurtEndTime = Time.time + hurtDuration;
    }

    public void EnableEnemyHitBox() { if (weaponCollider) weaponCollider.enabled = true; }
    public void DisableEnemyHitBox() { if (weaponCollider) weaponCollider.enabled = false; }

    protected virtual void Update()
    {
        if (currentState == EnemyState.Dead) return;
        if (Target == null) return;

        distanceToTarget = Vector2.Distance(transform.position, Target.position);

        switch (currentState)
        {
            case EnemyState.Idle: LogicIdle(); break;
            case EnemyState.Chase: LogicChase(); break;
            case EnemyState.Attack: LogicAttack(); break;
            case EnemyState.Hurt:  LogicHurt(); break;
        }
    }

    protected virtual void LogicIdle()
    {
        StopMovement();
        if (distanceToTarget <= stats.lookRadius) ChangeState(EnemyState.Chase);
    }

    protected virtual void LogicChase()
    {
        if (distanceToTarget > stats.lookRadius * 1.5f) { ChangeState(EnemyState.Idle); return; }
        if (distanceToTarget <= stats.attackRangeMax) { ChangeState(EnemyState.Attack); return; }
        FaceTarget();
        MoveToTarget();
    }

    protected virtual void LogicAttack()
    {
        StopMovement();
        if (distanceToTarget > stats.attackRangeMax) { ChangeState(EnemyState.Chase); return; }
        FaceTarget();
        
        if (Time.time >= nextAttackTime)
        {
            int attackType = ChooseAttackType();
            PerformAttack(attackType);
            nextAttackTime = Time.time + stats.attackCooldown;
        }
    }

    protected virtual void LogicHurt()
    {
        StopMovement();
        if (Time.time >= hurtEndTime) ChangeState(EnemyState.Chase);
    }

    protected void ChangeState(EnemyState newState) { currentState = newState; }

    protected virtual void FaceTarget()
    {
        if (Target == null || SpriteRenderer == null) return;
        Vector2 direction = Target.position - transform.position;
        if (direction.x != 0) SpriteRenderer.flipX = direction.x < 0;
    }

    protected virtual void MoveToTarget()
    {
        if (Anim) Anim.SetBool("IsMoving", true);
        Vector2 direction = (Target.position - transform.position).normalized;
        float speed = StatsManager != null ? StatsManager.MoveSpeed : stats.BaseMoveSpeed;
        RB.linearVelocity = direction * speed;
    }

    protected virtual void StopMovement()
    {
        if (Anim) Anim.SetBool("IsMoving", false);
        RB.linearVelocity = Vector2.zero;
    }

    protected virtual void HandleDeath()
    {
        ChangeState(EnemyState.Dead);
        StopMovement();
        DisableEnemyHitBox();
        RB.simulated = false;

        if (Anim) 
        {
            Anim.ResetTrigger("Hurt"); 
            Anim.ResetTrigger("Attack1");
            Anim.ResetTrigger("Attack2");
            Anim.SetTrigger("Die"); 
        }
    }

    private void ReturnToPool()
    {
        if (EnemyManager.Instance != null)
            EnemyManager.Instance.ReturnEnemy(originalPrefab, gameObject);
        else
            Destroy(gameObject);
    }

    protected virtual int ChooseAttackType()
    {
        if (distanceToTarget <= stats.attackRangeMin) return 1;
        return 2;
    }

    protected abstract void PerformAttack(int attackIndex);

    protected virtual void OnDrawGizmosSelected()
    {
        if (stats == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stats.lookRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRangeMax);
    }
}