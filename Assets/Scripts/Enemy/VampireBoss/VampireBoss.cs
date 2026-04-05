using System.Collections;
using UnityEngine;
public class VampireBoss : EnemyBase
{
    // ═══════════════════════════════════════════════════════════════
    // PHASE SYSTEM
    // ═══════════════════════════════════════════════════════════════
    [Header("Phase Settings")]
    [SerializeField] private float phase2Threshold = 0.6f;
    [SerializeField] private float phase3Threshold = 0.3f;
    
    private int currentPhase = 1;
    private bool isInPhaseTransition = false;
    
    // ═══════════════════════════════════════════════════════════════
    // COMBAT SETTINGS
    // ═══════════════════════════════════════════════════════════════
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileDamage = 10f;
    [SerializeField] private float projectileKnockback = 10f;
    
    [Header("Minion Spawning")]
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private float minionSpawnCooldown = 8f;
    
    [Header("Telegraph")]
    [SerializeField] private float telegraphDuration = 0.8f;
    
    [Header("Lifesteal (Phase 2+)")]
    [SerializeField] private float lifestealPercent = 0.15f;
    
    [Header("Enrage (Phase 3)")]
    [SerializeField] private float enrageAttackSpeedBonus = 0.3f;
    
    private SpawnData spawnData;
    private EncounterSpawner spawner;
    private float nextMinionSpawnTime = 0f;
    private float enrageTimer = 0f;
    private bool isChargingAttack = false;
    
