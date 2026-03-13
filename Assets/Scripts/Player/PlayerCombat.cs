using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Weapon References (Child Objects)")] 
    [SerializeField] private Collider2D normalAttackCollider;

    private PlayerStatsManager playerStatsManager;

    public void Initialize(PlayerStatsManager statsManager)
    {
        playerStatsManager = statsManager;
        SetupWeapon(normalAttackCollider);
    }

    private void SetupWeapon(Collider2D col)
    {
        if (col != null)
        {
            var hitbox = col.GetComponent<PlayerWeaponHitbox>();
            if (hitbox == null) hitbox = col.gameObject.AddComponent<PlayerWeaponHitbox>();
            hitbox.Initialize(playerStatsManager, transform);
            col.enabled = false; 
        }
    }

    public void TurnOnNormalAttackCollider()
    {
        if(normalAttackCollider) normalAttackCollider.enabled = true;
    }

    public void TurnOffNormalAttackCollider()
    {
        if(normalAttackCollider) normalAttackCollider.enabled = false;
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