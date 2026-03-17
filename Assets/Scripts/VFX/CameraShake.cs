using Unity.Cinemachine;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class CameraShake : MonoBehaviour
{
    [Header("Shake Scale")]
    [SerializeField] private float minStrength = 0.05f;
    [SerializeField] private float maxStrength = 0.5f;
    [SerializeField] private float damageScaleMultiplier = 0.015f;

    [Header("Rotation")]
    [SerializeField] private float maxRotationAngle = 2f;
    [SerializeField] private float rotationDuration = 0.15f;

    private CinemachineImpulseSource impulseSource;
    private CinemachineCamera cinemachineCamera;
    private PlayerWeaponHitbox hitbox;
    private Tween rotationTween;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

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

    private void OnHitLanded(float damage, Vector2 hitDirection, bool _)
    {
        float strength = Mathf.Clamp(
            Mathf.Log10(1 + damage) * damageScaleMultiplier * 10f,
            minStrength,
            maxStrength
        );

        Vector3 shakeDir = new Vector3(-hitDirection.x, -hitDirection.y, 0f) * strength;
        impulseSource.GenerateImpulse(shakeDir);

        float angle = Random.Range(-maxRotationAngle, maxRotationAngle) * (strength / maxStrength);
        rotationTween?.Kill();
        rotationTween = DOTween.Sequence()
            .Append(DOTween.To(
                () => cinemachineCamera.Lens.Dutch,
                v => { var l = cinemachineCamera.Lens; l.Dutch = v; cinemachineCamera.Lens = l; },
                angle, rotationDuration * 0.4f).SetEase(Ease.OutQuad))
            .Append(DOTween.To(
                () => cinemachineCamera.Lens.Dutch,
                v => { var l = cinemachineCamera.Lens; l.Dutch = v; cinemachineCamera.Lens = l; },
                0f, rotationDuration * 0.6f).SetEase(Ease.InOutCubic));
    }
}