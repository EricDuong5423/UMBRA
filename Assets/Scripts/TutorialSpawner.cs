using System;
using UnityEngine;

public class TutorialSpawner : MonoBehaviour
{
    [Header("Spawn Data")]
    [SerializeField] private SpawnData[] enemiesToSpawn;

    private void Start()
    {
        SpawnAllEnemies();
    }

    public void SpawnAllEnemies()
    {
        foreach (SpawnData data in enemiesToSpawn)
        {
            if (data.spawnPosition != null && data.enemyPrefab != null)
            {
                EnemyManager.Instance.SpawnEnemy(data.enemyPrefab
                                               , data.spawnPosition.position
                                               , data.spawnPosition.rotation);
            }
        }
    }
}
