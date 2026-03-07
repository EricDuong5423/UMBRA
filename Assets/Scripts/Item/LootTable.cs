using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loot Table", menuName = "Roguelike/Loot Table")]
public class LootTable : ScriptableObject
{
    [Header("Drop settings")]
    public float dropChance = 40f;
    [Header("Rarity Weight")]
    public float commonWeight = 50f;
    public float uncommonWeight = 30f;
    public float rareWeight = 15f;
    public float epicWeight = 4f;
    public float legendaryWeight = 1f;
    [Header("Loot pool")] 
    public List<ItemData> itemsToDrop;
}
