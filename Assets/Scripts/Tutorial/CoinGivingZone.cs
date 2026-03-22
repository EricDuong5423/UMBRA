using System;
using UnityEngine;

public class CoinGivingZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.Instance.PlayerCoinSystem.AddCoins(100);
        }
    }
}
