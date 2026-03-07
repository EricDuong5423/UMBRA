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
    [Header("Visuals")]
    public Color silhouetteColor = Color.black;
    public Color lightColor = Color.white;
}
