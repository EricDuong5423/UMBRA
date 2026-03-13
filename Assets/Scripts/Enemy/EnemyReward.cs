using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    private EnemyBase enemyBase;

    [Header("Reward Data")]
    [SerializeField] private EnemyStats stats;
    [Header("Loot Settings")]
    [SerializeField] private LootTable myLootTable;

    public void Initialize(EnemyBase manager)
    {
        enemyBase = manager;
        
        enemyBase.HealthSystem.OnDeath -= GiveReward;
        enemyBase.HealthSystem.OnDeath += GiveReward;
    }

    private void OnDestroy()
    {
        if (enemyBase != null && enemyBase.HealthSystem != null) 
            enemyBase.HealthSystem.OnDeath -= GiveReward;
    }

    private void GiveReward()
    {
        if (enemyBase.Target != null)
        {
            var playerHealth = enemyBase.Target.GetComponent<PlayerHealth>();
            if (playerHealth) playerHealth.Heal(stats.healOnKill);

            var playerCoin = enemyBase.Target.GetComponent<CoinSystem>();
            if(playerCoin) playerCoin.AddCoins(stats.ligthShardOnKill);

            ItemManager inventory = enemyBase.Target.GetComponent<ItemManager>();
            if (inventory) inventory.TriggerOnKillEnemyEffect();
        }

        if (LootManager.Instance != null && myLootTable != null)
        {
            LootManager.Instance.TryDropItem(transform.position, myLootTable);
        }
    }
}