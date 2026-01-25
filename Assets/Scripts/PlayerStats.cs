using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/Player Stats")]
public class PlayerStats : EntitesStats
{
    [Header("Player specifics")]
    [SerializeField] private float rollSpeed = 15f;
    public float RollSpeed => rollSpeed;
    [SerializeField] private float rollCooldown = 1f;
    public float RollCooldown => rollCooldown;

    [Header("Light mechanics")] 
    [SerializeField] private float baseEmbers = 100f;
    public float BaseEmbers => baseEmbers;

    [SerializeField] private float maxLightRadius = 15f;
    public float MaxLightRadius => maxLightRadius;
    [SerializeField] private float minLightRadius = 1f;
    public float MinLightRadius => minLightRadius;
    [SerializeField] private float maxLightIntensity = 1.5f;
    public float MaxLightIntensity => maxLightIntensity;
    [SerializeField] private float minLightIntensity = 0.5f;
    public float MinLightIntensity => minLightIntensity;
}