    [Header("Tactical Movement")]
    [SerializeField] private float strafeSpeed = 2.5f;
    [SerializeField] private float strafeDuration = 1f;
    [SerializeField] private float strafeCooldown = 2.5f;
    [SerializeField] private float approachSpeedMultiplier = 1.5f;
    [SerializeField] private float attackPauseDuration = 0.5f;
    private float nextStrafeTime = 0f;
    private bool isStrafing = false;
    private bool isApproaching = false;
    private enum BossAttackType
    {
        SingleProjectile,
        MultiProjectile,
        SummonMinions,
        ChargeBlast
    }
    public static VampireBoss Instance { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    protected void Start()
    {
        spawner = FindAnyObjectByType<EncounterSpawner>();
        if (spawner == null)
            Debug.LogError("VampireBoss: No EncounterSpawner found!");
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Instance = null;
    }
    protected override void Update()
    {
        if (currentState == EnemyState.Dead) return;
        if (Target == null) return;
        base.Update();
        
        if (!isInPhaseTransition)
        {
            CheckPhaseTransition();
            UpdateEnrage();
        }
        
        distanceToTarget = Vector2.Distance(transform.position, Target.position);
    }
    private void CheckPhaseTransition()
    {
        float healthRatio = HealthSystem.CurrentEmbers / HealthSystem.MaxEmbers;
        int newPhase = healthRatio > phase2Threshold ? 1 
                     : healthRatio > phase3Threshold ? 2 : 3;
        if (newPhase != currentPhase)
        {
            StartCoroutine(PhaseTransitionCoroutine(newPhase));
        }
    }
    private IEnumerator PhaseTransitionCoroutine(int newPhase)
    {
        isInPhaseTransition = true;
        StopMovement();
        yield return new WaitForSeconds(1f);
        currentPhase = newPhase;
        ApplyPhaseModifiers();
        isInPhaseTransition = false;
    }
    private void ApplyPhaseModifiers()
    {
        float speedMultiplier = currentPhase switch
        {
            2 => 1.3f,
            3 => 1.5f,
            _ => 1f
        };
        Debug.Log($"Phase {currentPhase} activated!");
    }
    private void UpdateEnrage()
    {
        if (currentPhase >= 3)
        {
            enrageTimer += Time.deltaTime;
            // Increase aggression over time in phase 3
        }
    }
    private IEnumerator StrafeRoutine()
    {
        isStrafing = true;
        nextStrafeTime = Time.time + strafeCooldown;
    
        // Choose perpendicular direction (left or right)
        Vector2 toPlayer = (Target.position - transform.position).normalized;
        Vector2 strafeDir = new Vector2(-toPlayer.y, toPlayer.x);
        if (Random.value > 0.5f) strafeDir *= -1;
    
        float elapsed = 0f;
        while (elapsed < strafeDuration)
        {
            if (Anim)
            {
                Anim.SetBool("IsMoving", true);
                Anim.SetFloat("X", strafeDir.x);
                Anim.SetFloat("Y", strafeDir.y);
            }
            RB.linearVelocity = strafeDir * strafeSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }
    
        isStrafing = false;
    }
    protected override void LogicChase()
    {
        if (distanceToTarget > stats.lookRadius * 1.5f)
        {
            ChangeState(stats.wandersWhenIdle ? EnemyState.Wander : EnemyState.Idle);
            return;
        }
        if (!isStrafing && Time.time >= nextStrafeTime && distanceToTarget < stats.attackRangeMax * 2f)
        {
            if (Random.value > 0.5f)
            {
                StartCoroutine(StrafeRoutine());
                return;
            }
        }
    
        if (distanceToTarget <= stats.attackRangeMax)
        {
            ChangeState(EnemyState.Attack);
            return;
        }
    
        FaceTarget();
        MoveToTarget();
    }
    protected override void LogicAttack()
    {
        StopMovement();
    
        if (distanceToTarget > stats.attackRangeMax)
        {
            isApproaching = true;
            ChangeState(EnemyState.Chase);
            return;
        }
    
        FaceTarget();
        
        if (Time.time >= nextAttackTime)
        {
            int attackType = ChooseAttackType();
            PerformAttack(attackType);
            nextAttackTime = Time.time + GetModifiedCooldown();
        }
    }
    // ═══════════════════════════════════════════════════════════════
    // CONTEXT-AWARE ATTACK SELECTION
    // ═══════════════════════════════════════════════════════════════
    protected override int ChooseAttackType()
    {
        return currentPhase switch
        {
            1 => ChoosePhase1Attack(),
            2 => ChoosePhase2Attack(),
            3 => ChoosePhase3Attack(),
            _ => 1
        };
    }
    private int ChoosePhase1Attack()
    {
        float rand = Random.value;
    
        if (distanceToTarget > stats.attackRangeMax)
        {
            // Far - projectile attacks
            if (rand > 0.6f) return (int)BossAttackType.MultiProjectile;
            return (int)BossAttackType.SingleProjectile;
        }
        else
        {
            // Close - mix of projectile and AoE
            if (rand > 0.7f && Time.time >= nextMinionSpawnTime)
                return (int)BossAttackType.SummonMinions;
            if (rand > 0.4f) return (int)BossAttackType.ChargeBlast;
            return (int)BossAttackType.SingleProjectile;
        }
    }
    private int ChoosePhase2Attack()
    {
        // Aggressive: More projectiles, dash attacks
        if (distanceToTarget > stats.attackRangeMax)
        {
            return (int)BossAttackType.MultiProjectile;
        }
        else
        {
            float rand = Random.value;
            if (rand > 0.7f && Time.time >= nextMinionSpawnTime)
                return (int)BossAttackType.SummonMinions;
            return (int)BossAttackType.MultiProjectile;
        }
    }
    private int ChoosePhase3Attack()
    {
        // Desperate: Max aggression, combos
        enrageTimer += Time.deltaTime;
        
        // More likely to do devastating attacks
        if (distanceToTarget > stats.attackRangeMax)
        {
            float rand = Random.value;
            if (rand > 0.6f) return (int)BossAttackType.ChargeBlast;
            if (rand > 0.3f) return (int)BossAttackType.MultiProjectile;
        }
        else
        {
            float rand = Random.value;
            if (rand > 0.5f && Time.time >= nextMinionSpawnTime)
                return (int)BossAttackType.SummonMinions;
            if (rand > 0.2f) return (int)BossAttackType.ChargeBlast;
        }

        return (int)BossAttackType.SingleProjectile;
    }
    private float GetModifiedCooldown()
    {
        float baseCooldown = stats.attackCooldown;
        
        // Phase 3 enrage reduces cooldown
        if (currentPhase >= 3)
        {
            baseCooldown *= (1f - enrageTimer * 0.02f); // Gets faster over time
            baseCooldown = Mathf.Max(baseCooldown, 0.5f); // Minimum 0.5s
        }
        
        return baseCooldown;
    }
    // ═══════════════════════════════════════════════════════════════
    // ATTACK EXECUTION
    // ═══════════════════════════════════════════════════════════════
    protected override void PerformAttack(int attackIndex)
    {
        if (currentState == EnemyState.Hurt) return;
        BossAttackType attackType = (BossAttackType)attackIndex;
    
        switch (attackType)
        {
            case BossAttackType.SingleProjectile:
                StartCoroutine(SingleProjectileRoutine());
                break;
            case BossAttackType.MultiProjectile:
                StartCoroutine(MultiProjectileRoutine());
                break;
            case BossAttackType.SummonMinions:
                SpawnMinions();
                break;
            case BossAttackType.ChargeBlast:
                StartCoroutine(ChargeBlastRoutine());
                break;
        }
    }
    // ═══════════════════════════════════════════════════════════════
    // ATTACK ROUTINES
    // ═══════════════════════════════════════════════════════════════
    private IEnumerator SingleProjectileRoutine()
    {
        if (Anim) Anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.2f);
        
        FireProjectile();
    }
    private IEnumerator MultiProjectileRoutine()
    {
        if (Anim) Anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);
        
