using UnityEngine;

public enum EnemyState { Idle, Wander, Chase, Attack, Flee, Hurt, Dead }

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyStatsManager))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyBase : MonoBehaviour
{
    // ── Public Getters ──────────────────────────────────────
    public EnemyHealth HealthSystem       { get; private set; }
    public EnemyStatsManager StatsManager { get; private set; }
    public EnemyEffectManager EffectManager { get; private set; }
    public EnemyReward RewardSystem       { get; private set; }
    public Rigidbody2D RB                 { get; private set; }
    public Animator Anim                  { get; private set; }
    public SpriteRenderer SpriteRenderer  { get; private set; }
    public Transform Target               { get; private set; }
    public EnemyStats Stats               => stats; // expose ra ngoài cho spawn system

    [Header("Setup")]
    [SerializeField] protected EnemyStats stats;
    [SerializeField] protected Collider2D weaponCollider;

    [Header("Combat Settings")]
    [SerializeField] protected float hurtDuration = 0.5f;

    [HideInInspector] public GameObject originalPrefab;

    protected EnemyState currentState = EnemyState.Idle;
    protected float nextAttackTime = 0f;
    protected float distanceToTarget;
    protected float hurtEndTime;

    // Wander
    private Vector2 wanderTarget;
    private float nextWanderTime;

    // ── Lifecycle ───────────────────────────────────────────
    protected virtual void Awake()
    {
        HealthSystem    = GetComponent<EnemyHealth>();
        StatsManager    = GetComponent<EnemyStatsManager>();
        EffectManager   = GetComponent<EnemyEffectManager>();
        RewardSystem    = GetComponent<EnemyReward>();
        RB              = GetComponent<Rigidbody2D>();
        Anim            = GetComponent<Animator>();
        SpriteRenderer  = GetComponent<SpriteRenderer>();

        // fix: dùng PlayerManager thay FindGameObjectWithTag
        if (PlayerManager.Instance != null)
            Target = PlayerManager.Instance.transform;
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
            HealthSystem.OnHit   += HandleHit;
        }

        if (weaponCollider != null)
        {
            var hitbox = weaponCollider.GetComponent<EnemyWeaponHitbox>();
            if (hitbox == null) hitbox = weaponCollider.gameObject.AddComponent<EnemyWeaponHitbox>();
            hitbox.Initialize(StatsManager, transform); // fix: truyền StatsManager, không cache float
            weaponCollider.enabled = false;
        }

        currentState = stats != null && stats.wandersWhenIdle ? EnemyState.Wander : EnemyState.Idle;
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
            HealthSystem.OnHit   -= HandleHit;
        }
    }

    // ── Update Loop ─────────────────────────────────────────
    protected virtual void Update()
    {
        if (currentState == EnemyState.Dead) return;
        if (Target == null) return;

        distanceToTarget = Vector2.Distance(transform.position, Target.position);

        // Kiểm tra flee ưu tiên cao
        if (ShouldFlee() && currentState != EnemyState.Flee && currentState != EnemyState.Hurt)
        {
            ChangeState(EnemyState.Flee);
        }

        switch (currentState)
        {
            case EnemyState.Idle:   LogicIdle();   break;
            case EnemyState.Wander: LogicWander(); break;
            case EnemyState.Chase:  LogicChase();  break;
            case EnemyState.Attack: LogicAttack(); break;
            case EnemyState.Flee:   LogicFlee();   break;
            case EnemyState.Hurt:   LogicHurt();   break;
        }
    }

    // ── State Logic (override thoải mái trong subclass) ─────
    protected virtual void LogicIdle()
    {
        StopMovement();
        if (distanceToTarget <= stats.lookRadius)
        {
            ChangeState(EnemyState.Chase);
            return;
        }
        if (stats.wandersWhenIdle && Time.time >= nextWanderTime)
            ChangeState(EnemyState.Wander);
    }

    protected virtual void LogicWander()
    {
        // Nếu thấy player → chase ngay
        if (distanceToTarget <= stats.lookRadius) { ChangeState(EnemyState.Chase); return; }

        // Chọn điểm wander mới khi đến nơi hoặc hết timer
        if (Time.time >= nextWanderTime || Vector2.Distance(transform.position, wanderTarget) < 0.3f)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            wanderTarget = (Vector2)transform.position + randomDir * Random.Range(1f, stats.wanderRadius);
            nextWanderTime = Time.time + stats.wanderInterval;
        }

        Vector2 dir = (wanderTarget - (Vector2)transform.position).normalized;
        FaceDirection(dir);
        float speed = StatsManager != null ? StatsManager.MoveSpeed * 0.5f : stats.BaseMoveSpeed * 0.5f;
        RB.linearVelocity = dir * speed;
        if (Anim) Anim.SetBool("IsMoving", true);
    }

    protected virtual void LogicChase()
    {
        if (distanceToTarget > stats.lookRadius * 1.5f)
        {
            ChangeState(stats.wandersWhenIdle ? EnemyState.Wander : EnemyState.Idle);
            return;
        }
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

    protected virtual void LogicFlee()
    {
        Vector2 fleeDir = ((Vector2)transform.position - (Vector2)Target.position).normalized;
        FaceDirection(fleeDir);
        float speed = StatsManager != null ? StatsManager.MoveSpeed : stats.BaseMoveSpeed;
        RB.linearVelocity = fleeDir * speed;
        if (Anim) Anim.SetBool("IsMoving", true);
        
        if (distanceToTarget > stats.lookRadius)
            ChangeState(stats.wandersWhenIdle ? EnemyState.Wander : EnemyState.Idle);
    }

    protected virtual void LogicHurt()
    {
        StopMovement();
        if (Time.time >= hurtEndTime)
            ChangeState(EnemyState.Chase);
    }

    // ── Hit / Death ─────────────────────────────────────────
    protected virtual void HandleHit(Vector2 dir)
    {
        ChangeState(EnemyState.Hurt);
        DisableEnemyHitBox();
        StopMovement();

        if (Anim)
        {
            Anim.ResetTrigger("Attack1");
            Anim.ResetTrigger("Attack2");
            Anim.SetTrigger("Hurt");
        }

        hurtEndTime = Time.time + hurtDuration;
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

    // ── Helpers ─────────────────────────────────────────────
    protected void ChangeState(EnemyState newState) { currentState = newState; }

    protected virtual void FaceTarget()
    {
        if (Target == null || SpriteRenderer == null) return;
        SpriteRenderer.flipX = (Target.position.x - transform.position.x) < 0;
    }

    protected void FaceDirection(Vector2 dir)
    {
        if (SpriteRenderer == null) return;
        if (dir.x != 0) SpriteRenderer.flipX = dir.x < 0;
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

    private bool ShouldFlee()
    {
        if (stats == null || !stats.fleesWhenLowHealth) return false;
        if (HealthSystem == null || HealthSystem.MaxEmbers <= 0) return false;
        return (HealthSystem.CurrentEmbers / HealthSystem.MaxEmbers) <= stats.fleeHealthThreshold;
    }

    public void EnableEnemyHitBox()  { if (weaponCollider) weaponCollider.enabled = true; }
    public void DisableEnemyHitBox() { if (weaponCollider) weaponCollider.enabled = false; }
    protected void SpawnProjectile(GameObject projectilePrefab, Vector2 direction, float speed)
    {
        if (projectilePrefab == null) return;
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        if (proj.TryGetComponent(out Rigidbody2D rb))
            rb.linearVelocity = direction.normalized * speed;
    }

    protected virtual int ChooseAttackType()
    {
        if (distanceToTarget <= stats.attackRangeMin) return 1;
        return 2;
    }
    protected abstract void PerformAttack(int attackIndex);
    public void SetStats(EnemyStats overrideStats)
    {
        stats = overrideStats;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (stats == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stats.lookRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRangeMax);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stats.attackRangeMin);
    }
}