using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    [Header("Reward Data")]
    [SerializeField] private EnemyStats stats;
    [Header("Loot Settings")]
    [SerializeField] private LootTable myLootTable;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        if (enemyHealth) enemyHealth.OnDeath += GiveReward;
    }

    private void OnDestroy()
    {
        if (enemyHealth) enemyHealth.OnDeath -= GiveReward;
    }

    private void GiveReward()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            var playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth) playerHealth.Heal(stats.healOnKill);

            var playerCoin = player.GetComponent<CoinSystem>();
            if(playerCoin) playerCoin.AddCoins(stats.ligthShardOnKill);
        }

        if (LootManager.Instance != null && myLootTable != null)
        {
            LootManager.Instance.TryDropItem(transform.position, myLootTable);
        }

        ItemManager inventory = player.GetComponent<ItemManager>();
        if (!inventory) return;
        inventory.TriggerOnKillEnemyEffect();
    }
}