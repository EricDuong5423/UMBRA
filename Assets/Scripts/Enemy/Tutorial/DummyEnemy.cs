using UnityEngine;

public class DummyEnemy : EnemyBase
{
    protected override void LogicIdle()
    {
        StopMovement();
        Vector2 faceDir = ((Vector2)transform.position - (Vector2)Target.position).normalized;
        FaceDirection(-faceDir);
    }

    protected override void LogicAttack()
    {
        StopMovement();
        Vector2 faceDir = ((Vector2)transform.position - (Vector2)Target.position).normalized;
        FaceDirection(-faceDir);
    }

    protected override void LogicChase()
    {
        StopMovement();
        Vector2 faceDir = ((Vector2)transform.position - (Vector2)Target.position).normalized;
        FaceDirection(-faceDir);
    }

    protected override void LogicFlee()
    {
        StopMovement();
        Vector2 faceDir = ((Vector2)transform.position - (Vector2)Target.position).normalized;
        FaceDirection(-faceDir);
    }

    protected override void LogicHurt()
    {
        Vector2 faceDir = ((Vector2)transform.position - (Vector2)Target.position).normalized;
        FaceDirection(-faceDir);
        if (Time.time >= hurtEndTime)
        {
            StopMovement();
            ChangeState(EnemyState.Idle);
        }
    }

    protected override void HandleDeath()
    {
        Vector2 faceDir = ((Vector2)transform.position - (Vector2)Target.position).normalized;
        FaceDirection(-faceDir);
        base.HandleDeath();
    }

    protected override void PerformAttack(int attackIndex)
    {
        
    }
}
