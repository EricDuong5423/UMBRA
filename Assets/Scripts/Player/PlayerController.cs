using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private float hurtDuration = 0.2f;

    [Header("Components")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private StaminaSystem stamina;
    [SerializeField] private PlayerVisuals visuals;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private StatsManager statsManager;

    private Vector2 moveInput;
    private float nextRollTime;
    private bool isRolling = false;
    private bool isAttacking = false;
    private bool isHurting = false;

    private void Start()
    {
        visuals.Initialize(playerStats, health);
        health.OnHit += HandleGetHit;
    }

    private void OnDestroy()
    {
        health.OnHit -= HandleGetHit;
    }

    private void HandleGetHit(Vector2 dir)
    {
        isAttacking = false; 
        isRolling = false;
        isHurting = true;
        
        movement.StopMoving(); 
        visuals.UpdateMovementAnim(Vector2.zero, false);
        
        CancelInvoke(nameof(RecoverFromHurt)); // Hủy lệnh cũ nếu bị đánh liên tiếp
        Invoke(nameof(RecoverFromHurt), hurtDuration);
    }
    
    private void RecoverFromHurt()
    {
        isHurting = false;
    }

    private void Update()
    {
        // 1. Kiểm tra chết đầu tiên
        if (health.IsDead || isHurting) 
        {
            movement.StopMoving(); // Đảm bảo xác không trôi
            return; 
        }

        if (isRolling || isAttacking) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
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
        if (Input.GetKeyDown(KeyCode.R)) statsManager.AddExperience(100);
    }

    private void FixedUpdate()
    {
        if (!isRolling && !isAttacking && !health.IsDead && !isHurting) 
            movement.Move(moveInput);
        else 
            movement.StopMoving();
    }

    // ... (Giữ nguyên Attack, Roll, EndRoll như cũ) ...
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