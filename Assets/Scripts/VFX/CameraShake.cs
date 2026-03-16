using System;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    private CinemachineCamera _camera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _camera = GetComponent<CinemachineCamera>();
    }

    private void OnShake(float duration, float strength)
    {
        var sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => 0f, x => _camera.Lens.Dutch = x, strength, duration));
        sequence.Append(DOTween.To(() => strength, x => _camera.Lens.Dutch = x, 0, duration));
        sequence.SetEase(Ease.InOutBounce).Play();
    }
    
    public static void Shake(float duration, float strength) => Instance.OnShake(duration, strength);
}
