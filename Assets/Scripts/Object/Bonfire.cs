using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Bonfire : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactionSection;
    [SerializeField] private GameObject initSection;
    [SerializeField] private GameObject successSection;
    [SerializeField] private GameObject failSection;
    private PlayerHealth playerHealth;
    private CoinSystem playerCoin;
    private Interactor playerInteractor;

    [SerializeField] private uint coinAmountRequired = 100;
    [SerializeField] private float healPercentAmount = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player"))
        {
            playerInteractor =  other.gameObject.GetComponent<Interactor>();
            playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerCoin = other.gameObject.GetComponent<CoinSystem>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player"))
        {
            playerHealth = null;
            playerCoin = null;
            playerInteractor = null;
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

    public void StopInteraction()
    {
        PlayerController.isMovable = true;
        playerHealth = null;
        playerCoin = null;
        successSection.SetActive(false);
        failSection.SetActive(false);
        initSection.SetActive(true);
        interactionSection.SetActive(false);
        playerInteractor.TurnOnInteractIcon();
    }
    
    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        PlayerController.isMovable = false;
        interactionSection.SetActive(true);
    }
}
