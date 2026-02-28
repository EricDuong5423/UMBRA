using System;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    //Su kien
    public event Action<uint> OnCoinChanged;
    //Getter
    private uint currentCoins;
    public uint CurrentCoins => currentCoins;

    private void Awake()
    {
        currentCoins = 0;
    }

    private void Start()
    {

        BroadCastCoins();
    }

    public void AddCoins(uint amount)
    {
        currentCoins += amount;
        BroadCastCoins();
    }

    public void MinusCoins(uint amount)
    {
        currentCoins -= amount;
        BroadCastCoins();
    }

    public bool TrySpendCoinsBuy(uint price)
    {
        if (currentCoins >= price)
        {
            currentCoins -= price;
            BroadCastCoins();
            return true;
        }
        return false;
    }

    public bool CanAfford(uint price)
    {
        return currentCoins >= price;
    }

    private void BroadCastCoins()
    {
        OnCoinChanged?.Invoke(currentCoins);
    }
}
