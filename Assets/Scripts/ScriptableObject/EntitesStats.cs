using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityStats", menuName = "Stats/EntitesStats")]
public class EntitesStats : ScriptableObject
{
    [Header("Base stats level 1")] 
    public float BaseMaxEmbers = 100f;
    public float BaseMoveSpeed = 5f;
    public float BaseAtkDamage = 10f;
    public float BaseMaxStamina = 100f;
    public float BaseStaminaRegen = 5f;
    public float BaseCritRate = 1f;
    public float BaseCritDamage = 10f;
    public float BaseArmor = 20f;
    [Header("Visuals")]
    public Color silhouetteColor = Color.black;
    public Color lightColor = Color.white;
}
