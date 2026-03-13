using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Stats/Enemy Stats")]
public class EnemyStats : EntitesStats
{
    [Header("Info")]
    public string enemyName = "Monster";

    [Header("Rewards")]
    public float healOnKill = 10f;
    public uint ligthShardOnKill = 10;

    [Header("AI Settings")]
    public float lookRadius = 6f;
    public float stopDistance = 1f;
    
    [Header("Combat")]
    public float attackCooldown = 2f;
    public float attackRangeMin = 1.2f; 
    public float attackRangeMax = 2.5f;
}
