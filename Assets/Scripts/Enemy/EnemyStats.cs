using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Stats/Enemy Stats")]
public class EnemyStats : EntitesStats
{
    [Header("Info")]
    public string enemyName = "Monster";

    [Header("Rewards")]
    public int xpReward = 20;        // Tăng kinh nghiệm
    public float healOnKill = 10f;   // Hồi máu cho Player khi giết con này
    public uint ligthShardOnKill = 10; //Tiền sau khi giết

    [Header("AI Settings")]
    public float lookRadius = 6f;    // Tầm nhìn
    public float stopDistance = 1f;  // Khoảng cách dừng lại trước mặt Player
    
    [Header("Combat")]
    public float attackCooldown = 2f;
    
    // Hỗ trợ nhiều attack: Attack 1 (Gần/Nhanh), Attack 2 (Xa/Mạnh)
    public float attackRangeMin = 1.2f; 
    public float attackRangeMax = 2.5f;
}
