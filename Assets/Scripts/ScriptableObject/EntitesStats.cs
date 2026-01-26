using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityStats", menuName = "Stats/EntitesStats")]
public class EntitesStats : ScriptableObject
{
    [Header("Base stats level 1")] 
    public float baseMaxEmbers = 100f;
    public float baseMoveSpeed = 5f;
    public float baseAtkDamage = 10f;
    public float baseMaxStamina = 100f;
    public float baseStaminaRegen = 5f;
    [Header("Growth Multipliers")]
    [Range(1f, 2f)] public float healthGrowth = 1.1f; 
    [Range(1f, 2f)] public float damageGrowth = 1.1f;
    [Range(1f, 2f)] public float speedGrowth = 1.0f;
    [Range(1f, 2f)] public float staminaGrowth = 1.0f;
    [Range(1f, 2f)] public float staminaRegenGrowth = 1.02f;
    [Header("Visuals")]
    public Color silhouetteColor = Color.black;
    public Color lightColor = Color.white;
}
