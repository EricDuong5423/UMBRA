using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerStats playerStats;

    [Header("Components")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private StaminaSystem stamina;
    [SerializeField] private PlayerVisuals visuals;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private StatsManager statsManager;

    // ... (Các biến private giữ nguyên) ...
    private Vector2 moveInput;
    private float nextRollTime;
    private bool isRolling = false;
    private bool isAttacking = false;

    private void Start()
    {
        // Truyền PlayerHealth vào Visuals
        visuals.Initialize(playerStats, health);
    }

    // ... (Giữ nguyên toàn bộ logic Update, FixedUpdate, Attack, Roll) ...
    // ... Chỉ copy lại phần Update để đảm bảo context ...

    private void Update()
    {
        if (isRolling || isAttacking) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (health.isDead)
        {
            x = 0;
            y = 0;
        }
        moveInput = new Vector2(x, y).normalized;
        
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        visuals.UpdateMovementAnim(moveInput, isMoving);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (Time.time >= nextRollTime && stamina.TryConsumeStamina(20f)) Roll();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (stamina.TryConsumeStamina(10f)) Attack();
        }


        
        // Test Code
        if (Input.GetKeyDown(KeyCode.R)) statsManager.AddExperience(10);
    }

    private void FixedUpdate()
    {
        if (!isRolling && !isAttacking) movement.Move(moveInput);
    }

    // ... (Các hàm Attack, EndAttack, Roll, EndRoll giữ nguyên) ...
    private void Attack()
    {
        isAttacking = true;
        moveInput = Vector2.zero;
        visuals.UpdateMovementAnim(Vector2.zero, false);
        movement.StopMoving();
        visuals.TriggerAttack();
    }
    
    public void EndAttack() { isAttacking = false; }
    
    private void Roll()
    {
        isRolling = true;
        nextRollTime = Time.time + playerStats.rollCooldown;
        movement.StartRoll(moveInput, playerStats.rollSpeed);
        visuals.TriggerRoll(moveInput);
        Invoke(nameof(EndRoll), 0.2f);
    }

    private void EndRoll()
    {
        isRolling = false;
        movement.StopMoving();
    }
}