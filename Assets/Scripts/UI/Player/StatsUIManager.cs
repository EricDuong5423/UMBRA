using UnityEngine;

public class StatsUIManager : MonoBehaviour
{
    [SerializeField] private BarUI healthBarUI;
    [SerializeField] private BarUI staminaBarUI;
    [SerializeField] private PlayerStatsUI playerStatsUI;
    void Start()
    {
        healthBarUI.Setup();
        staminaBarUI.Setup();
        playerStatsUI.Setup();
    }
}
