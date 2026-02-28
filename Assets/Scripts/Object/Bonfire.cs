using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Bonfire : MonoBehaviour
{
    [SerializeField] private GameObject interactionSection;
    [SerializeField] private GameObject initSection;
    [SerializeField] private GameObject successSection;
    [SerializeField] private GameObject failSection;
    private PlayerHealth playerHealth;
    private CoinSystem playerCoin;

    [SerializeField] private uint coinAmountRequired = 100;
    [SerializeField] private float healPercentAmount = 0.5f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player"))
        {
            if (interactionSection) interactionSection.SetActive(true);
            playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerCoin = other.gameObject.GetComponent<CoinSystem>();
        }
    }

    public void Healing()
    {
        if (playerCoin.CurrentCoins < coinAmountRequired)
        {
            failSection.SetActive(true);
            return;
        }
        successSection.SetActive(true);
        float healAmount = (playerHealth.MaxEmbers - playerHealth.CurrentEmbers) * healPercentAmount;
        playerHealth.Heal(healAmount);
        playerCoin.MinusCoins(100);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player"))
        {
            if (interactionSection) interactionSection.SetActive(false);
            playerHealth = null;
            playerCoin = null;
            successSection.SetActive(false);
            failSection.SetActive(false);
            initSection.SetActive(true);
        }
    }
}
