using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/Player Stats")]
public class PlayerStats : EntitesStats
{
    [Header("Player Movement Specifics")]
    public float rollSpeed = 15f;
    public float rollCooldown = 1f;

    [Header("Leveling Settings")]
    public float xpRequirementMultiplier = 1.5f;

    [Header("Light Mechanics (Visuals)")] 
    
    public float maxLightRadius = 15f;
    public float minLightRadius = 1f;
    
    public float maxLightIntensity = 1.5f;
    public float minLightIntensity = 0.5f;
}