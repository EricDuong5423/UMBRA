using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Hitboxes & Colliders")]
    [SerializeField] private BoxCollider2D playerHitbox;      // Hitbox nhận sát thương của nhân vật
    [SerializeField] private Collider2D attackLeftCollider;   // Vũ khí bên trái
    [SerializeField] private Collider2D attackRightCollider;
    public void turnOnPlayerHitBox()
    {
        if (playerHitbox) playerHitbox.enabled = true;
    }

    public void turnOffPlayerHitBox()
    {
        // Thường dùng khi lộn (Roll) để né đòn (Invincible frame)
        if (playerHitbox) playerHitbox.enabled = false;
    }

    public void turnOnLeftAttackCollider()
    {
        if (attackLeftCollider) attackLeftCollider.enabled = true;
    }

    public void turnOffLeftAttackCollider()
    {
        if (attackLeftCollider) attackLeftCollider.enabled = false;
    }

    public void turnOnRightAttackCollider()
    {
        if (attackRightCollider) attackRightCollider.enabled = true;
    }

    public void turnOffRightAttackCollider()
    {
        if (attackRightCollider) attackRightCollider.enabled = false;
    }
}
