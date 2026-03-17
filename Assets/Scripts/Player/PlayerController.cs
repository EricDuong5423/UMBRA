using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private float hurtDuration = 0.2f;

    private Vector2 moveInput;
    private float nextRollTime;
    private bool isRolling = false;
    private bool isAttacking = false;
    private bool isHurting = false;
    private Vector3 _baseScale;
    public static bool isMovable = true;

    private void Awake()
    {
        _baseScale = transform.localScale;
    }

    public void Initialize(PlayerHealth playerHealth)
    {
        playerHealth.OnHit += HandleGetHit;
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.PlayerHealth.OnHit -= HandleGetHit;
    }

    private void HandleGetHit(Vector2 dir)
    {
        isAttacking = false; 
        isRolling = false;
        isHurting = true;
        
        PlayerManager.Instance.PlayerMovement.StopMoving(); 
        PlayerManager.Instance.PlayerVisuals.UpdateMovementAnim(Vector2.zero, false);
        
        CancelInvoke(nameof(RecoverFromHurt));
        Invoke(nameof(RecoverFromHurt), hurtDuration);
    }
    
    private void RecoverFromHurt()
    {
        isHurting = false;
    }

    private void Update()
    {
        if (PlayerManager.Instance.PlayerHealth.IsDead || isHurting || !isMovable) 
        {
            PlayerManager.Instance.PlayerMovement.StopMoving();
            return; 
        }

        if (isRolling || isAttacking) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Flip(x);
        moveInput = new Vector2(x, y).normalized;
        
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        PlayerManager.Instance.PlayerVisuals.UpdateMovementAnim(moveInput, isMoving);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (Time.time >= nextRollTime && PlayerManager.Instance.PlayerStamina.TryConsumeStamina(20f)) Roll();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PlayerManager.Instance.PlayerStamina.TryConsumeStamina(10f)) Attack();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TabManager.Instance == null) return;
            var mainMenuCanvas = TabManager.Instance.gameObject.GetComponent<CanvasGroup>();
            if (mainMenuCanvas == null) return;
            mainMenuCanvas.alpha = TabManager.isOpened ?  0f : 1f;
            TabManager.isOpened = !TabManager.isOpened;
            GameManager.Instance.PauseGame();
        }
        
        // Test Code
        if (Input.GetKeyDown(KeyCode.R))
        {
            ItemData item = PlayerManager.Instance.PlayerItemManager.randomItemsDebug[Random.Range(0, PlayerManager.Instance.PlayerItemManager.randomItemsDebug.Count)];
            PlayerManager.Instance.PlayerItemManager.AddItem(item);
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal < 0)
        {
            transform.localScale = new Vector3(-_baseScale.x, _baseScale.y, _baseScale.z);    
        }
        else if (horizontal > 0)
        {
            transform.localScale = _baseScale;
        }
    }

    private void FixedUpdate()
    {
        if (PlayerManager.Instance.PlayerHealth.IsDead || isHurting || isAttacking) 
        {
            PlayerManager.Instance.PlayerMovement.StopMoving();
            return;
        }
        if (isRolling)
        {
            return; 
        }
        PlayerManager.Instance.PlayerMovement.Move(moveInput);
    }
    
    private void Attack()
    {
        isAttacking = true;
        moveInput = Vector2.zero;
        PlayerManager.Instance.PlayerVisuals.UpdateMovementAnim(Vector2.zero, false);
        PlayerManager.Instance.PlayerMovement.StopMoving();
        PlayerManager.Instance.PlayerVisuals.TriggerAttack();
    }
    
    public void EndAttack() { isAttacking = false; }
    
    private void Roll()
    {
        isRolling = true;
        nextRollTime = Time.time + PlayerManager.Instance.PlayerStatsManager.BaseStats.RollCooldown;
        PlayerManager.Instance.PlayerMovement.StartRoll(moveInput, PlayerManager.Instance.PlayerStatsManager.BaseStats.RollSpeed);
        PlayerManager.Instance.PlayerVisuals.TriggerRoll(moveInput);
        Invoke(nameof(EndRoll), 0.2f);
    }

    private void EndRoll()
    {
        isRolling = false;
        PlayerManager.Instance.PlayerMovement.StopMoving();
    }
}