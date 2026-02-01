using UnityEngine;

public enum EnemyState { Idle, Chase, Attack, Dead, Hurt }

[RequireComponent(typeof(EnemyHealth))] 
[RequireComponent(typeof(StatsManager))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] protected EnemyStats stats;
    
    // BIẾN DUY NHẤT ĐỂ CHỨA VŨ KHÍ CỦA QUÁI
    [SerializeField] protected Collider2D weaponCollider; 

    [Header("Combat Settings")]
    [SerializeField] protected float hurtDuration = 0.5f;
    
    protected Transform target;
    protected EnemyHealth healthSystem;
    public StatsManager statsManager;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    protected EnemyState currentState = EnemyState.Idle;
    protected float nextAttackTime = 0f;
    protected float distanceToTarget;
    protected float hurtEndTime;

    protected virtual void Awake()
    {
        healthSystem = GetComponent<EnemyHealth>();
        statsManager = GetComponent<StatsManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) target = playerObj.transform;
    }
    
    protected virtual void Start()
    {
        if (healthSystem)
        {
            healthSystem.OnDeath += HandleDeath;
            healthSystem.OnHit += HandleHit;
        }

        // TỰ ĐỘNG SETUP VŨ KHÍ CHO QUÁI
        if (weaponCollider != null)
        {
            var hitbox = weaponCollider.GetComponent<EnemyWeaponHitbox>();
            if (hitbox == null) hitbox = weaponCollider.gameObject.AddComponent<EnemyWeaponHitbox>();
            
            // Ưu tiên lấy damage từ StatsManager (nếu có level), nếu không thì lấy gốc
            float dmg = statsManager != null ? statsManager.AttackDamage : stats.baseAtkDamage;
            
            hitbox.Initialize(dmg, transform);
            weaponCollider.enabled = false; // Tắt mặc định
        }
    }

    protected virtual void OnDestroy()
    {
        if (healthSystem)
        {
            healthSystem.OnDeath -= HandleDeath;
            healthSystem.OnHit -= HandleHit;
        }
    }

    // --- ANIMATION EVENTS (Dùng chung cho mọi con quái) ---
    public void EnableEnemyHitBox()
    {
        if (weaponCollider) weaponCollider.enabled = true;
    }

    public void DisableEnemyHitBox()
    {
        if (weaponCollider) weaponCollider.enabled = false;
    }

    protected virtual void HandleHit(Vector2 dir)
    {
        ChangeState(EnemyState.Hurt);
        hurtEndTime = Time.time + hurtDuration;
        if(anim) anim.SetTrigger("Hurt");
    }
    
    protected virtual void FaceTarget()
    {
        if (target == null || spriteRenderer == null) return;
        Vector2 direction = target.position - transform.position;
        if (direction.x != 0) spriteRenderer.flipX = direction.x < 0;
    }

    protected virtual void Update()
    {
        if (currentState == EnemyState.Dead) return;
        if (target == null) return;

        distanceToTarget = Vector2.Distance(transform.position, target.position);

        switch (currentState)
        {
            case EnemyState.Idle: LogicIdle(); break;
            case EnemyState.Chase: LogicChase(); break;
            case EnemyState.Attack: LogicAttack(); break;
            case EnemyState.Hurt:  LogicHurt(); break;
        }
    }

    // ... (Giữ nguyên LogicIdle, LogicChase) ...
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

    protected virtual void MoveToTarget()
    {
        if (anim) anim.SetBool("IsMoving", true);
        Vector2 direction = (target.position - transform.position).normalized;
        float speed = statsManager != null ? statsManager.MoveSpeed : stats.baseMoveSpeed;
        rb.linearVelocity = direction * speed;
    }

    protected virtual void StopMovement()
    {
        if (anim) anim.SetBool("IsMoving", false);
        rb.linearVelocity = Vector2.zero;
    }

    protected virtual void HandleDeath()
    {
        ChangeState(EnemyState.Dead);
        StopMovement();
        rb.simulated = false;
        if (anim) anim.SetTrigger("Die");
        Destroy(gameObject, 2f); 
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