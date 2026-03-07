using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private ItemManager playerInventory;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private ItemSlotUI itemSlotPrefab;

    private Dictionary<ItemData, ItemSlotUI> spawnedSlot = new Dictionary<ItemData, ItemSlotUI>();

    private void Start()
    {
        if (playerInventory != null) playerInventory.OnInvetoryChanged += HandleItemChanged;
    }

    private void OnDestroy()
    {
        if (playerInventory != null) playerInventory.OnInvetoryChanged -= HandleItemChanged;
    }

    private void HandleItemChanged(ItemData item, int currentStack)
    {
        if (spawnedSlot.ContainsKey(item))
        {
            spawnedSlot[item].Setup(item, currentStack);
        }
        else
        {
            ItemSlotUI newSlot = Instantiate(itemSlotPrefab, itemContainer);
            newSlot.Setup(item, currentStack);
            spawnedSlot.Add(item, newSlot);
        }
    }
}