        int count = currentPhase >= 2 ? 5 : 3;
        float spreadAngle = currentPhase >= 2 ? 60f : 40f;
        
        Vector2 predictedPos = PredictPlayerMovement(0.3f);
        Vector2 baseDir = (predictedPos - (Vector2)transform.position).normalized;
        
        for (int i = 0; i < count; i++)
        {
            float angle = Mathf.Lerp(-spreadAngle / 2f, spreadAngle / 2f, i / (float)(count - 1));
            Vector2 dir = Quaternion.Euler(0, 0, angle) * baseDir;
            FireProjectile();
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void SpawnMinions()
    {
        if (spawner == null || minionPrefab == null) return;
        
        int count = currentPhase switch
        {
            1 => 1,
            2 => Random.Range(2, 4),
            3 => Random.Range(3, 5),
            _ => 1
        };
        spawnData = new SpawnData();
        spawnData.enemyPrefab = minionPrefab;
        spawnData.radius = 3f;
        spawnData.count = count;
        spawnData.spawnPoint = transform;
        
        spawner.SpawnEnemies(spawnData);
        nextMinionSpawnTime = Time.time + minionSpawnCooldown;
    }
    private IEnumerator ChargeBlastRoutine()
    {
        isChargingAttack = true;
        
        if (Anim) Anim.SetBool("IsMoving", true);
        
        Vector2 startPos = transform.position;
        Vector2 targetPos = Target.position;
        
        yield return new WaitForSeconds(telegraphDuration * 1.2f);
        
        if (Anim) Anim.SetBool("IsMoving", false);
        Vector2 dirToPlayer = (targetPos - (Vector2)transform.position).normalized;
        float coneAngle = 90f;
        float range = currentPhase >= 3 ? 6f : 4f;
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        
        foreach (var hit in hits)
        {
            Vector2 toTarget = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;
            float angle = Vector2.Angle(dirToPlayer, toTarget);
            
            if (angle < coneAngle / 2f && hit.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(projectileDamage, transform, true, projectileKnockback * 1.5f);
            }
        }
        
        ApplyLifesteal();
        isChargingAttack = false;
    }
    // ═══════════════════════════════════════════════════════════════
    // HELPER METHODS
    // ═══════════════════════════════════════════════════════════════
    private void FireProjectile()
    {
        if (projectilePrefab == null) return;
        ProjectileManager.Instance.SpawnProjectile(
            projectilePrefab,
            Target,
            transform,
            projectileDamage,
            projectileKnockback
        );
    }
    private Vector2 PredictPlayerMovement(float leadTime)
    {
        if (Target == null) return transform.position;
        
        Rigidbody2D playerRb = Target.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            return (Vector2)Target.position + playerRb.linearVelocity * leadTime;
        }
        
        return Target.position;
    }
    private void ApplyLifesteal()
    {
        if (currentPhase < 2) return;
        
        // Heal based on damage dealt (simplified)
        float healAmount = projectileDamage * lifestealPercent;
        HealthSystem.Heal(healAmount);
    }
    protected override void MoveToTarget()
    {
        if (isStrafing) return;
    
        if (Anim) Anim.SetBool("IsMoving", true);
    
        Vector2 direction = (Target.position - transform.position).normalized;
        Anim.SetFloat("X", direction.x);
        Anim.SetFloat("Y", direction.y);
    
        float speed = StatsManager?.MoveSpeed ?? stats.BaseMoveSpeed;
    
        // Phase speed modifier
        speed *= currentPhase switch
        {
            2 => 1.3f,
            3 => 1.5f,
            _ => 1f
        };
    
        // Faster approach when closing distance
        if (distanceToTarget > stats.attackRangeMax * 0.5f)
        {
            speed *= approachSpeedMultiplier;
        }
    
        RB.linearVelocity = direction * speed;
    }
    protected override void FaceDirection(Vector2 dir) { }
    protected override void FaceTarget() { }
    protected override void StopMovement()
    {
        if (Anim) Anim.SetBool("IsMoving", false);
        RB.linearVelocity = Vector2.zero;
    }
    // ═══════════════════════════════════════════════════════════════
    // HIT & DEATH HANDLING
    // ═══════════════════════════════════════════════════════════════
    protected override void HandleHit(Vector2 dir, float knockbackForce)
    {
        if (currentState == EnemyState.Dead) return;
        
        ChangeState(EnemyState.Hurt);
        DisableEnemyHitBox();
        StopMovement();
        isChargingAttack = false;
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
        Instance = null;
        if (Anim)
        {
            Anim.ResetTrigger("Hurt");
            Anim.ResetTrigger("Attack");
            Anim.SetTrigger("isDead");
        }
    }
}