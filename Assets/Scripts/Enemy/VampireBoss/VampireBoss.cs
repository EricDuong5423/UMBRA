using UnityEngine;

public class VampireBoss : EnemyBase
{
    [SerializeField] private GameObject _projectilePrefab;
    protected override void PerformAttack(int attackIndex)
    {
        if (currentState == EnemyState.Hurt) return;
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

    private void SpawnProjectile(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            var projectile_animator = projectile.GetComponent<Animator>();
            var projectile_rigidbody = projectile.GetComponent<Rigidbody2D>();
            Vector2 direction = (Target.position - transform.position).normalized;
            projectile_animator.SetFloat("X", direction.x);
            projectile_animator.SetFloat("Y", direction.y);
            projectile_rigidbody.AddForce(direction * 2f, ForceMode2D.Impulse);
        }
    }
}
