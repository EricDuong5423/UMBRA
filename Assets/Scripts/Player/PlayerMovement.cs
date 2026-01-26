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
        
        // Setup Rigidbody cho game Top-Down
        rb.gravityScale = 0; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Move(Vector2 direction)
    {
        // Di chuyển bằng vận tốc vật lý
        rb.linearVelocity = direction * statsManager.MoveSpeed;
    }

    public void StartRoll(Vector2 direction, float rollSpeed)
    {
        // Logic roll đơn giản: đẩy mạnh một cái
        // (Để làm roll mượt hơn bạn có thể dùng Coroutine hoặc Animation Curve sau này)
        rb.linearVelocity = direction * rollSpeed;
    }
    
    public void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
    }
}