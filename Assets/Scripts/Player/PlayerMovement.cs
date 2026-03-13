using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerStatsManager playerStatsManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Initialize(PlayerStatsManager statsManager)
    {
        playerStatsManager = statsManager;
    }

    public void Move(Vector2 direction)
    {
        rb.linearVelocity = direction * playerStatsManager.MoveSpeed;
    }

    public void StartRoll(Vector2 direction, float rollSpeed)
    {
        rb.linearVelocity = direction * rollSpeed;
    }
    
    public void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
    }
}