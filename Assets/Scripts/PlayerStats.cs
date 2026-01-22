using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/Player Stats")]
public class PlayerStats : EntitesStats
{
    [Header("Player specifics")]
    [SerializeField] private float dashSpeed = 15f;
    public float DashSpeed => dashSpeed;
    [SerializeField] private float dashCooldown = 1f;
    public float DashCooldown => dashCooldown;

    [Header("Light mechanics")] 
    [SerializeField] private float baseEmbers = 100f;
    public float BaseEmbers => baseEmbers;

    [SerializeField] private float lightRadius = 5f;
    public float LightRadius => lightRadius;
}
