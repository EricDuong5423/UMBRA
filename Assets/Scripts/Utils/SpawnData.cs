using UnityEngine;

[System.Serializable]
public struct SpawnData
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    [Min(1)] public int count; 
    [Min(0f)] public float radius;
}