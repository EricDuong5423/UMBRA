using System;
using UnityEngine;

public class SpawningTrigger : MonoBehaviour
{
    [SerializeField] private EncounterSpawner spawner;
    [SerializeField] private SpawnData spawnData;
    [SerializeField] private bool triggerOnce = false;
    private bool hasTriggered = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggerOnce && hasTriggered) return;
        spawner.SpawnEnemies(spawnData);
        hasTriggered = true;
    }
}
