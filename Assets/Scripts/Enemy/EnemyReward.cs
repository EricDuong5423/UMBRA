using UnityEngine;

public class EnemyReward : MonoBehaviour, IEnemyComponent
{
    private EnemyBase _brain;

    public void Initialize(EnemyBase brain)
    {
        _brain = brain;
        brain.Health.OnDeath += GiveReward;
    }

    private void OnDestroy()
    {
        if (_brain != null && _brain.Health != null) _brain.Health.OnDeath -= GiveReward;
    }

    private void GiveReward()
    {
        if (LootManager.Instance != null && _brain.Stats != null && _brain.Stats.LootTable != null)
        {
            LootManager.Instance.TryDropItem(transform.position, _brain.Stats.LootTable);
        }
    }
}