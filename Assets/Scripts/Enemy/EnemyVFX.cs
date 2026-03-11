using UnityEngine;

public class EnemyVFX : MonoBehaviour, IEnemyComponent
{
    private EnemyBase _brain;

    public void Initialize(EnemyBase brain)
    {
        _brain = brain;
        brain.Health.OnHit += HandleHitVFX;
        brain.Health.OnDoTHit += HandleDoTVFX;
    }

    private void OnDestroy()
    {
        if (_brain != null && _brain.Health != null)
        {
            _brain.Health.OnHit -= HandleHitVFX;
            _brain.Health.OnDoTHit -= HandleDoTVFX;
        }
    }

    private void HandleHitVFX(float dmg, Transform src) => DamageTextManager.Instance.SpawnDamageText(dmg.ToString(), Color.red, transform);
    private void HandleDoTVFX(float dmg) => DamageTextManager.Instance.SpawnDamageText(dmg.ToString(), Color.green, transform);
}
