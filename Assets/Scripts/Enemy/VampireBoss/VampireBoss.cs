using UnityEngine;

public class VampireBoss : EnemyBase
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileDamage = 10f;
    [SerializeField] private float _knockBackForce = 10f;
    protected override void PerformAttack(int attackIndex)
    {
        if (currentState == EnemyState.Hurt) return;
        Anim.SetTrigger("Attack");
        switch (attackIndex)
        {
            case 1: SpawnProjectile(1); break;
            case 2: SpawnProjectile(10); break;
            default: break;
        }
    }

    protected override int ChooseAttackType()
    {
        int result = Random.Range(1, 2);
        return result;
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
        for (int i = 0; i < count; i++)
        {
            ProjectileManager.Instance.SpawnProjectile(_projectilePrefab, Target, this.transform, _projectileDamage,  _knockBackForce);
        }
    }
}
