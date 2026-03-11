using UnityEngine;

public class EnemySound : MonoBehaviour, IEnemyComponent
{
    private EnemyBase _brain;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] hitSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip[] attackSound;

    public void Initialize(EnemyBase brain)
    {
        _brain = brain;

        brain.Health.OnHit += PlayHit;
        brain.Health.OnDeath += PlayDeath;
        if (brain.Combat != null) brain.Combat.OnAttackPerformed += PlayAttack;
    }

    private void OnDestroy()
    {
        if (_brain == null) return;
        if (_brain.Health != null)
        {
            _brain.Health.OnHit -= PlayHit;
            _brain.Health.OnDeath -= PlayDeath;
        }
        if (_brain.Combat != null) _brain.Combat.OnAttackPerformed -= PlayAttack;
    }

    private void PlayHit(float dmg, Transform src)
    {
        AudioClip clip = hitSound[Random.Range(0, hitSound.Length)];
        if (AudioController.Instance == null) return;
        AudioController.Instance.PlaySFXSound(clip);
    }

    private void PlayDeath()
    {
        if (AudioController.Instance == null) return;
        AudioController.Instance.PlaySFXSound(deathSound);
    }

    private void PlayAttack()
    {
        AudioClip clip = attackSound[Random.Range(0, hitSound.Length)];
        if (AudioController.Instance == null) return;
        AudioController.Instance.PlaySFXSound(clip);
    }
}