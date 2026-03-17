using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Stats/Enemy Stats")]
public class EnemyStats : EntitesStats
{
    [Header("Info")]
    public string enemyName = "Monster";

    [Header("Rewards")]
    public float healOnKill = 10f;
    public uint ligthShardOnKill = 10;

    [Header("AI - Detection")]
    public float lookRadius = 6f;
    public float stopDistance = 1f;

    [Header("AI - Combat")]
    public float attackCooldown = 2f;
    public float attackRangeMin = 1.2f;
    public float attackRangeMax = 2.5f;

    [Header("AI - Wander")]
    public bool wandersWhenIdle = false;
    public float wanderRadius = 3f;
    public float wanderInterval = 2f;

    [Header("AI - Flee")]
    public bool fleesWhenLowHealth = false;
    [Range(0f, 1f)] public float fleeHealthThreshold = 0.25f;
}