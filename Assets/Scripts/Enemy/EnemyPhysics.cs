using System;
using UnityEngine;

public class EnemyPhysics : MonoBehaviour, IEnemyComponent
{
    private Rigidbody2D _rb;
    private EnemyBase _brain;
    public bool IsFrozen { get;  private set; }

    public void Initialize(EnemyBase brain)
    {
        brain = _brain;
        _rb = GetComponent<Rigidbody2D>();
        IsFrozen = false;
        _rb.simulated = true;
    }
    
    private void Update()
    {
        if (_brain.CurrentState == EnemyState.Dead || IsFrozen) return;
        
        if (_brain.CurrentState == EnemyState.Chase && _brain.Target != null)
        {
            Vector2 direction = (_brain.Target.position - transform.position).normalized;
            Move(direction, _brain.Stats.MoveSpeed);
        }
        else
        {
            StopMoving();
        }
    }

    public void Move(Vector2 direction, float speed)
    {
        if (IsFrozen) return;
        _rb.linearVelocity = direction * speed;
    }
    
    public void StopMoving()
    {
        _rb.linearVelocity = Vector2.zero;
    }
    
    public void SetFrozen(bool state)
    {
        IsFrozen = state;
        StopMoving();
        _brain.ChangeState(state ? EnemyState.Frozen : EnemyState.Idle);
    }
}
