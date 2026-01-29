using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerStats playerStats;

    [Header("Components")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private StaminaSystem stamina;
    [SerializeField] private PlayerVisuals visuals;
    [SerializeField] private HealthSystem health;
    [SerializeField] private StatsManager statsManager;

    private Vector2 moveInput;
    private float nextRollTime;
    private bool isRolling = false;

    private void Start()
    {
        visuals.Initialize(playerStats, health);
    }

    private void Update()
    {
        if (isRolling) return;

        // 1. Đọc Input
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(x, y).normalized;

        // ---> CẬP NHẬT ANIMATION <---
        // Kiểm tra xem có đang bấm nút không
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        visuals.UpdateMovementAnim(moveInput, isMoving);

        // 2. Roll
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (Time.time >= nextRollTime && stamina.TryConsumeStamina(20f))
            {
                Roll();
            }
        }
        
        // 3. Attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (stamina.TryConsumeStamina(10f))
            {
                Attack();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            // health.TakeDamage(10f);
            statsManager.AddExperience(10);
        }
    }

    // ... (Giữ nguyên FixedUpdate, Attack, Roll, EndRoll)
    private void FixedUpdate()
    {
        if (!isRolling) movement.Move(moveInput);
    }

    private void Attack()
    {
        visuals.TriggerAttack();
    }
    
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