using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TrapManager : MonoBehaviour
{
    [SerializeField] private float damageToPlayer = 20f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{PlayerManager.Instance.PlayerHealth.CurrentEmbers * (damageToPlayer/ 100f)}");
            PlayerManager.Instance.PlayerHealth.TakeDamage(PlayerManager.Instance.PlayerHealth.CurrentEmbers * (damageToPlayer/ 100f), transform, false, 0f);
        }
    }
}
