using UnityEngine;
using System.Collections;

public class PlayerHealth : EntityHealth
{
    [Header("Player Settings")]
    [SerializeField] private float iFrameDuration = 0.5f; // Thời gian bất tử
    private bool isInvincible = false;

    public override void TakeDamage(float amount, Transform source)
    {
        // Nếu đang bất tử thì bỏ qua damage
        if (isInvincible || isDead) return;

        // Gọi logic trừ máu của cha
        base.TakeDamage(amount, source);

        // Kích hoạt bất tử tạm thời
        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        // TODO: Thêm code nhấp nháy Sprite Player ở đây nếu muốn
        yield return new WaitForSeconds(iFrameDuration);
        isInvincible = false;
    }
}