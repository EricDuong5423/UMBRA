using UnityEngine;

public class EnemyAnimator : MonoBehaviour, IEnemyComponent
{
    private EnemyBase brain;
    private Animator anim;
    private SpriteRenderer sr;

    public void Initialize(EnemyBase brain)
    {
        this.brain = brain;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        brain.Health.OnHit += HandleHit;
        brain.Health.OnDeath += HandleDeath;
        if (brain.Combat != null) brain.Combat.OnAttackPerformed += TriggerAttack;
    }

    private void OnDestroy()
    {
        if (brain == null) return;
        if (brain.Health != null)
        {
            brain.Health.OnHit -= HandleHit;
            brain.Health.OnDeath -= HandleDeath;
        }
        if (brain.Combat != null) brain.Combat.OnAttackPerformed -= TriggerAttack;
    }

    private void Update()
    {
        if (brain.CurrentState == EnemyState.Dead) return;

        if (brain.Target != null)
        {
            float dirX = brain.Target.position.x - transform.position.x;
            if (dirX != 0) sr.flipX = dirX < 0;
        }

        if (anim) anim.SetBool("IsMoving", brain.CurrentState == EnemyState.Chase && !brain.Physics.IsFrozen);
    }

    private void HandleHit(float dmg, Transform src) { if (anim) anim.SetTrigger("Hurt"); }
    private void HandleDeath() { if (anim) anim.SetTrigger("Die"); }
    private void TriggerAttack() { if (anim) anim.SetTrigger("Attack1"); }
}