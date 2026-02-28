using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Weapon References (Child Objects)")]
    [SerializeField] private Collider2D attackLeftCollider;
    [SerializeField] private Collider2D attackRightCollider;

    private StatsManager statsManager;

    private void Awake()
    {
        statsManager = GetComponent<StatsManager>();
    }

    private void Start()
    {
        // Tự động setup cho 2 vũ khí
        SetupWeapon(attackLeftCollider);
        SetupWeapon(attackRightCollider);
    }

    private void SetupWeapon(Collider2D col)
    {
        if (col != null)
        {
            // 1. Đảm bảo có script Hitbox
            var hitbox = col.GetComponent<PlayerWeaponHitbox>();
            if (hitbox == null) hitbox = col.gameObject.AddComponent<PlayerWeaponHitbox>();
            
            // 2. Nạp chỉ số damage
            hitbox.Initialize(statsManager.AttackDamage, transform);
            
            // 3. Tắt hitbox mặc định
            col.enabled = false; 
        }
    }

    // --- ANIMATION EVENTS (Đặt tên chuẩn PascalCase) ---

    // Gọi khi animation chém trái bắt đầu gây dmg
    public void TurnOnLeftAttackCollider()
    {
        if (attackLeftCollider) attackLeftCollider.enabled = true;
    }

    public void TurnOffLeftAttackCollider()
    {
        if (attackLeftCollider) attackLeftCollider.enabled = false;
    }

    // Gọi khi animation chém phải bắt đầu gây dmg
    public void TurnOnRightAttackCollider()
    {
        if (attackRightCollider) attackRightCollider.enabled = true;
    }

    public void TurnOffRightAttackCollider()
    {
        if (attackRightCollider) attackRightCollider.enabled = false;
    }
    
    public void EnableIFrame()
    {
        PlayerHealth.isInvincible = true;
    }
    
    public void DisableIFrame()
    {
        PlayerHealth.isInvincible = false;
    }
}