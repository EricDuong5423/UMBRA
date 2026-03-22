using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TrapManager : MonoBehaviour
{
    [SerializeField] private float damageToPlayer = 20f;
    private bool hasDamageForTutorialMap = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Tutorial-map" && hasDamageForTutorialMap) return;
            if (SceneManager.GetActiveScene().name == "Tutorial-map") hasDamageForTutorialMap = true;
            float damage = SceneManager.GetActiveScene().name == "Tutorial-map" ? PlayerManager.Instance.PlayerHealth.MaxEmbers * (0.5f):  PlayerManager.Instance.PlayerHealth.MaxEmbers * (damageToPlayer / 100f);
            PlayerManager.Instance.PlayerHealth.TakeDamage(damage, transform, false, 0f);
            
        }
    }
}
