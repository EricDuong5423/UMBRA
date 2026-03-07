using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Roguelike/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")] 
    public string Name;
    public ItemRarity Rarity;
    public Sprite Icon;
    [TextArea] 
    public string Description;

    [Header("Stats")] 
    public float bonusMaxEmbers;
    public float bonusMoveSpeed;
    public float bonusAttackDmg;
    public float bonusStamina;
    public float bonusStaminaRegen;
    public float bonusCritRate;
    public float bonusCritDamage;
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic, 
    Legendary,
}
