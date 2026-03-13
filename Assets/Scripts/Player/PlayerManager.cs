using UnityEngine;

[RequireComponent(typeof(PlayerStatsManager))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(PlayerVisuals))]
[RequireComponent(typeof(PlayerEffectManager))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(ItemManager))]
[RequireComponent(typeof(CoinSystem))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(StaminaSystem))]
[RequireComponent(typeof(PlayerController))]
public class PlayerManager : MonoBehaviour
{
    public PlayerStatsManager PlayerStatsManager { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerCombat PlayerCombat { get; private set; }
    public PlayerVisuals PlayerVisuals  { get; private set; }
    public PlayerEffectManager PlayerEffectManager { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public ItemManager PlayerItemManager { get; private set; }
    public CoinSystem PlayerCoinSystem { get; private set; }
    public StaminaSystem PlayerStamina { get; private set; }
    public PlayerController PlayerController { get; private set; }
    
    public static PlayerManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        PlayerStatsManager = GetComponent<PlayerStatsManager>();
        PlayerHealth = GetComponent<PlayerHealth>();
        PlayerCombat = GetComponent<PlayerCombat>();
        PlayerVisuals = GetComponent<PlayerVisuals>();
        PlayerEffectManager = GetComponent<PlayerEffectManager>();
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerItemManager = GetComponent<ItemManager>();
        PlayerCoinSystem = GetComponent<CoinSystem>();
        PlayerStamina = GetComponent<StaminaSystem>();
        PlayerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        PlayerStatsManager.Initialize();
        PlayerCoinSystem.Initialize();
        PlayerHealth.Initialize(PlayerStatsManager);
        PlayerStamina.Initialize(PlayerStatsManager);
        PlayerMovement.Initialize(PlayerStatsManager);
        PlayerCombat.Initialize(PlayerStatsManager);
        PlayerItemManager.Initialize(PlayerStatsManager);
        PlayerEffectManager.Initialize(PlayerStatsManager);
        PlayerVisuals.Initialize(PlayerStatsManager.BaseStats, PlayerHealth);
        PlayerController.Initialize(PlayerHealth);
    }
}