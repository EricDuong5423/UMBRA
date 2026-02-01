using System;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [Header("Data connection")]
    [SerializeField] private CoinSystem coinSystem;
    [SerializeField] private TMP_Text coinText;

    private void Start()
    {
        if (coinSystem != null)
        {
            coinSystem.OnCoinChanged += ChangeCoinText;
            ChangeCoinText(coinSystem.CurrentCoins);
        }
    }

    private void OnDestroy()
    {
        if (coinSystem != null)
        {
            coinSystem.OnCoinChanged -= ChangeCoinText;
        }
    }

    private void ChangeCoinText(uint newCurrentCoin)
    {
        coinText.text = $"x{newCurrentCoin:N0}";
    }
}
