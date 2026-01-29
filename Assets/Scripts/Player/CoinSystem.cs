using System;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    //Su kien
    public event Action<uint> OnCoinChanged;
    //Getter
    private uint currentCoins;
    public uint CurrentCoins => currentCoins;
    [SerializeField] private StatsManager stats;

    private void Awake()
    {
        stats = GetComponent<StatsManager>();

        if (stats != null)
        {
            currentCoins = 0;
        }
    }

    private void Start()
    {
        if (stats != null)
        {
            
        }

        BroadCastCoins();
    }
    
    

    private void BroadCastCoins()
    {
        OnCoinChanged?.Invoke(currentCoins);
    }
}
