using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    [Header("Hit Stop")]
    [SerializeField] private float minStopDuration = 0.03f;
    [SerializeField] private float maxStopDuration = 0.1f;
    [SerializeField] private float maxDamageForScale = 100f;

    private PlayerWeaponHitbox hitbox;

    private void Start()
    {
        hitbox = PlayerManager.Instance.PlayerWeaponHitbox;
        if (hitbox != null)
            hitbox.OnHitLanded += OnHitLanded;
    }

    private void OnDestroy()
    {
        if (hitbox != null)
            hitbox.OnHitLanded -= OnHitLanded;
    }

    private void OnHitLanded(float damage, Vector2 _, bool isCrit)
    {
        if (!isCrit) return;
        float duration = Mathf.Lerp(minStopDuration, maxStopDuration, damage / maxDamageForScale);
        StartCoroutine(FreezeRoutine(duration));
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}