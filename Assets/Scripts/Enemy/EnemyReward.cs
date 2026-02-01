using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;

    private EnemyHealth healthSystem; // Đổi thành EnemyHealth

    private void Awake()
    {
        healthSystem = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        if (healthSystem) healthSystem.OnDeath += GiveReward;
    }

    private void OnDestroy()
    {
        if (healthSystem) healthSystem.OnDeath -= GiveReward;
    }

    private void GiveReward()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            var playerStats = player.GetComponent<StatsManager>();
            if (playerStats) playerStats.AddExperience(stats.xpReward);

            // Tìm PlayerHealth để hồi máu
            var playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth) playerHealth.Heal(stats.healOnKill);

            var playerCoin = player.GetComponent<CoinSystem>();
            if(playerCoin) playerCoin.AddCoins(stats.ligthShardOnKill);
        }
    }
}