using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/Player Stats")]
public class PlayerStats : EntitesStats
{
    [Header("Player Movement Specifics")]
    public float RollSpeed = 15f;
    public float RollCooldown = 1f;
    public float InvicibleDuration = 0.5f;
    [Header("Light Mechanics (Visuals)")] 
    public float MaxLightRadius = 5f;
    public float MinLightRadius = 1f;
    public float MaxLightIntensity = 1.5f;
    public float MinLightIntensity = 0.5f;
}