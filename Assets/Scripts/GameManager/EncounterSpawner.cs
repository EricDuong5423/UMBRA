using System.Collections.Generic;
using UnityEngine;

public class EncounterSpawner : MonoBehaviour
{
    [Header("Enemies to Spawn")]
    [SerializeField] private List<SpawnData> enemiesToSpawn;
    [SerializeField] private bool spawnOnStart = false;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnEnemies();
        }
    }
    public void SpawnEnemies()
    {
        if (enemiesToSpawn == null || enemiesToSpawn.Count == 0) return;

        foreach (var data in enemiesToSpawn)
        {
            if (data.enemyPrefab == null || data.spawnPoint == null) continue;

            for (int i = 0; i < data.count; i++)
            {
                Vector2 randomCirclePoint = Random.insideUnitCircle * data.radius;
                Vector3 randomPosition = data.spawnPoint.position + new Vector3(randomCirclePoint.x, randomCirclePoint.y, 0f);
                EnemyManager.Instance.SpawnEnemy(
                    data.enemyPrefab, 
                    randomPosition, 
                    data.spawnPoint.rotation
                );
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (enemiesToSpawn != null)
        {
            Gizmos.color = Color.red;
            foreach (var data in enemiesToSpawn)
            {
                if (data.spawnPoint != null)
                {
                    Gizmos.DrawLine(transform.position, data.spawnPoint.position);
                    Gizmos.DrawWireSphere(data.spawnPoint.position, 0.5f);
                }
            }
        }
    }
}