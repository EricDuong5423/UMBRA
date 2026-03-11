using UnityEngine;
[System.Serializable]
public class SpawnData
{
    [Header("Position")] 
    [field: SerializeField] public Transform spawnPosition;
    [Header("Enemy prefab")]
    [field: SerializeField] public GameObject enemyPrefab;
}