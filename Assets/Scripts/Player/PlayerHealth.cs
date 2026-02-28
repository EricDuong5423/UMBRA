using UnityEngine;
using System.Collections;

public class PlayerHealth : EntityHealth
{
    [Header("Player Settings")]
    [SerializeField] private float iFrameDuration = 0.5f;
    public static bool isInvincible = false;

    public override void TakeDamage(float amount, Transform source)
    {
        if (isInvincible || isDead) return;

        base.TakeDamage(amount, source);
        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(iFrameDuration);
        isInvincible = false;
    }
}