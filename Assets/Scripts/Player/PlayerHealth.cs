using UnityEngine;
using System.Collections;

public class PlayerHealth : EntityHealth
{
    [Header("Player Settings")]
    [SerializeField] private float iFrameDuration = 0.5f;
    private bool isInvincible = false;

    public override void TakeDamage(float amount, Transform source)
    {
        if (isInvincible || isDead) return;

        base.TakeDamage(amount, source);
        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        // Logic nháy sprite có thể thêm ở đây
        yield return new WaitForSeconds(iFrameDuration);
        isInvincible = false;
    }
}