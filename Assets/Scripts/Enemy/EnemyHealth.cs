using UnityEngine;

public class EnemyHealth : EntityHealth, IEnemyComponent
{
    private EnemyBase _brain;

    public void Initialize(EnemyBase brain)
    {
        _brain = brain;
        InitializeHealth(brain.Stats.MaxEmbers);
    }

    public override void TakeDamage(float damage, Transform source)
    {
        base.TakeDamage(damage, source);
        if (_brain != null && !IsDead)
        {
            _brain.ChangeState(EnemyState.Hurt);
            _brain.Physics.StopMoving();
        }
    }

    protected override void Die()
    {
        base.Die();

        if (_brain != null)
        {
            _brain.ChangeState(EnemyState.Dead);
            _brain.Physics.StopMoving();
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
            EnemyManager.Instance.ReturnEnemy(_brain.originalPrefab, _brain.gameObject);
        }
    }
}