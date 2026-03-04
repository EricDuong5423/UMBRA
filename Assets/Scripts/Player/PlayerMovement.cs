using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private StatsManager statsManager; // Để lấy MoveSpeed

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        statsManager = GetComponent<StatsManager>();
        rb.gravityScale = 0; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Move(Vector2 direction)
    {
        rb.linearVelocity = direction * statsManager.MoveSpeed;
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